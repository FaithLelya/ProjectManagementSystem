using System;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Views.Technicians
{
    public partial class RecordedAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTechnicians();
                LoadProjects();

                // Initial load of all attendance records
                LoadAttendanceRecords();
            }
        }

        private void LoadTechnicians()
        {
            // Load all technicians from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianID, Username FROM Technician";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlTechnicianFilter.Items.Clear();
                    ddlTechnicianFilter.Items.Add(new ListItem("-- All Technicians --", ""));

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddlTechnicianFilter.Items.Add(new ListItem(reader["Username"].ToString(), reader["TechnicianID"].ToString()));
                        }
                    }
                }
            }
        }

        private void LoadProjects()
        {
            // Load projects from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT ProjectId, ProjectName FROM Projects";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddlProjectFilter.Items.Clear();
                    ddlProjectFilter.Items.Add(new ListItem("-- All Projects --", ""));

                    while (reader.Read())
                    {
                        ddlProjectFilter.Items.Add(new ListItem(reader["ProjectName"].ToString(), reader["ProjectId"].ToString()));
                    }
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadAttendanceRecords();
        }

        private void LoadAttendanceRecords()
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();

                    // Base query with date format handling for SQLite
                    string query = @"
                        SELECT a.AttendanceId, t.Username AS Technician, p.ProjectName, 
                               a.Date, a.HoursWorked, a.OvertimeHours, a.IsAbsent, a.Notes,
                               sub.Username AS SubmittedBy
                        FROM Attendance a
                        JOIN Technician t ON a.TechnicianId = t.TechnicianId
                        JOIN Projects p ON a.ProjectId = p.ProjectId
                        JOIN Technician sub ON a.SubmittedBy = sub.TechnicianId
                        WHERE 1=1";

                    // Add filters
                    if (!string.IsNullOrEmpty(ddlTechnicianFilter.SelectedValue))
                    {
                        query += " AND a.TechnicianId = @TechnicianId";
                    }

                    if (!string.IsNullOrEmpty(ddlProjectFilter.SelectedValue))
                    {
                        query += " AND a.ProjectId = @ProjectId";
                    }

                    DateTime startDate, endDate;
                    if (DateTime.TryParse(txtStartDate.Text, out startDate))
                    {
                        // Use date() function in SQLite for proper date comparison
                        query += " AND date(a.Date) >= date(@StartDate)";
                    }

                    if (DateTime.TryParse(txtEndDate.Text, out endDate))
                    {
                        query += " AND date(a.Date) <= date(@EndDate)";
                    }

                    // Add absent/present filter if selected
                    if (ddlAttendanceStatus.SelectedValue == "Present")
                    {
                        query += " AND a.IsAbsent = 0";
                    }
                    else if (ddlAttendanceStatus.SelectedValue == "Absent")
                    {
                        query += " AND a.IsAbsent = 1";
                    }

                    // Order by date descending
                    query += " ORDER BY a.Date DESC, t.Username";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // Add parameter values if filters are applied
                        if (!string.IsNullOrEmpty(ddlTechnicianFilter.SelectedValue))
                        {
                            command.Parameters.AddWithValue("@TechnicianId", int.Parse(ddlTechnicianFilter.SelectedValue));
                        }

                        if (!string.IsNullOrEmpty(ddlProjectFilter.SelectedValue))
                        {
                            command.Parameters.AddWithValue("@ProjectId", int.Parse(ddlProjectFilter.SelectedValue));
                        }

                        if (DateTime.TryParse(txtStartDate.Text, out startDate))
                        {
                            // Format date in ISO format for SQLite
                            command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        }

                        if (DateTime.TryParse(txtEndDate.Text, out endDate))
                        {
                            command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                        }

                        // Execute reader and bind to GridView
                        using (var reader = command.ExecuteReader())
                        {
                            gvAttendance.DataSource = reader;
                            gvAttendance.DataBind();
                        }
                    }
                }

                // Show message if no records found
                if (gvAttendance.Rows.Count == 0)
                {
                    lblMessage.Text = "No attendance records found with the selected filters.";
                    lblMessage.CssClass = "alert alert-info";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading attendance records: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("Exception details: " + ex.ToString());
            }
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Check if the row represents an absent record
                    bool isAbsent = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsAbsent"));

                    // Add a badge indicating presence or absence
                    Label lblStatus = new Label();
                    if (isAbsent)
                    {
                        lblStatus.Text = "Absent";
                        lblStatus.CssClass = "badge bg-danger";

                        // For absent records, display "-" in hours columns
                        e.Row.Cells[4].Text = "-"; // HoursWorked column
                        e.Row.Cells[5].Text = "-"; // OvertimeHours column
                    }
                    else
                    {
                        lblStatus.Text = "Present";
                        lblStatus.CssClass = "badge bg-success";
                    }

                    // Add the badge to the Status column
                    e.Row.Cells[6].Controls.Add(lblStatus);

                    // Handle date conversion properly
                    var dateValue = DataBinder.Eval(e.Row.DataItem, "Date");
                    DateTime date;

                    if (dateValue is long || dateValue is Int64)
                    {
                        // If stored as Unix timestamp (seconds since epoch)
                        long ticks = Convert.ToInt64(dateValue);
                        date = new DateTime(1970, 1, 1).AddSeconds(ticks);
                    }
                    else if (dateValue is string)
                    {
                        // If stored as string
                        date = DateTime.Parse(dateValue.ToString());
                    }
                    else
                    {
                        // Try normal conversion
                        date = Convert.ToDateTime(dateValue);
                    }

                    e.Row.Cells[3].Text = date.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    System.Diagnostics.Debug.WriteLine("Row binding error: " + ex.ToString());

                    // Safe handling - display raw data or placeholder
                    e.Row.Cells[3].Text = DataBinder.Eval(e.Row.DataItem, "Date")?.ToString() ?? "Invalid Date";
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Export the current grid view data to CSV
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=AttendanceRecords.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            // Headers
            Response.Write("Technician,Project,Date,Hours Worked,Overtime Hours,Status,Submitted By,Notes\n");

            // Data rows
            foreach (GridViewRow row in gvAttendance.Rows)
            {
                string technician = row.Cells[1].Text;
                string project = row.Cells[2].Text;
                string date = row.Cells[3].Text;
                string hoursWorked = row.Cells[4].Text;
                string overtimeHours = row.Cells[5].Text;

                // Get the status label text
                string status = "";
                foreach (Control ctrl in row.Cells[6].Controls)
                {
                    if (ctrl is Label)
                    {
                        status = ((Label)ctrl).Text;
                        break;
                    }
                }

                string submittedBy = row.Cells[7].Text;
                string notes = row.Cells[8].Text.Replace(",", " ");  // Remove commas from notes to avoid CSV issues

                Response.Write($"{technician},{project},{date},{hoursWorked},{overtimeHours},{status},{submittedBy},{notes}\n");
            }

            Response.End();
        }

        protected void gvAttendance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Get the attendance ID from the command argument
            int attendanceId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                // Load the attendance record for editing
                LoadAttendanceForEditing(attendanceId);

                // Show the edit modal using JavaScript
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "showEditModal();", true);
            }
            else if (e.CommandName == "DeleteItem")
            {
                // Delete the attendance record
                DeleteAttendance(attendanceId);

                // Reload the grid
                LoadAttendanceRecords();
            }
        }

        private void LoadAttendanceForEditing(int attendanceId)
        {
            try
            {
                // Ensure the technician and project dropdowns are loaded
                if (ddlEditTechnician.Items.Count == 0)
                {
                    LoadTechnicians(ddlEditTechnician);
                }

                if (ddlEditProject.Items.Count == 0)
                {
                    LoadProjects(ddlEditProject);
                }

                hdnAttendanceId.Value = attendanceId.ToString();

                // Fetch the attendance record from the database
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string query = @"
                SELECT a.AttendanceId, a.TechnicianId, a.ProjectId, a.Date, 
                       a.HoursWorked, a.OvertimeHours, a.IsAbsent, a.Notes
                FROM Attendance a
                WHERE a.AttendanceId = @AttendanceId";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AttendanceId", attendanceId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fill the form fields with the attendance data
                                int technicianId = Convert.ToInt32(reader["TechnicianId"]);
                                int projectId = Convert.ToInt32(reader["ProjectId"]);

                                // Set dropdown selections
                                ddlEditTechnician.SelectedValue = technicianId.ToString();
                                ddlEditProject.SelectedValue = projectId.ToString();

                                // Handle date conversion properly
                                var dateValue = reader["Date"];
                                DateTime date;

                                if (dateValue is long || dateValue is Int64)
                                {
                                    // If stored as Unix timestamp (seconds since epoch)
                                    long ticks = Convert.ToInt64(dateValue);
                                    date = new DateTime(1970, 1, 1).AddSeconds(ticks);
                                }
                                else if (dateValue is string)
                                {
                                    // If stored as string
                                    date = DateTime.Parse(dateValue.ToString());
                                }
                                else
                                {
                                    // Try normal conversion
                                    date = Convert.ToDateTime(dateValue);
                                }

                                txtEditDate.Text = date.ToString("yyyy-MM-dd");

                                // Set absence status
                                bool isAbsent = Convert.ToBoolean(reader["IsAbsent"]);
                                chkAbsent.Checked = isAbsent;

                                // Set hours worked and overtime
                                if (!isAbsent)
                                {
                                    txtEditHoursWorked.Text = reader["HoursWorked"].ToString();
                                    txtEditOvertimeHours.Text = reader["OvertimeHours"].ToString();
                                }
                                else
                                {
                                    txtEditHoursWorked.Text = "0";
                                    txtEditOvertimeHours.Text = "0";
                                }

                                // Set notes
                                txtEditNotes.Text = reader["Notes"]?.ToString() ?? string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading attendance record: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("Exception details: " + ex.ToString());
            }
        }

        // Add method to load technicians for the edit dropdown
        private void LoadTechnicians(DropDownList ddl)
        {
            // Load all technicians from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT TechnicianID, Username FROM Technician";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddl.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ddl.Items.Add(new ListItem(reader["Username"].ToString(), reader["TechnicianID"].ToString()));
                        }
                    }
                }
            }
        }

        // Add method to load projects for the edit dropdown
        private void LoadProjects(DropDownList ddl)
        {
            // Load projects from the database and bind to the dropdown
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT ProjectId, ProjectName FROM Projects";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    ddl.Items.Clear();

                    while (reader.Read())
                    {
                        ddl.Items.Add(new ListItem(reader["ProjectName"].ToString(), reader["ProjectId"].ToString()));
                    }
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                int attendanceId = Convert.ToInt32(hdnAttendanceId.Value);
                int technicianId = Convert.ToInt32(ddlEditTechnician.SelectedValue);
                int projectId = Convert.ToInt32(ddlEditProject.SelectedValue);
                DateTime date = DateTime.Parse(txtEditDate.Text);
                bool isAbsent = chkAbsent.Checked;

                decimal hoursWorked = 0;
                decimal overtimeHours = 0;

                if (!isAbsent)
                {
                    hoursWorked = Convert.ToDecimal(txtEditHoursWorked.Text);
                    overtimeHours = Convert.ToDecimal(txtEditOvertimeHours.Text);
                }

                string notes = txtEditNotes.Text;

                // Get current user's technician ID (for tracking who updated the record)
                // Replace this with your actual user authentication logic
                int submittedBy = 1; // Default to admin or current user's ID

                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string query = @"
                UPDATE Attendance
                SET TechnicianId = @TechnicianId,
                    ProjectId = @ProjectId,
                    Date = @Date,
                    HoursWorked = @HoursWorked,
                    OvertimeHours = @OvertimeHours,
                    IsAbsent = @IsAbsent,
                    Notes = @Notes,
                    SubmittedBy = @SubmittedBy
                WHERE AttendanceId = @AttendanceId";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TechnicianId", technicianId);
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                        command.Parameters.AddWithValue("@OvertimeHours", overtimeHours);
                        command.Parameters.AddWithValue("@IsAbsent", isAbsent ? 1 : 0);
                        command.Parameters.AddWithValue("@Notes", notes);
                        command.Parameters.AddWithValue("@SubmittedBy", submittedBy);
                        command.Parameters.AddWithValue("@AttendanceId", attendanceId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Attendance record updated successfully.";
                            lblMessage.CssClass = "alert alert-success";
                            lblMessage.Visible = true;
                        }
                        else
                        {
                            lblMessage.Text = "No changes made to the attendance record.";
                            lblMessage.CssClass = "alert alert-warning";
                            lblMessage.Visible = true;
                        }
                    }
                }

                // Reload the attendance records
                LoadAttendanceRecords();

                // Close the modal using JavaScript
                ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal",
                    "$('#editModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating attendance record: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("Exception details: " + ex.ToString());
            }
        }

        private void DeleteAttendance(int attendanceId)
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
                {
                    connection.Open();
                    string query = "DELETE FROM Attendance WHERE AttendanceId = @AttendanceId";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AttendanceId", attendanceId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Attendance record deleted successfully.";
                            lblMessage.CssClass = "alert alert-success";
                            lblMessage.Visible = true;
                        }
                        else
                        {
                            lblMessage.Text = "Failed to delete attendance record.";
                            lblMessage.CssClass = "alert alert-danger";
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting attendance record: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine("Exception details: " + ex.ToString());
            }
        }
    }
}