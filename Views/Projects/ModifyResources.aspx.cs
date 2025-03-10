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
                        List<Resource> resources = new List<Resource>();
                        while (reader.Read())
                        {
                            resources.Add(new Resource
                            {
                                ResourceName = reader.GetString(0),
                                Quantity = reader.GetInt32(1)
                            });
                        }
                        ResourceRepeater.DataSource = resources;
                        ResourceRepeater.DataBind();
                    }
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtResourceName.Text) || !int.TryParse(txtQuantity.Text, out int quantity))
            {
                lblMessage.Text = "Please enter a valid resource name and quantity.";
                return;
            }

            string resourceName = txtResourceName.Text; // Assuming you have a TextBox for resource name

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

            lblMessage.Text = "Resource added successfully!";
            LoadResources(); // Refresh the resource list
        }
    }
}