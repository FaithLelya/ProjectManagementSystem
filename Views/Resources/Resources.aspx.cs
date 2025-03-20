using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Resources
{
    public partial class Resources : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadResources();
            }
        }

        private void LoadResources()
        {
            var resources = new List<Resource>();
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Resources ORDER BY ResourceName";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Resource resource = new Resource
                            {
                                ResourceId = reader.GetInt32(reader.GetOrdinal("ResourceId")),
                                ResourceName = reader.GetString(reader.GetOrdinal("ResourceName")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                              string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                CostPerunit = reader.IsDBNull(reader.GetOrdinal("CostPerUnit")) ?
                                              0 : reader.GetDecimal(reader.GetOrdinal("CostPerUnit"))
                            };

                            resources.Add(resource);
                        }
                    }
                }
            }

            ResourcesRepeater.DataSource = resources;
            ResourcesRepeater.DataBind();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int resourceId = Convert.ToInt32(btn.CommandArgument);

            // Retrieve resource details for editing
            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Resources WHERE ResourceId = @ResourceId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ResourceId", resourceId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hfResourceId.Value = resourceId.ToString();
                            txtResourceName.Text = reader["ResourceName"].ToString();
                            txtDescription.Text = reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                                 string.Empty : reader["Description"].ToString();
                            txtQuantity.Text = reader["Quantity"].ToString();
                            txtCostPerUnit.Text = reader.IsDBNull(reader.GetOrdinal("CostPerUnit")) ?
                                                 "0.00" : Convert.ToDecimal(reader["CostPerUnit"]).ToString("F2");

                            formTitle.InnerText = "Edit Resource";
                            btnSave.Text = "Update Resource";
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int resourceId = Convert.ToInt32(hfResourceId.Value);
            string resourceName = txtResourceName.Text.Trim();
            string description = txtDescription.Text.Trim();
            int quantity = Convert.ToInt32(txtQuantity.Text);
            decimal costPerUnit = Convert.ToDecimal(txtCostPerUnit.Text);

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                if (resourceId > 0)
                {
                    // Update existing resource
                    string sql = @"UPDATE Resources 
                                  SET ResourceName = @ResourceName, 
                                      Description = @Description, 
                                      Quantity = @Quantity, 
                                      CostPerUnit = @CostPerUnit 
                                  WHERE ResourceId = @ResourceId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResourceId", resourceId);
                        cmd.Parameters.AddWithValue("@ResourceName", resourceName);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerUnit", costPerUnit);

                        cmd.ExecuteNonQuery();

                        // Also update the resource name in ProjectResources table
                        string updateProjectResources = @"UPDATE ProjectResources 
                                                        SET ResourceName = @ResourceName 
                                                        WHERE ResourceId = @ResourceId";

                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateProjectResources, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@ResourceId", resourceId);
                            updateCmd.Parameters.AddWithValue("@ResourceName", resourceName);
                            updateCmd.ExecuteNonQuery();
                        }

                        lblMessage.Text = "Resource updated successfully!";
                    }
                }
                else
                {
                    // Add new resource
                    string sql = @"INSERT INTO Resources (ResourceName, Description, Quantity, CostPerUnit) 
                                  VALUES (@ResourceName, @Description, @Quantity, @CostPerUnit)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResourceName", resourceName);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@CostPerUnit", costPerUnit);

                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "Resource added successfully!";
                    }
                }
            }

            // Reset form
            ResetForm();
            LoadResources();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            hfResourceId.Value = "0";
            txtResourceName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtCostPerUnit.Text = string.Empty;
            formTitle.InnerText = "Add New Resource";
            btnSave.Text = "Save Resource";
            lblMessage.Text = string.Empty;
        }
    }
}