using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class ModifyResources : System.Web.UI.Page
    {
        private int projectId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the project ID from the query string
                if (int.TryParse(Request.QueryString["projectId"], out projectId))
                {
                    LoadProjectDetails();
                    LoadResources();
                }
                else
                {
                    lblMessage.Text = "Invalid project ID.";
                }
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
            // Load existing resources for the project
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT ResourceName, Quantity FROM ProjectResources WHERE ProjectId = @ProjectId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", projectId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<ProjectResource> resources = new List<ProjectResource>();
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

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Get the resource name and quantity
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
                string insertQuery = "INSERT INTO ProjectResources (ProjectId, ResourceName, Quantity) VALUES (@ProjectId, @ResourceName, @Quantity)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.Parameters.AddWithValue("@ResourceName", resourceName);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery();
                }
            }
            // Clear form fields
            txtResourceName.Text = "";
            txtQuantity.Text = "";

            lblMessage.Text = "Resource added successfully!";
            LoadResources(); // Refresh the resource list
        }
    }
}