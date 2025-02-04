<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompensationDetails.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.CompensationDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compensation Details</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Compensation Details</h2>
            <asp:Label ID="lblHourlyRate" runat="server" Text="Hourly Rate: "></asp:Label>
            <asp:Label ID="lblOvertimeRate" runat="server" Text="Overtime Rate: "></asp:Label>
            <asp:Label ID="lblBonuses" runat="server" Text="Bonuses Earned: "></asp:Label>
            <asp:Label ID="lblTotalPayment" runat="server" Text="Total Payment Estimate: "></asp:Label>
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save Compensation Info" OnClick="btnSave_Click" />
        </div>
    </form>
</body>
</html>
