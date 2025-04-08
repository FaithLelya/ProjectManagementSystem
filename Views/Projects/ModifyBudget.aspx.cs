using System;
using System.Data.SQLite;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Projects
{
    public partial class ModifyBudget : System.Web.UI.Page
    {
        private int _projectId;
        private Project _project;
        private string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check for Admin permissions
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                AuthErrorPanel.Visible = true;
                EditPanel.Visible = false;
                return;
            }

            // Get project ID from query parameter
            if (!int.TryParse(Request.QueryString["projectId"], out _projectId))
            {
                Response.Redirect("~/Views/Projects/Projects.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProjectDetails();
            }
        }

        private void LoadProjectDetails()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT ProjectName, Budget FROM Projects WHERE ProjectId = @ProjectId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", _projectId);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblProjectName.Text = reader.GetString(0);
                                decimal budget = reader.GetDecimal(1);

                                decimal totalExpense = CalculateTotalExpense(_projectId);

                                lblCurrentBudget.Text = string.Format("KES {0:N2}", budget);
                                lblTotalExpense.Text = string.Format("KES {0:N2}", totalExpense);
                                txtNewBudget.Text = budget.ToString("F2");
                            }
                            else
                            {
                                Response.Redirect("~/Views/Projects/Projects.aspx");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }

        private decimal CalculateTotalExpense(int projectId)
        {
            decimal totalExpense = 0;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT SUM(Amount) FROM Expenses WHERE ProjectId = @ProjectId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", projectId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            totalExpense = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch
            {
                // Handle exceptions or return 0
            }

            return totalExpense;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                decimal newBudget = decimal.Parse(txtNewBudget.Text);
                string justification = txtJustification.Text;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Update the project budget
                    string updateSql = "UPDATE Projects SET Budget = @Budget WHERE ProjectId = @ProjectId";

                    using (SQLiteCommand cmd = new SQLiteCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Budget", newBudget);
                        cmd.Parameters.AddWithValue("@ProjectId", _projectId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LogBudgetChange(conn, newBudget, justification);

                            EditPanel.Visible = false;
                            SuccessPanel.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }

        private void LogBudgetChange(SQLiteConnection conn, decimal newBudget, string justification)
        {
            string logSql = @"
                INSERT INTO BudgetChangeLog (ProjectId, OldBudget, NewBudget, ChangedBy, ChangeDate, Justification)
                VALUES (@ProjectId, @OldBudget, @NewBudget, @ChangedBy, @ChangeDate, @Justification)";

            try
            {
                decimal oldBudget = decimal.Parse(lblCurrentBudget.Text.Replace("KES ", "").Replace(",", ""));

                using (SQLiteCommand cmd = new SQLiteCommand(logSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectId", _projectId);
                    cmd.Parameters.AddWithValue("@OldBudget", oldBudget);
                    cmd.Parameters.AddWithValue("@NewBudget", newBudget);
                    cmd.Parameters.AddWithValue("@ChangedBy", Session["UserId"]?.ToString() ?? "Unknown");
                    cmd.Parameters.AddWithValue("@ChangeDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Justification", justification);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Log the exception, or handle it better.
                Console.WriteLine($"Error logging budget change: {ex.Message}");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }
    }
}