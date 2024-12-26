<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="ProjectManagementSystem.Views.Projects.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Projects</h2> 
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"> 
                <Columns> 
                    <asp:BoundField DataField="ProjectId" HeaderText="ID" ReadOnly="True" /> 
                    <asp:BoundField DataField="ProjectName" HeaderText="Name" /> 
                    <asp:BoundField DataField="Budget" HeaderText="Budget" /> 
                </Columns> 
            </asp:GridView>
        </div>
    </form>
</body>
</html>
