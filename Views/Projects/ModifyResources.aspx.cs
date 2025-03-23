using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Helpers;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class ModifyResources : System.Web.UI.Page
    {
        private int projectId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize the projectId from the query string
            if (Request.QueryString["projectId"] != null)
            {
                if (!int.TryParse(Request.QueryString["projectId"], out projectId))
                {
                    // Handle invalid ID (redirect or show error)
                    Response.Redirect("~/Views/Projects/Projects.aspx");
                    return;
                }
            }
            else
            {
                // No project ID provided, redirect back
                Response.Redirect("~/Views/Projects/Projects.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadProjectDetails();
                LoadResourceDropdown();
                LoadResources();
            }
        }

        protected void LoadProjectDetails()
        {
            // Load project name
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ProjectName FROM Projects WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    lblProjectName.Text = cmd.ExecuteScalar()?.ToString() ?? "Project not found.";
                }
            }
        }

        protected void LoadResourceDropdown()
        {
            // Load all resources into the dropdown list
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ResourceId, ResourceName, CostPerUnit FROM Resources ORDER BY ResourceName";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        ddlResourceName.Items.Clear();
                        ddlResourceName.Items.Add(new ListItem("-- Select a Resource --", ""));

                        while (reader.Read())
                        {
                            string resourceId = reader["ResourceId"].ToString();
                            string resourceName = reader["ResourceName"].ToString();
                            decimal costPerUnit = Convert.ToDecimal(reader["CostPerUnit"]);

                            ListItem item = new ListItem(resourceName, resourceId);
                            item.Attributes.Add("data-cost", costPerUnit.ToString());
                            ddlResourceName.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void LoadResources()
        {
            List<ProjectResource> resources = SQLiteHelper.GetProjectResources(projectId);

            if (resources.Count == 0)
            {
                ResourceRepeater.Visible = false;
                NoResourcesPanel.Visible = true;
            }
            else
            {
                ResourceRepeater.DataSource = resources;
                ResourceRepeater.DataBind();
                ResourceRepeater.Visible = true;
                NoResourcesPanel.Visible = false;
            }

            // Load all resources assigned to this project
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string sql = "SELECT r.ResourceName, pr.Quantity as QuantityUsed, r.CostPerUnit " +
                             "FROM ProjectResources pr " +
                             "JOIN Resources r ON pr.ResourceId = r.ResourceId " +
                             "WHERE pr.ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<ProjectResource> projectResources = new List<ProjectResource>();
                        while (reader.Read())
                        {
                            ProjectResource resource = new ProjectResource
                            {
                                ResourceName = reader["ResourceName"].ToString(),
                                QuantityUsed = Convert.ToInt32(reader["QuantityUsed"]),
                                CostPerUnit = Convert.ToDecimal(reader["CostPerUnit"])
                            };
                            projectResources.Add(resource);
                        }
                        ResourceRepeater.DataSource = projectResources;
                        ResourceRepeater.DataBind();
                    }
                }
            }
        }
        protected void btnAddResource_Click(object sender, EventArgs e)
        {
            // Now we're using the selected value from dropdown instead of text input
            if (ddlResourceName.SelectedValue == "")
            {
                lblMessage.Text = "Please select a resource.";
                lblMessage.CssClass = "alert alert-danger";
                return;
            }

            int resourceId = Convert.ToInt32(ddlResourceName.SelectedValue);
            string resourceName = ddlResourceName.SelectedItem.Text;
            int quantity;
            decimal costPerUnit;

            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.CssClass = "alert alert-danger";
                return;
            }

            // Get the cost per unit from the database rather than from the textbox
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT CostPerUnit FROM Resources WHERE ResourceId = @ResourceId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ResourceId", resourceId);
                    object result = cmd.ExecuteScalar();

                    if (result == null || !decimal.TryParse(result.ToString(), out costPerUnit))
                    {
                        lblMessage.Text = "Error retrieving cost per unit for the selected resource.";
                        lblMessage.CssClass = "alert alert-danger";
                        return;
                    }
                }
            }

            SQLiteConnection connection = null;
            SQLiteTransaction transaction = null;

            try
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();

                // Begin transaction to ensure both operations succeed or fail together
                transaction = connection.BeginTransaction();

                // Update the quantity in general resources
                string updateResourceQuery = "UPDATE Resources SET Quantity = Quantity + @Quantity " +
                                            "WHERE ResourceId = @ResourceId";

                using (var updateCommand = new SQLiteCommand(updateResourceQuery, connection, transaction))
                {
                    updateCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                    updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                    updateCommand.ExecuteNonQuery();
                }

                // Check if the resource is already assigned to this project
                string checkProjectQuery = "SELECT COUNT(*) FROM ProjectResources WHERE ProjectId = @ProjectId AND ResourceId = @ResourceId";
                using (var checkCommand = new SQLiteCommand(checkProjectQuery, connection, transaction))
                {
                    checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    checkCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // Update the existing resource allocation for this project
                        string updateQuery = "UPDATE ProjectResources SET Quantity = Quantity + @Quantity " +
                                            "WHERE ProjectId = @ProjectId AND ResourceId = @ResourceId";

                        using (var updateCommand = new SQLiteCommand(updateQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            updateCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                            updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Add the new resource allocation for this project
                        string insertQuery = "INSERT INTO ProjectResources (ProjectId, ResourceId, ResourceName, Quantity) " +
                                            "VALUES (@ProjectId, @ResourceId, @ResourceName, @Quantity)";

                        using (var insertCommand = new SQLiteCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            insertCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                            insertCommand.Parameters.AddWithValue("@ResourceName", resourceName);
                            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                // Commit transaction
                transaction.Commit();

                // Clear form fields
                ddlResourceName.SelectedIndex = 0;
                txtQuantity.Text = "";
                txtCostPerUnit.Text = "";

                lblMessage.Text = "Resource added successfully!";
                lblMessage.CssClass = "alert alert-success";
                LoadResources(); // Refresh the resource list
            }
            catch (Exception ex)
            {
                // Roll back transaction if any operation fails
                if (transaction != null && connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // Ignore any errors in rollback
                    }
                }
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
            }
            finally
            {
                // Make sure to clean up resources
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
            }
        }
    }
}