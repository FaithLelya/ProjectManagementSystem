<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeLogging.aspx.cs" Inherits="ProjectManagementSystem.Views.Technicians.TimeLogging" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Field Work Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Time Logging</h2>
            <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        function showSuccessPopup() {
            alert("You have successfully submitted the form and are awaiting approval from the Head Technician.");
        }
    </script>
</head>
</body>
    <form id="form2" runat="server">
        <div>
            <h2>Time Logging</h2>
            <asp:Label ID="Label1" runat="server" Text="User  ID:"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br />

            <asp:Label ID="lblProject" runat="server" Text="Select Project:"></asp:Label>
            <asp:DropDownList ID="ddlProjects" runat="server"></asp:DropDownList>
            <br />

            <asp:GridView ID="gvTimeEntries" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hours Worked">
                        <ItemTemplate>
                            <asp:TextBox ID="txtHours" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />

            <asp:Label ID="lblDeliverables" runat="server" Text="Project Deliverables:"></asp:Label>
            <asp:TextBox ID="txtDeliverables" runat="server" TextMode="MultiLine"></asp:TextBox>
            <br />

            <asp:CheckBox ID="chkAgreeCompensation" runat="server" Text="I agree to HR's compensation rates." />
            <br />
            <asp:CheckBox ID="chkCorrectInfo" runat="server" Text="I confirm that the information entered is correct." />
            <br />

            <asp:Button ID="btnSubmit" runat="server" Text="Submit Field Work Hours" OnClick="btnSubmit_Click" />
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

            <asp:Button ID="btnViewCompensation" runat="server" Text="See Estimated Compensation" OnClick="btnViewCompensation_Click" Visible="false" />
        </div>
    </form>
</body>
</html> <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        function showSuccessPopup() {
            alert("You have successfully submitted the form and are awaiting approval from the Head Technician.");
        }
    </script>
</head>
<body>
    <form id="form3" runat="server">
        <div>
            <h2>Time Logging</h2>
            <asp:Label ID="Label2" runat="server" Text="User  ID:"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <br />

            <asp:Label ID="Label3" runat="server" Text="Select Project:"></asp:Label>
            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
            <br />

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hours Worked">
                        <ItemTemplate>
                            <asp:TextBox ID="txtHours" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />

            <asp:Label ID="Label4" runat="server" Text="Project Deliverables:"></asp:Label>
            <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine"></asp:TextBox>
            <br />

            <asp:CheckBox ID="CheckBox1" runat="server" Text="I agree to HR's compensation rates." />
            <br />
            <asp:CheckBox ID="CheckBox2" runat="server" Text="I confirm that the information entered is correct." />
            <br />

            <asp:Button ID="Button1" runat="server" Text="Submit Field Work Hours" OnClick="btnSubmit_Click" />
            <br />
            <asp:Label ID="Label5" runat="server" Text="" ForeColor="Red"></asp:Label>

            <asp:Button ID="Button2" runat="server" Text="See Estimated Compensation" OnClick="btnViewCompensation_Click" Visible="false" />
        </div>
    </form>
</body>
</html>