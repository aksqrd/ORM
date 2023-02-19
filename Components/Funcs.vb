Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.Web.Mail

Imports System.Drawing
Imports System.Drawing.Imaging

Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json.Linq
Imports System.Globalization
Imports System.Net.Mail
Imports System.Configuration

Public Class Funcs

#Region "Generic Functions"

#End Region

#Region "CMX Production UI"

#End Region

#Region "Credit Cards"

#End Region

#Region "CMX Administration"

#End Region

#Region "Image Manipulation"

#End Region

#Region "Forms"

    Public Function LoadFromAnyDDLB(ByVal vddl As DropDownList,
    ByVal AppConnectionString As String,
    ByVal strSQL As String,
    ByVal sDefault As String,
    ByVal sValue As String,
    ByVal sText As String)

        'vddl = DropDownListBox Ojbect
        'AppConnectionString = connection string to my DB
        'strSQL = pre-built sql to fill listbox
        'sDefault = Value of item selected by default
        'sValue = Column from result set used for the value field in listbox
        'sText = Column from result set used for the display field in listbox

        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection

        sqlConn.ConnectionString = AppConnectionString
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)
        sqlCmd.Connection.Open()

        Dim dReader As SqlDataReader
        dReader = sqlCmd.ExecuteReader

        Dim defaultItem As New System.Web.UI.WebControls.ListItem
        defaultItem.Value = ""
        defaultItem.Text = "Select"

        With vddl
            .DataSource = dReader
            .DataValueField = sValue
            .DataTextField = sText
            If sText = "calDate" Then
                .DataTextFormatString = "{0:d}"
            End If
            If sText = "intervalDesc" Then
                .DataTextFormatString = "{0:t}"
            End If
            .DataBind()
            .Items.Insert(0, defaultItem)
            CType(.DataSource, SqlDataReader).Close()

            If .Items.Count = 0 Then
                .BackColor = System.Drawing.Color.LightGray
                .Enabled = False
            End If

            Dim x As Integer
            For x = 1 To (.Items.Count - 1)
                If .Items(x).Value = sDefault Then
                    .Items(x).Selected = True
                End If
            Next

        End With

        Return vddl
        sqlConn.Close()
        sqlConn.Dispose()

    End Function

#End Region


End Class
