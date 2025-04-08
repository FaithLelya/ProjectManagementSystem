using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI.WebControls;

namespace ProjectManagementSystem.Views.ProjectManagers
{
    public partial class ViewTechnicians : System.Web.UI.Page
    {
        private string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTechnicians();
            }
        }

        private void LoadTechnicians()
        {
            List<Technician> technicians = new List<Technician>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT UserId, UserName, Email FROM User WHERE Role = 'Technician'";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            technicians.Add(new Technician
                            {
                                UserId = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Email = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            if (technicians.Count > 0)
            {
                TechniciansRepeater.DataSource = technicians;
                TechniciansRepeater.DataBind();
                lblNoTechnicians.Visible = false;
            }
            else
            {
                lblNoTechnicians.Visible = true;
            }
        }

        protected void TechniciansRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // You can add any additional item binding logic here if needed
            }
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string userId = btn.CommandArgument;
            // Redirect to technician details page or show modal with more info
            Response.Redirect($"TechnicianDetails.aspx?userId={userId}");
        }
    }
}