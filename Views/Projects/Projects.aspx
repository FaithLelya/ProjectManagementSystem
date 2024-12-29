<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Create Project</h2> 
            <asp:Label ID="lblProjectName" runat="server" Text="Project Name"></asp:Label> 
            <asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox> 
            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label> 
            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox> 
            <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label> 
            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox> 
            <asp:Label ID="lblBudget" runat="server" Text="Budget"></asp:Label> 
            <asp:TextBox ID="txtBudget" runat="server"></asp:TextBox> 
            <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label> 
            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox> 
            <asp:Label ID="lblEndDate" runat="server" Text="End Date"></asp:Label> 
            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox> 
            <asp:Button ID="btnCreateProject" runat="server" Text="Create Project" OnClick="CreateProject_Click" /> 

        </div> 
        <div> 
            <h2>Project List</h2> 
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"> 
                <Columns> 
                    <asp:BoundField DataField="ProjectId" HeaderText="ID" ReadOnly="True" /> 
                    <asp:BoundField DataField="ProjectName" HeaderText="Name" /> 
                    <asp:BoundField DataField="Description" HeaderText="Description" /> 
                    <asp:BoundField DataField="Location" HeaderText="Location" /> 
                    <asp:BoundField DataField="Budget" HeaderText="Budget" /> 
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" /> 
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" /> 
                    <asp:BoundField DataField="Status" HeaderText="Status" /> 

                </Columns> 

            </asp:GridView> 

        </div>
    </form>
</body>
</html>
