<%-- SidebarPartial.ascx --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SidebarPartial.ascx.cs" Inherits="ProjectManagementSystem.Views.Shared.SidebarPartial" %>

<nav id="sidebar" class="bg-primary">
    <div class="sidebar-header p-3 text-center">
        <img src="/Content/images/itdolls_logo.png" alt="IT DOLLS Logo" class="img-fluid mb-2" style="max-width: 100px;" />
        <h3 class="text-white">IT DOLLS PMS</h3>
    </div>

    <div class="sidebar-menu">
        <%-- Common Menu Items --%>
        <asp:LinkButton ID="btnDashboard" runat="server" CssClass="sidebar-item active d-flex align-items-center text-decoration-none" OnClick="btnDashboard_Click">
            <i class="fas fa-home mr-3"></i>
            <span>Dashboard</span>
        </asp:LinkButton>

        <asp:LinkButton ID="btnProjects" runat="server" CssClass="sidebar-item d-flex align-items-center text-decoration-none" OnClick="btnProjects_Click">
            <i class="fas fa-tasks mr-3"></i>
            <span>Projects</span>
        </asp:LinkButton>

        <asp:LinkButton ID="btnResources" runat="server" CssClass="sidebar-item" OnClick="btnResources_Click">
            <i class="fas fa-tools"></i>
            <span>Resources</span>
        </asp:LinkButton>

        <%-- Role-Specific Menu Items --%>
        <%-- Technician-specific Items --%>
        <asp:LinkButton ID="btnAttendance" runat="server" CssClass="sidebar-item" OnClick="btnAttendance_Click" Visible='<%# IsUserTechnician() %>'>
            <i class="fas fa-clock"></i>
            <span>Attendance</span>
        </asp:LinkButton>

        <asp:LinkButton ID="btnPaymentInfo" runat="server" CssClass="sidebar-item" OnClick="btnPaymentInfo_Click" Visible='<%# IsUserTechnician() %>'>
            <i class="fas fa-money-bill-wave"></i>
            <span>Payment Details</span>
        </asp:LinkButton>

        <%-- Project Manager-specific Items --%>
        <asp:LinkButton ID="btnAssignTechnicians" runat="server" CssClass="sidebar-item" OnClick="btnAssignTechnicians_Click" Visible='<%# IsProjectManager() %>'>
            <i class="fas fa-users"></i>
            <span>View Technicians</span>
        </asp:LinkButton>

        <%-- Admin-specific Items --%>
        <asp:LinkButton ID="btnCreateProject" runat="server" CssClass="sidebar-item" OnClick="btnCreateProject_Click" Visible='<%# IsAdmin() %>'>
            <i class="fas fa-plus-circle"></i>
            <span>Create Project</span>
        </asp:LinkButton>

        <asp:LinkButton ID="btnReports" runat="server" CssClass="sidebar-item" OnClick="btnReports_Click" Visible='<%# IsAdmin() %>'>
            <i class="fas fa-chart-bar"></i>
            <span>Reports</span>
        </asp:LinkButton>

        <asp:LinkButton ID="btnCreateUser" runat="server" CssClass="sidebar-item" OnClick="btnCreateUser_Click" Visible='<%# IsAdmin() %>'>
            <i class="fas fa-user-plus"></i>
            <span>Create User</span>
        </asp:LinkButton>

        <%-- Common Account Settings --%>
        <asp:LinkButton ID="btnAccount" runat="server" CssClass="sidebar-item" OnClick="btnAccount_Click">
            <i class="fas fa-user-cog"></i>
            <span>My Account</span>
        </asp:LinkButton>
    </div>

    <div class="sidebar-footer mt-auto p-3">
        <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn btn-outline-light w-100" OnClick="btnLogout_Click">
            <i class="fas fa-sign-out-alt mr-2"></i>Logout
        </asp:LinkButton>
    </div>
</nav>