using ProjectManagementSystem.Models;
using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace ProjectManagementSystem.Views.ProjectManagers
{
    public partial class TechnicianDetails : System.Web.UI.Page
    {
        private string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["userId"] != null)
                {
                    string userId = Request.QueryString["userId"];
                    LoadTechnicianDetails(userId);
                    LoadAssignedProjects(userId);
                }
                else
                {
                    // Redirect if no user ID is provided
                    Response.Redirect("ViewTechnicians.aspx");
                }
            }
        }

        private void LoadTechnicianDetails(string userId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT UserId, UserName, Email FROM User WHERE UserId = @UserId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTechnicianName.InnerText = reader["UserName"].ToString();
                            lblTechnicianId.InnerText = "ID: " + reader["UserId"].ToString();
                            lblEmail.InnerText = reader["Email"].ToString();
                        }
                        else
                        {
                            // Technician not found, redirect back
                            Response.Redirect("ViewTechnicians.aspx");
                        }
                    }
                }
            }
        }

        private void LoadAssignedProjects(string userId)
        {
            List<ProjectAssignment> projects = new List<ProjectAssignment>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT p.ProjectId, p.ProjectName, p.Status 
                                FROM Projects p
                                INNER JOIN ProjectAssignment pa ON p.ProjectId = pa.ProjectId
                                WHERE pa.UserId = @UserId";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(new ProjectAssignment
                            {
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }

            if (projects.Count > 0)
            {
                ProjectsRepeater.DataSource = projects;
                ProjectsRepeater.DataBind();
                lblNoProjects.Visible = false;
            }
            else
            {
                lblNoProjects.Visible = true;
            }
        }
    }

    public class ProjectAssignment
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
    }
}