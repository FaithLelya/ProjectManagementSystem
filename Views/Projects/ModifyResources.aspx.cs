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
        
            // Load all available resources
           using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string sql = "SELECT r.ResourceName, pr.Quantity as QuantityUsed FROM ProjectResources pr " +
                             "JOIN Resources r ON pr.ResourceId = r.ResourceId " +
                             "WHERE pr.ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        //List<ProjectResource> resources = new List<ProjectResource>();
                        while (reader.Read())
                        {
                            ProjectResource resource = new ProjectResource
                            {
                                ResourceName = reader["ResourceName"].ToString(),
                                QuantityUsed = Convert.ToInt32(reader["QuantityUsed"])
                            };
                            resources.Add(resource);
                        }
                        ResourceRepeater.DataSource = resources;
                        ResourceRepeater.DataBind();
                    }
                }
            }
        }
        protected void btnAddResource_Click(object sender, EventArgs e)
        {
            string resourceName = txtResourceName.Text.Trim();
            int quantity;

            if (string.IsNullOrEmpty(resourceName))
            {
                lblMessage.Text = "Please enter a resource name.";
                lblMessage.CssClass = "alert alert-danger";
                return;
            }
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.CssClass = "alert alert-danger";
                return;
            }
            // Add the resource to the database
            // You might need to implement this method in SQLiteHelper
            // bool success = SQLiteHelper.AddResourceToProject(projectId, resourceName, quantity);

            bool success = true; // Replace with actual call to add resource

            if (success)
            {
                lblMessage.Text = "Resource added successfully.";
                lblMessage.CssClass = "alert alert-success";

                // Clear the form
                txtResourceName.Text = "";
                txtQuantity.Text = "";

                // Reload resources
                LoadResources();
            }
            else
            {
                lblMessage.Text = "Failed to add resource.";
                lblMessage.CssClass = "alert alert-danger";
            }
        }


        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Get selected resource ID and quantity
            string resourceName = txtResourceName.Text.Trim();
            int quantity;

            if (string.IsNullOrEmpty(resourceName))
            {
                lblMessage.Text = "Please enter a resource name.";
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();

                // Begin transaction to ensure both operations succeed or fail together
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try 
                        {
                            int resourceId = 0;
                            bool resourceExists = false;

                            // Check if the resource already exists in general resources
                            string checkQuery = "SELECT ResourceId FROM Resources WHERE ResourceName = @ResourceName";
                            using (var checkCommand = new SQLiteCommand(checkQuery, connection))
                            {
                                checkCommand.Parameters.AddWithValue("@ResourceName", resourceName);
                                var result = checkCommand.ExecuteScalar();

                                if (result != null)
                                {
                                    resourceId = Convert.ToInt32(result);
                                    resourceExists = true;
                                }
                            }

                            // If resource doesn't exist, add it to general resources
                            if (!resourceExists)
                            {
                                string insertResourceQuery = "INSERT INTO Resources (ResourceName, Description, Quantity, CostPerUnit) " +
                                                            "VALUES (@ResourceName, '', @Quantity, 0); " +
                                                            "SELECT last_insert_rowid();";

                                using (var insertCommand = new SQLiteCommand(insertResourceQuery, connection))
                                {
                                    insertCommand.Parameters.AddWithValue("@ResourceName", resourceName);
                                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    resourceId = Convert.ToInt32(insertCommand.ExecuteScalar());
                                }
                            }
                            else
                            {
                                // Update the quantity in general resources if it already exists
                                string updateResourceQuery = "UPDATE Resources SET Quantity = Quantity + @Quantity " +
                                                            "WHERE ResourceId = @ResourceId";

                                using (var updateCommand = new SQLiteCommand(updateResourceQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                                    updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    updateCommand.ExecuteNonQuery();
                                }
                            }

                            // Check if the resource is already assigned to this project
                            string checkProjectQuery = "SELECT COUNT(*) FROM ProjectResources WHERE ProjectId = @ProjectId AND ResourceId = @ResourceId";
                            using (var checkCommand = new SQLiteCommand(checkProjectQuery, connection))
                            {
                                checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                                checkCommand.Parameters.AddWithValue("@ResourceId", resourceId);
                                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                                if (count > 0)
                                {
                                    // Update the existing resource allocation for this project
                                    string updateQuery = "UPDATE ProjectResources SET Quantity = Quantity + @Quantity " +
                                                        "WHERE ProjectId = @ProjectId AND ResourceId = @ResourceId";

                                    using (var updateCommand = new SQLiteCommand(updateQuery, connection))
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

                                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
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
                            txtResourceName.Text = "";
                            txtQuantity.Text = "";

                            lblMessage.Text = "Resource added successfully!";
                            LoadResources(); // Refresh the resource list
                        }
                        catch (Exception ex)
                        {
                            // Roll back transaction if any operation fails
                            transaction.Rollback();
                            lblMessage.Text = "Error: " + ex.Message;
                        }
                }
            }

            lblMessage.Text = "Resource allocation updated successfully!";
        }
    }
}