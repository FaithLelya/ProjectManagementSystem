<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnicianDetails.aspx.cs" Inherits="ProjectManagementSystem.Views.ProjectManagers.TechnicianDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Technician Details</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        .profile-header {
            background: linear-gradient(135deg, #4361ee, #3f37c9);
            color: white;
            border-radius: 10px;
        }
        .avatar-lg {
            width: 120px;
            height: 120px;
            font-size: 48px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-4">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="fas fa-user-cog me-2"></i>Technician Details</h2>
                <a href="ViewTechnicians.aspx" class="btn btn-outline-secondary">
                    <i class="fas fa-arrow-left me-2"></i>Back to Technicians
                </a>
            </div>

            <div class="card shadow">
                <div class="profile-header p-4 text-center">
                    <div class="avatar avatar-lg bg-white text-primary rounded-circle d-inline-flex align-items-center justify-content-center mb-3">
                        <i class="fas fa-user"></i>
                    </div>
                    <h3 runat="server" id="lblTechnicianName"></h3>
                    <p class="mb-0" runat="server" id="lblTechnicianId"></p>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-4">
                                <h5><i class="fas fa-info-circle me-2 text-primary"></i>Basic Information</h5>
                                <hr />
                                <div class="mb-3">
                                    <label class="fw-bold">Email Address</label>
                                    <p runat="server" id="lblEmail" class="form-control-static"></p>
                                </div>
                                <div class="mb-3">
                                    <label class="fw-bold">Role</label>
                                    <p class="form-control-static">Technician</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="mb-4">
                                <h5><i class="fas fa-tasks me-2 text-primary"></i>Assigned Projects</h5>
                                <hr />
                                <asp:Repeater ID="ProjectsRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="card mb-2">
                                            <div class="card-body py-2">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <span><%# Eval("ProjectName") %></span>
                                                    <span class="badge bg-primary"><%# Eval("Status") %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblNoProjects" runat="server" Text="No projects assigned" CssClass="text-muted" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>