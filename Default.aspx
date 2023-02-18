<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ORM._Default" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ORM</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style>
        H1 {
            FONT-WEIGHT: bold;
            FONT-SIZE: 18pt
        }

        .tbl_HeaderRow {
            BORDER-RIGHT: 1px solid;
            BORDER-TOP: 1px solid;
            FONT-WEIGHT: bold;
            FONT-SIZE: 12pt;
            BORDER-LEFT: 1px solid;
            BORDER-BOTTOM: 1px solid;
            BACKGROUND-COLOR: #c0c0c0
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <h1>The Code Generator: By Adam Kiger</h1>
        <p>Pointed at a SQL database table, this creates code for:</p>
        <ul>
            <li>
            A VB.NET or C# Object Class
				<li>
            A VB.NET or C# Insert/Update Class
				<li>
            An InsertUpdate SQL stored procedure</FONT>
				<li>A SQL select stored procedure</li>
        </ul>
        <table id="Table1" cellspacing="0" cellpadding="2" border="1" width="900">
            <tr class="tbl_HeaderRow">
                <td colspan="2">Database:</td>
            </tr>
            <tr>
                <td width="25%" nowrap>Target Database</td>
                <td width="75%">&nbsp;
						<asp:DropDownList ID="drpDBs" runat="server" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="ChangeDB">
                            <asp:ListItem Text="Select..." Value="" />
						</asp:DropDownList>
                </td>
            </tr>
            <tr class="tbl_HeaderRow">
                <td colspan="2">Configuration:</td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate for APIs?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_api" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate for C#?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_csharp" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate All Tables?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_All" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate Stored Procs Only?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_prc" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate Business Classes Only?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_bc" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate DAL Classes Only?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_dal" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Generate Count Classes Only?</td>
                <td width="75%">&nbsp;
						<asp:CheckBox ID="chk_cnt" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <!--txt_AppConnectionString-->
            <tr>
                <td width="25%" nowrap>VB.NET ConfigurationSettings AppSettings Value</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_AppConnectionString" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>C# ConfigurationSettings AppSettings Value</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_CSharpAppConnectionString" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>DB Connection String</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_ConStr" runat="server" Width="400px" AutoPostBack="True" OnTextChanged="ChangeTables"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Target Table</td>
                <td width="75%">&nbsp;
						<asp:DropDownList ID="drp_targetTable" runat="server" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="FillBoxes"></asp:DropDownList>
                </td>
            </tr>
            <tr class="tbl_HeaderRow">
                <td colspan="2">Create Code with these names:</td>
            </tr>
            <tr>
                <td width="25%" nowrap>Object Class Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_ObjClassName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>IU Class Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_IUClassName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Count Class Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_CountClassName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>IU Sproc Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_IUSprocName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Select Sproc Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_SelectSprocName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="25%" nowrap>Count Sproc Name</td>
                <td width="75%">&nbsp;
						<asp:TextBox ID="txt_CountSprocName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">

                    <p>&nbsp;</p>
                    <p align="right">
                        <asp:ImageButton ID="btn_Submit" AlternateText="Submit" runat="server" ImageUrl="/images/submit.gif" OnClick="btn_Submit_Click"></asp:ImageButton>&nbsp;
							<asp:ImageButton ID="btn_Reset" AlternateText="Clear" runat="server" ImageUrl="/images/reset.gif" OnClick="btn_Reset_Click"></asp:ImageButton>
                    </p>
                </td>
            </tr>
        </table>
        <p>&nbsp;</p>
        <p>      <asp:Panel ID="pnl_Results" runat="server" Visible="False">
        </p>
        <h2>Results
				<hr align="left" width="900">
        </h2>
        <pre>
				<asp:literal id="ltl_CodeResults" runat="server"></asp:literal></pre>
        <hr align="left" width="900">
        </asp:panel>
    </form>
</body>
</html>

