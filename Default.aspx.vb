Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Public Class _Default
    Inherits System.Web.UI.Page
    Protected DBConStr As String
    Public AppConnectString As String = "AppConnectionString"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here


        If Not Page.IsPostBack Then


            Session("appConString") = ""

            For Each connectionString As ConnectionStringSettings In ConfigurationManager.ConnectionStrings
                'Console.WriteLine(connectionString.Name)

                Dim newListItem As ListItem
                newListItem = New ListItem(connectionString.Name, connectionString.Name)
                If connectionString.Name.IndexOf("LocalSqlServer") = -1 Then
                    drpDBs.Items.Add(newListItem)
                End If


            Next




        End If


    End Sub


    Public Sub ChangeDB(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If drpDBs.SelectedItem.Value <> "" Then


            txt_ObjClassName.Text = ""
            txt_IUSprocName.Text = ""
            txt_IUClassName.Text = ""
            txt_SelectSprocName.Text = ""
            txt_CountClassName.Text = ""
            txt_CountSprocName.Text = ""
            txt_AppConnectionString.Text = ""
            txt_CSharpAppConnectionString.Text = ""

            pnl_Results.Visible = False
            ltl_CodeResults.Text = ""


            AppConnectString = drpDBs.SelectedItem.Value
            Session("appConString") = AppConnectString

            txt_AppConnectionString.Text = "ConfigurationManager.AppSettings(""" & AppConnectString & """)"
            txt_CSharpAppConnectionString.Text = "ConfigurationManager.AppSettings[""" & AppConnectString & """]"
            txt_ConStr.Text = ConfigurationManager.AppSettings("" & AppConnectString & "")


            Dim strSQL As String

            strSQL = "SELECT INFORMATION_SCHEMA.TABLES.TABLE_NAME AS TableName "
            strSQL += "                FROM dbo.sysobjects INNER JOIN  "
            strSQL += "INFORMATION_SCHEMA.TABLES ON dbo.sysobjects.name =  "
            strSQL += "INFORMATION_SCHEMA.TABLES.TABLE_NAME  "
            strSQL += "                WHERE ((TABLE_TYPE = 'BASE TABLE') AND  "
            strSQL += "(INFORMATION_SCHEMA.TABLES.TABLE_NAME<>'dtproperties'))   ORDER BY TableName ASC  "

            LoadFromAnyDDLB(drp_targetTable, ConfigurationManager.AppSettings("" & AppConnectString & ""), strSQL, 0, "TableName", "TableName")

        End If


    End Sub

    Public Sub ChangeTables(ByVal Sender As Object, ByVal E As EventArgs) Handles txt_ConStr.TextChanged
        Dim strSQL As String

        strSQL = "SELECT INFORMATION_SCHEMA.TABLES.TABLE_NAME AS TableName "
        strSQL += "                FROM dbo.sysobjects INNER JOIN  "
        strSQL += "INFORMATION_SCHEMA.TABLES ON dbo.sysobjects.name =  "
        strSQL += "INFORMATION_SCHEMA.TABLES.TABLE_NAME  "
        strSQL += "                WHERE ((TABLE_TYPE = 'BASE TABLE') AND  "
        strSQL += "(INFORMATION_SCHEMA.TABLES.TABLE_NAME<>'dtproperties'))   ORDER BY TableName ASC  "

        If chk_All.Checked Then
            chk_All.Checked = False
        End If

        LoadFromAnyDDLB(drp_targetTable, txt_ConStr.Text, strSQL, 0, "TableName", "TableName")

        txt_ObjClassName.Text = ""
        txt_IUSprocName.Text = ""
        txt_IUClassName.Text = ""
        txt_SelectSprocName.Text = ""
        txt_CountClassName.Text = ""
        txt_CountSprocName.Text = ""
        'txt_AppConnectionString.Text = ""
        'txt_CSharpAppConnectionString.Text = ""

        pnl_Results.Visible = False
        ltl_CodeResults.Text = ""
    End Sub


    Public Sub FillBoxes(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Dim AppConnectString As String = "AppConnectionString"

        AppConnectString = Session("appConString")

        txt_ObjClassName.Text = drp_targetTable.SelectedValue
        txt_IUSprocName.Text = "prc_IU_" & drp_targetTable.SelectedValue
        txt_IUClassName.Text = "IU_" & drp_targetTable.SelectedValue
        If chk_api.Checked Then
            txt_SelectSprocName.Text = "api_Get_" & drp_targetTable.SelectedValue
        Else
            txt_SelectSprocName.Text = "prc_Get_" & drp_targetTable.SelectedValue
        End If
        txt_CountClassName.Text = "Count_" & drp_targetTable.SelectedValue
        txt_CountSprocName.Text = "prc_Count_" & drp_targetTable.SelectedValue
        txt_AppConnectionString.Text = "ConfigurationManager.AppSettings(""" & AppConnectString & """)"
        txt_CSharpAppConnectionString.Text = "ConfigurationManager.ConnectionStrings[""" & AppConnectString & """].ConnectionString"

        pnl_Results.Visible = False
        ltl_CodeResults.Text = ""
    End Sub




    Sub btn_Reset_Click(ByVal sender As Object, ByVal a As ImageClickEventArgs) Handles btn_Reset.Click
        Response.Redirect("Default.aspx")
    End Sub

    Sub btn_Submit_Click(ByVal sender As Object, ByVal a As ImageClickEventArgs) Handles btn_Submit.Click

        If Page.IsPostBack Then
            Page.Validate()

            If Page.IsValid Then

                If drpDBs.SelectedItem.Value <> "" Then



                    AppConnectString = Session("appConString")

                    ltl_CodeResults.Text = ""

                    Dim SP As Boolean = chk_prc.Checked
                    Dim BC As Boolean = chk_bc.Checked
                    Dim DAL As Boolean = chk_dal.Checked
                    Dim CNT As Boolean = chk_cnt.Checked
                    Dim Switch As Integer = 0

                    If SP Then
                        Switch = 1
                    End If

                    If BC Then
                        Switch = 2
                    End If

                    If DAL Then
                        Switch = 3
                    End If

                    If CNT Then
                        Switch = 4
                    End If

                    If Not Switch > 0 Then
                        Switch = 0
                    End If

                    If chk_All.Checked Then



                        DBConStr = txt_ConStr.Text
                        Dim strSQL As String

                        strSQL = "SELECT INFORMATION_SCHEMA.TABLES.TABLE_NAME AS TableName "
                        'strSQL += "TableName,  "
                        'strSQL += "                        (SELECT REPLACE(CAST  "
                        'strSQL += "(sysproperties.value AS NVARCHAR(255)), ',', ';')  "
                        'strSQL += "                                FROM sysproperties  "
                        'strSQL += "                                WHERE ((sysobjects.id =  "
                        'strSQL += "sysproperties.id) AND (sysproperties.type = 3))) AS  "
                        'strSQL += "Description  "
                        strSQL += "                FROM dbo.sysobjects INNER JOIN  "
                        strSQL += "INFORMATION_SCHEMA.TABLES ON dbo.sysobjects.name =  "
                        strSQL += "INFORMATION_SCHEMA.TABLES.TABLE_NAME  "
                        strSQL += "                WHERE ((TABLE_TYPE = 'BASE TABLE') AND  "
                        strSQL += "(INFORMATION_SCHEMA.TABLES.TABLE_NAME<>'dtproperties'))  "


                        Dim DataReader As SqlDataReader
                        Dim sqlConn As New SqlConnection(DBConStr)
                        Dim sqlCmd As New SqlClient.SqlCommand(strSQL, sqlConn)
                        Try
                            sqlCmd.Connection.Open()
                            DataReader = sqlCmd.ExecuteReader()


                            Dim DBConn = ConfigurationManager.ConnectionStrings("" & AppConnectString & "").ConnectionString                  '"dbo.Person"
                            Dim DBCSB = New SqlConnectionStringBuilder(DBConn)
                            Dim DBName As String = DBCSB.InitialCatalog

                            Dim TableName As String
                            Dim ClassNameHere As String
                            Dim StoredProcName As String
                            Dim IU2Use As String
                            Dim LoadStoredProcName As String
                            Dim Count2Use As String
                            Dim CountStoredProcName As String
                            Dim TempAppConnectionString As String
                            Dim TempCSharpAppConnectionString As String

                            While DataReader.Read()

                                TableName = DataReader.Item("TableName")                             '"dbo.Person"
                                ClassNameHere = DataReader.Item("TableName")                               '"MBR_Transaction"
                                StoredProcName = "prc_IU_" & DataReader.Item("TableName")                              '"sp_IU_MBR_Transaction"
                                IU2Use = "IU_" & DataReader.Item("TableName")                             '"IU_MBR_Transaction"
                                If chk_api.Checked Then
                                    LoadStoredProcName = "api_Get_" & DataReader.Item("TableName")                           '"Get_MBR_Transaction"
                                Else
                                    LoadStoredProcName = "prc_Get_" & DataReader.Item("TableName")                           '"Get_MBR_Transaction"
                                End If

                                Count2Use = "Count_" & DataReader.Item("TableName")                         '"Count_PCPW_RecallCodes_TYP"
                                CountStoredProcName = "prc_Count_" & DataReader.Item("TableName")                             '"prc_Count_PCPW_RecallCodes_TYP"
                                TempAppConnectionString = txt_AppConnectionString.Text
                                TempCSharpAppConnectionString = txt_CSharpAppConnectionString.Text

                                pnl_Results.Visible = True
                                ltl_CodeResults.Text += MakeClassesAndIUs(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)


                            End While


                            txt_ObjClassName.Text = ""
                            txt_IUSprocName.Text = ""
                            txt_IUClassName.Text = ""
                            txt_SelectSprocName.Text = ""
                            txt_CountClassName.Text = ""
                            txt_CountSprocName.Text = ""
                            txt_AppConnectionString.Text = ""
                            txt_CSharpAppConnectionString.Text = ""

                            LoadFromAnyDDLB(drp_targetTable, txt_ConStr.Text, strSQL, 0, "TableName", "TableName")

                        Catch ex As System.Exception
                            Throw New System.Exception(ex.ToString())
                        Finally
                            If sqlConn.State = Data.ConnectionState.Open Then
                                sqlConn.Close()
                                sqlConn.Dispose()
                            End If
                        End Try

                    Else

                        DBConStr = txt_ConStr.Text

                        Dim DBConn = ConfigurationManager.ConnectionStrings("" & AppConnectString & "").ConnectionString                  '"dbo.Person"
                        Dim DBCSB = New SqlConnectionStringBuilder(DBConn)
                        Dim DBName As String = DBCSB.InitialCatalog

                        Dim TableName As String = drp_targetTable.SelectedValue                   '"dbo.Person"
                        Dim ClassNameHere As String = txt_ObjClassName.Text                  '"MBR_Transaction"
                        Dim StoredProcName As String = txt_IUSprocName.Text                  '"sp_IU_MBR_Transaction"
                        Dim IU2Use As String = txt_IUClassName.Text                 '"IU_MBR_Transaction"
                        Dim LoadStoredProcName As String = txt_SelectSprocName.Text                   '"Get_MBR_Transaction"
                        Dim Count2Use As String = txt_CountClassName.Text                    '"Count_PCPW_RecallCodes_TYP"
                        Dim CountStoredProcName As String = txt_CountSprocName.Text                  '"prc_Count_PCPW_RecallCodes_TYP"
                        Dim TempAppConnectionString As String = txt_AppConnectionString.Text
                        Dim TempCSharpAppConnectionString = txt_CSharpAppConnectionString.Text

                        pnl_Results.Visible = True
                        ltl_CodeResults.Text = MakeClassesAndIUs(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                    End If

                End If

            End If
        End If

    End Sub




    Public Function MakeClassesAndIUs(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        'strSQL += "Select * From " & TableName


        'sqlConn.ConnectionString = DBConStr
        'sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            'sqlCmd.Connection.Open()
            'DataReader = sqlCmd.ExecuteReader

            'rept_column_count = DataReader.FieldCount
            'counter = 0

            'HttpContext.Current.Response.Write(DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize"))
            'HttpContext.Current.Response.End()

            '*************************************************************************
            'Stored procs only
            '*************************************************************************
            If Switch = 1 Then


                Target += MakeIUStoredProcedure(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                Target += MakeLoadGETStoredProceudre(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                Target += MakeCountStoredProceudre(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf




            End If




            '*************************************************************************
            'Generate DAL only
            '*************************************************************************
            If Switch = 3 Then



                Target += MakeIUDALClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                Target += vbCrLf & vbCrLf & vbCrLf



            End If



            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf


            '*************************************************************************
            'Generate Count Class only
            '*************************************************************************
            If Switch = 4 Then


                Target += MakeCountBLClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf



            End If



            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf



            '*************************************************************************
            'Generate BC only
            '*************************************************************************
            If Switch = 2 Then


                Target += MakeBLClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************


                'Dim fileName As String = "C:\Users\akiger\source\repos\Github\TAGC\Components\BusinessClasses.vb"
                'Dim lines As String() = File.ReadAllLines(fileName)
                'For i As Integer = 0 To lines.Length - 1
                '    Dim line As String = lines(i)
                '    If line.StartsWith("'End of File X") Then
                '        lines(i) = Target & line(i)
                '    End If
                'Next
                'File.WriteAllLines(fileName, lines)


                'Dim TheNewFile As String = ""
                'Dim MyLine As String
                'Dim MyStream2 As New FileStream("C:\Users\akiger\source\repos\Github\TAGC\Components\BusinessClasses.vb", FileMode.Open)
                'Dim MyReader As New StreamReader(MyStream2)
                'Dim MySettings As New StringReader(MyReader.ReadToEnd)
                'MyReader.BaseStream.Seek(0, SeekOrigin.Begin)
                'MyReader.Close()
                'MyStream2.Close()
                'Try
                '    Do
                '        MyLine = MySettings.ReadLine
                '        'Response.Write(MyLine & vbCrLf)
                '        'This if statement is an exit parameter. It can be if it contains or if 5 consecutive lines are nothing. It could be a number of things
                '        If MyLine Is Nothing Then Exit Do
                '        'This is the file you will write. You could do if MyLine = "Test" Then ........... append whatever and however you need to

                '        If MyLine.IndexOf("'End of File") > -1 Then
                '            TheNewFile = TheNewFile & Target & MyLine & vbCrLf
                '        Else
                '            TheNewFile += MyLine
                '        End If


                '    Loop
                'Catch ex As Exception
                '    'MsgBox(ex.ToString())
                'End Try
                ''-----------------Write The new file!!!----------------
                'Dim MyStream3 As New FileStream("C:\Users\akiger\source\repos\Github\TAGC\Components\BusinessClasses.vb", FileMode.Create)
                'Dim MyWriter3 As New StreamWriter(MyStream3)
                'MyWriter3.Write(TheNewFile)
                'MyWriter3.Close()
                'MyStream3.Close()



            End If



            '*************************************************************************
            'Generate SPs, BL and DAL for selected table
            '*************************************************************************
            If Switch = 0 Then


                Target += MakeIUStoredProcedure(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                Target += MakeLoadGETStoredProceudre(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                Target += MakeCountStoredProceudre(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************


                Target += MakeCountBLClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************


                Target += MakeIUDALClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************


                Target += MakeBLClass(DBName, TableName, ClassNameHere, StoredProcName, IU2Use, LoadStoredProcName, Switch, Count2Use, CountStoredProcName, TempAppConnectionString, TempCSharpAppConnectionString)

                Target += vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************







                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf

                'Target += "[{" & vbCrLf & vbCrLf

                'For counter = 0 To (rept_column_count - 1)

                '    Target += """" & DataReader.GetName(counter) & """: """"," & vbCrLf

                'Next

                'Target += vbCrLf & "}]" & vbCrLf & vbCrLf

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf







                'For counter = 1 To (rept_column_count - 1)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        Target += "Protected WithEvents drp" & DataReader.GetName(counter) & " As System.Web.UI.WebControls.DropDownList" & vbCrLf
                '    Else
                '        Target += "Protected WithEvents txt" & DataReader.GetName(counter) & " As System.Web.UI.WebControls.TextBox" & vbCrLf
                '    End If
                '    Target += "Protected WithEvents  rfv" & DataReader.GetName(counter) & " As System.Web.UI.WebControls.RequiredFieldValidator" & vbCrLf
                'Next

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf

                'Target += "<table width=""750"" border=""0"" cellspacing=""0"" cellpadding=""0"">" & vbCrLf

                'For counter = 1 To (rept_column_count - 1)
                '    Target += "<tr>" & vbCrLf
                '    Dim RequiredFieldString As String
                '    Dim Title As String = DataReader.GetName(counter).ToString
                '    Title = Replace(Title, "ap_", "")
                '    Title = Replace(Title, "sp_", "")

                '    Target += "<td width=""35%""><sup><font size=""1"" color=""red"">*</font></sup>&nbsp;<font size=""2"">" & Title & ":</font></td>" & vbCrLf
                '    Target += "<td>"

                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        Target += "?asp:dropdownlist id=""drp" & DataReader.GetName(counter) & """ runat=""server"">?/asp:dropdownlist>"
                '        RequiredFieldString = "?asp:requiredfieldvalidator id=""rfv" & DataReader.GetName(counter) & """ runat=""server"" errormessage=""Please make a selection"" display=""Dynamic"" controltovalidate=""drp" & DataReader.GetName(counter) & """ font-size=""X-Small"">?/asp:requiredfieldvalidator>"
                '    Else
                '        Target += "?asp:textbox id=""txt" & DataReader.GetName(counter) & """ runat=""server"">?/asp:textbox>"
                '        RequiredFieldString = "?asp:requiredfieldvalidator id=""rfv" & DataReader.GetName(counter) & """ runat=""server"" errormessage=""" & Title & " is required"" display=""Dynamic"" controltovalidate=""txt" & DataReader.GetName(counter) & """ font-size=""X-Small"">?/asp:requiredfieldvalidator>"
                '    End If

                '    Target += "</td>" & vbCrLf
                '    Target += "</tr>" & vbCrLf
                '    Target += "<tr>" & vbCrLf
                '    Target += "<td colspan=""2"">" & RequiredFieldString & "</td>" & vbCrLf
                '    Target += "</tr>" & vbCrLf
                'Next
                'Target += "</table>" & vbCrLf

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                'For counter = 1 To (rept_column_count - 1)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        Target += "?asp:dropdownlist id=""drp" & DataReader.GetName(counter) & """ runat=""server"">?/asp:dropdownlist>" & vbCrLf
                '    Else
                '        Target += "?asp:textbox id=""txt" & DataReader.GetName(counter) & """ runat=""server"">?/asp:textbox>" & vbCrLf
                '    End If
                'Next

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf


                'Target += "If Page.IsPostBack Then" & vbCrLf
                'Target += "Page.Validate()" & vbCrLf
                'Target += "If Page.IsValid Then" & vbCrLf
                'Target += "Dim NewGeneric As New TAGC.Classes." & ClassNameHere & "" & vbCrLf
                'Target += "With NewGeneric" & vbCrLf

                'Dim PrimaryKeyName As String
                'For counter = 0 To (rept_column_count - 1)
                '    'get primary key column name here
                '    PrimaryKeyName = DataReader.GetName(0)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        Target += "." & DataReader.GetName(counter) & " = Request.Form(""drp" & DataReader.GetName(counter) & """)" & vbCrLf
                '    Else
                '        Target += "." & DataReader.GetName(counter) & " = Request.Form(""txt" & DataReader.GetName(counter) & """)" & vbCrLf
                '    End If

                'Next
                'Target += ".save()" & vbCrLf
                'Target += "End With" & vbCrLf
                'Target += "Dim NewGenericID As String" & vbCrLf
                'Target += "NewGenericID = NewGeneric." & PrimaryKeyName & "" & vbCrLf
                'Target += "End If" & vbCrLf
                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf
                'Target += "Else" & vbCrLf
                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

                'For counter = 0 To (rept_column_count - 1)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        If DataReader.GetName(counter).IndexOf("State") >= 0 Then
                '            Target += "Dim oFuncs As New TAGC.Funcs" & vbCrLf & vbCrLf
                '            Target += "oFuncs.LoadDDLB(" & DataReader.GetName(counter) & ", ""Select * From State_Typ"", 0, ""STypeID"", ""SDesc"")" & vbCrLf
                '        End If
                '    End If
                'Next

                'Target += vbCrLf & vbCrLf

                'For counter = 0 To (rept_column_count - 1)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        If DataReader.GetName(counter).IndexOf("County") >= 0 Then
                '            Target += "Dim oFuncs As New TAGC.Funcs" & vbCrLf & vbCrLf
                '            Target += "oFuncs.LoadDDLB(" & DataReader.GetName(counter) & ", ""Select * From County_Typ"", 0, ""CountyID"", ""CountyDesc"")" & vbCrLf
                '        End If
                '    End If
                'Next

                'Target += vbCrLf & vbCrLf

                'For counter = 0 To (rept_column_count - 1)
                '    'HttpContext.Current.Response.Write("adam = " & DataReader.GetDataTypeName(counter).ToString & "" & vbCrLf)
                '    If DataReader.GetDataTypeName(counter).ToString = "int" Then
                '        If DataReader.GetName(counter).IndexOf("StatusID") >= 0 Then
                '            Target += "Dim oFuncs As New TAGC.Funcs" & vbCrLf & vbCrLf
                '            Target += "oFuncs.LoadDDLB(drp" & DataReader.GetName(counter) & ", ""Select * From Status_Typ"", 0, ""StatusID"", ""StatusType"")" & vbCrLf
                '        End If
                '    End If
                'Next

                'Target += vbCrLf & vbCrLf



                'Target += "End If" & vbCrLf

                'Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

            End If


            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            'If sqlConn.State = Data.ConnectionState.Open Then
            '    sqlConn.Close()
            'End If
        End Try

    End Function


    Public Function MakeIUStoredProcedure(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0

            '*********************************************************************************************************************************
            '*********************************************************************************************************************************

            Target += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & StoredProcName & "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" & vbCrLf
            Target += "drop procedure [dbo].[" & StoredProcName & "]" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf

            Target += "CREATE PROCEDURE [dbo].[" & StoredProcName & "]" & vbCrLf


            For counter = 0 To (rept_column_count - 1)

                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "decimal"
                        Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("NumericPrecision") & "," & DataReader.GetSchemaTable.Rows(counter).Item("NumericScale") & ")" & " = NULL, " & vbCrLf
                    Case "varchar", "nvarchar", "nchar", "char", "ntext", "text"
                        Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                    Case Else
                        Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL, " & vbCrLf
                End Select



            Next

            Target += "@RetVal int OUTPUT " & vbCrLf
            Target += "AS" & vbCrLf
            Target += "SET NOCOUNT ON" & vbCrLf

            For counter = 0 To (rept_column_count - 1)
                If DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                    Target += "IF EXISTS(SELECT * FROM [dbo].[" & TableName & "] WHERE " & DataReader.GetName(counter) & " = @" & DataReader.GetName(counter) & ") "
                End If
            Next

            Target += vbCrLf & " BEGIN " & vbCrLf
            Target += "UPDATE  [dbo].[" & TableName & "]" & vbCrLf
            '
            Target += "Set "
            For counter = 0 To (rept_column_count - 1)
                If Not DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                    If counter = rept_column_count - 1 Then
                        Target += DataReader.GetName(counter) & " = isNull(@" & DataReader.GetName(counter) & ", " & DataReader.GetName(counter) & ")"
                    Else
                        Target += DataReader.GetName(counter) & " = isNull(@" & DataReader.GetName(counter) & ", " & DataReader.GetName(counter) & "),"
                    End If
                End If
            Next
            Target += "" & vbCrLf

            For counter = 0 To (rept_column_count - 1)
                If DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                    Target += "WHERE " & DataReader.GetName(counter) & " = @" & DataReader.GetName(counter) & " " & vbCrLf
                End If
            Next
            Target += "Return (0)" & vbCrLf
            Target += "End" & vbCrLf
            Target += "ELSE " & vbCrLf & " BEGIN " & vbCrLf
            Target += "INSERT INTO  [dbo].[" & TableName & "]" & vbCrLf
            Target += "("
            For counter = 0 To (rept_column_count - 1)
                If Not DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                    If counter = rept_column_count - 1 Then
                        Target += DataReader.GetName(counter)
                    Else
                        Target += DataReader.GetName(counter) & ", "
                    End If
                End If
            Next
            Target += ")" & vbCrLf & "VALUES ("
            For counter = 0 To (rept_column_count - 1)
                If Not DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                    If counter = rept_column_count - 1 Then
                        If DataReader.GetDataTypeName(counter).ToString = "datetime" Then
                            Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL)"
                        Else
                            If DataReader.GetName(counter) = "SecondaryNavID" Then
                                Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL)"
                            Else
                                'Target += "IsNull(@" & DataReader.GetName(counter) & ", ''),"
                                Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL)"
                            End If
                        End If

                    Else
                        If DataReader.GetDataTypeName(counter).ToString = "datetime" Then
                            Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL), "
                        Else
                            If DataReader.GetName(counter) = "SecondaryNavID" Then
                                Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL),"
                            Else
                                'Target += "IsNull(@" & DataReader.GetName(counter) & ", ''),"
                                Target += "IsNull(@" & DataReader.GetName(counter) & ", NULL),"
                            End If
                        End If

                    End If

                End If
            Next
            Target += ")" & vbCrLf & "SET @RetVal = @@IDENTITY" & vbCrLf & "End" & vbCrLf & "GO"

            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf



            '*********************************************************************************************************************************
            '*********************************************************************************************************************************




            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

    End Function

    Public Function MakeLoadGETStoredProceudre(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0


            '*********************************************************************************************************************************
            '*********************************************************************************************************************************




            Target += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & LoadStoredProcName & "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" & vbCrLf
            Target += "drop procedure [dbo].[" & LoadStoredProcName & "]" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf


            Target += "CREATE PROCEDURE  [dbo].[" & LoadStoredProcName & "]" & vbCrLf

            Dim GetCounter As Integer = 0
            Dim TempCounter As Integer = 0

            For counter = 0 To (rept_column_count - 1)
                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar", "int", "nvarchar", "uniqueidentifier", "nchar", "char", "text", "ntext"
                        GetCounter = GetCounter + 1
                End Select
            Next

            'Target += GetCounter & "" & vbCrLf

            For counter = 0 To (rept_column_count - 1)

                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If
                    Case "nchar"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If
                    Case "char"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If
                    Case "nvarchar"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If
                    Case "ntext"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If
                    Case "text"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If

                    Case "int"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If

                    Case "uniqueidentifier"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If


                    Case "decimal"
                        If TempCounter = GetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("NumericPrecision") & "," & DataReader.GetSchemaTable.Rows(counter).Item("NumericScale") & ")" & " = NULL, " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("NumericPrecision") & "," & DataReader.GetSchemaTable.Rows(counter).Item("NumericScale") & ")" & " = NULL, " & vbCrLf
                            TempCounter = TempCounter + 1
                        End If

                End Select

            Next

            'Target += TempCounter & "" & vbCrLf

            Target += "AS" & vbCrLf
            Target += "Declare @WhereClause as varchar(8000)" & vbCrLf
            Target += "SELECT  @WhereClause =  'SELECT * FROM [dbo].[" & TableName & "] WHERE 1 = 1 '" & "" & vbCrLf
            For counter = 0 To (rept_column_count - 1)

                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar", "int", "nvarchar", "nchar", "char", "ntext", "text"
                        If DataReader.GetName(counter) = "SecondaryNavID" Then
                            Target += "If DataLength(@SecondaryNavID) > 0 Select @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                            Target += "if @SecondaryNavID = 0" & "" & vbCrLf
                            Target += "SELECT @WhereClause = case @SecondaryNavID when isnull(@SecondaryNavID,'') then @WhereClause + ' SecondaryNavID IS NULL' else @WhereClause end" & "" & vbCrLf
                            Target += "else" & "" & vbCrLf
                            Target += "SELECT @WhereClause = case @SecondaryNavID when isnull(@SecondaryNavID,'') then @WhereClause + ' SecondaryNavID = ' + CONVERT(varchar,@SecondaryNavID) else @WhereClause end" & vbCrLf
                        Else
                            Target += "if DataLength(@" & DataReader.GetName(counter) & ") > 0 SELECT @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                            If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Or DataReader.GetDataTypeName(counter).ToString = "text" Then
                                Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " LIKE ''%' + CONVERT(varchar(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & "),@" & DataReader.GetName(counter) & ") + '%''' else @WhereClause end" & vbCrLf
                            Else
                                Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " = ' + CONVERT(varchar,@" & DataReader.GetName(counter) & ") else @WhereClause end" & vbCrLf
                            End If

                        End If
                    Case "uniqueidentifier"
                        Target += "if DataLength(@" & DataReader.GetName(counter) & ") > 0 SELECT @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                        Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " = ''' + CONVERT(varchar(40),@" & DataReader.GetName(counter) & ") + '''' else @WhereClause end" & vbCrLf
                End Select


            Next


            'Target += "exec('SELECT (' + @WhereClause + ' FOR JSON AUTO, Without_Array_Wrapper) AS JSONData')" & "" & vbCrLf
            If chk_api.Checked Then
                Target += "exec('SELECT (' + @WhereClause + ' FOR JSON AUTO, Without_Array_Wrapper) AS JSONData')" & "" & vbCrLf
            Else
                Target += "exec(@WhereClause)" & "" & vbCrLf
            End If
            Target += "GO"

            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf


            '*********************************************************************************************************************************
            '*********************************************************************************************************************************



            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Function


    Public Function MakeCountStoredProceudre(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0

            '*********************************************************************************************************************************
            '*********************************************************************************************************************************


            Target += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & CountStoredProcName & "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" & vbCrLf
            Target += "drop procedure [dbo].[" & CountStoredProcName & "]" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf

            Target += "CREATE PROCEDURE  " & CountStoredProcName & "" & vbCrLf

            Dim xGetCounter As Integer = 0
            Dim xTempCounter As Integer = 0

            For counter = 0 To (rept_column_count - 1)
                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar", "int", "nvarchar", "uniqueidentifier", "nchar", "text", "ntext"
                        xGetCounter = xGetCounter + 1
                End Select
            Next

            'Target += GetCounter & "" & vbCrLf

            For counter = 0 To (rept_column_count - 1)

                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "nchar"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "char"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "nvarchar"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "text"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "ntext"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If
                    Case "int"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If

                    Case "uniqueidentifier"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If


                    Case "decimal"
                        If xTempCounter = xGetCounter - 1 Then
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("NumericPrecision") & "," & DataReader.GetSchemaTable.Rows(counter).Item("NumericScale") & ")" & " = NULL, " & vbCrLf
                        Else
                            Target += "@" & DataReader.GetName(counter) & " AS " & DataReader.GetDataTypeName(counter).ToString & "(" & DataReader.GetSchemaTable.Rows(counter).Item("NumericPrecision") & "," & DataReader.GetSchemaTable.Rows(counter).Item("NumericScale") & ")" & " = NULL, " & vbCrLf
                            xTempCounter = xTempCounter + 1
                        End If

                End Select

            Next

            'Target += TempCounter & "" & vbCrLf

            Target += "AS" & vbCrLf
            Target += "Declare @WhereClause as varchar(8000)" & vbCrLf
            Target += "SELECT  @WhereClause =  'SELECT Count(*) As ReturnCount FROM [dbo].[" & TableName & "] WHERE 1 = 1 '" & "" & vbCrLf
            For counter = 0 To (rept_column_count - 1)

                Select Case DataReader.GetDataTypeName(counter).ToString
                    Case "varchar", "int", "nvarchar", "nchar", "char", "ntext", "text"
                        If DataReader.GetName(counter) = "SecondaryNavID" Then
                            Target += "if DataLength(@SecondaryNavID) > 0 SELECT @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                            Target += "if @SecondaryNavID = 0" & "" & vbCrLf
                            Target += "SELECT @WhereClause = case @SecondaryNavID when isnull(@SecondaryNavID,'') then @WhereClause + ' SecondaryNavID IS NULL' else @WhereClause end" & "" & vbCrLf
                            Target += "else" & "" & vbCrLf
                            Target += "SELECT @WhereClause = case @SecondaryNavID when isnull(@SecondaryNavID,'') then @WhereClause + ' SecondaryNavID = ' + CONVERT(varchar,@SecondaryNavID) else @WhereClause end" & vbCrLf
                        Else
                            Target += "if DataLength(@" & DataReader.GetName(counter) & ") > 0 SELECT @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                            If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "text" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Then
                                Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " LIKE ''%' + CONVERT(varchar(" & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & "),@" & DataReader.GetName(counter) & ") + '%''' else @WhereClause end" & vbCrLf
                            Else
                                Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " = ' + CONVERT(varchar,@" & DataReader.GetName(counter) & ") else @WhereClause end" & vbCrLf
                            End If

                        End If
                    Case "uniqueidentifier"
                        Target += "if DataLength(@" & DataReader.GetName(counter) & ") > 0 SELECT @WhereClause = @WhereClause + ' AND '" & "" & vbCrLf
                        Target += "SELECT  @WhereClause = case @" & DataReader.GetName(counter) & " when isnull(@" & DataReader.GetName(counter) & ",'') then  @WhereClause + ' " & DataReader.GetName(counter) & " = ''' + CONVERT(varchar(40),@" & DataReader.GetName(counter) & ") + '''' else @WhereClause end" & vbCrLf
                End Select


            Next

            If chk_api.Checked Then
                Target += "exec('SELECT (' + @WhereClause + ' FOR JSON AUTO, Without_Array_Wrapper) AS JSONData')" & "" & vbCrLf
            Else
                Target += "exec(@WhereClause)" & "" & vbCrLf
            End If
            Target += "GO"

            Target += vbCrLf & vbCrLf
            Target += "SET QUOTED_IDENTIFIER ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += "SET ANSI_NULLS ON" & vbCrLf
            Target += "GO" & vbCrLf
            Target += vbCrLf & vbCrLf

            'Target += "CREATE PROCEDURE  " & LoadStoredProcName & "" & vbCrLf
            'For counter = 0 To (rept_column_count - 1)
            '	If counter = 0 Then
            '		Target += "@" & DataReader.GetName(counter) & " " & DataReader.GetDataTypeName(counter).ToString & "" & vbCrLf
            '	End If
            'Next

            'Target += "AS" & vbCrLf
            'Target += "Select * from " & TableName & "" & vbCrLf
            'For counter = 0 To (rept_column_count - 1)
            '	If counter = 0 Then
            '		Target += "WHERE " & DataReader.GetName(counter) & " = @" & DataReader.GetName(counter) & " " & vbCrLf
            '	End If
            'Next

            'Target += "GO"

            '*********************************************************************************************************************************
            '*********************************************************************************************************************************

            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Function

    Public Function MakeCountBLClass(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0

            If Not chk_csharp.Checked Then

                Target += "Public NotInheritable Class " & Count2Use & " " & vbCrLf

                Target += "Public RetVal As String" & vbCrLf
                For counter = 0 To (rept_column_count - 1)
                    Target += "Public " & DataReader.GetName(counter) & " As String " & vbCrLf
                Next

                Target += "Public Function ExecuteProc() " & vbCrLf
                Target += "	Dim sqlConn As New SqlConnection(" & TempAppConnectionString & ") " & vbCrLf
                Target += "	Dim sqlCmd As New SqlClient.SqlCommand(""[dbo].[" & CountStoredProcName & "]"", sqlConn) " & vbCrLf
                Target += "	With sqlCmd " & vbCrLf
                Target += "		.CommandType = CommandType.StoredProcedure " & vbCrLf
                For counter = 0 To (rept_column_count - 1)
                    'HttpContext.Current.Response.Write("adam = " & DataReader.GetSchemaTable.Rows(counter).Item(5) & "" & vbCrLf)
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar", "nvarchar", "nchar", "char", "text", "ntext"
                            Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                        Case "int"
                            Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                    End Select
                Next

                Target += "End With " & vbCrLf
                Target += "Dim DataReader As SqlDataReader " & vbCrLf
                Target += "Try " & vbCrLf
                Target += "	sqlCmd.Connection.Open() " & vbCrLf
                Target += "	DataReader = sqlCmd.ExecuteReader() " & vbCrLf
                Target += "Do While DataReader.Read()" & vbCrLf


                Target += "If Not IsDBNull(DataReader.Item(""ReturnCount"")) Then" & vbCrLf
                Target += "RetVal = DataReader.Item(""ReturnCount"")" & vbCrLf
                Target += "Else" & vbCrLf
                Target += "RetVal = 0" & vbCrLf
                Target += "End If" & vbCrLf


                Target += "Loop" & vbCrLf
                Target += "Catch ex As System.Exception " & vbCrLf
                Target += "	Throw New System.Exception(ex.ToString()) " & vbCrLf
                Target += "Finally " & vbCrLf
                Target += "	If sqlConn.State = Data.ConnectionState.Open Then " & vbCrLf
                Target += "		sqlConn.Close() " & vbCrLf
                Target += "	End If " & vbCrLf
                Target += "End Try " & vbCrLf

                Target += "End Function " & vbCrLf

                Target += "End Class " & vbCrLf


            Else

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                Target += "public sealed class " & Count2Use & " " & vbCrLf
                Target += "    { " & vbCrLf
                Target += "        public string RetVal; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)

                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nvarchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "char"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "text"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "ntext"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "bit"
                            Target += "public bool " & DataReader.GetName(counter) & "; " & vbCrLf
                        Case "int"
                            Target += "public int? " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "datetime"
                            Target += "public DateTime " & DataReader.GetName(counter) & "; " & vbCrLf
                        Case Else
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                    End Select

                Next

                Target += " " & vbCrLf
                Target += "        public void ExecuteProc() " & vbCrLf
                Target += "        { " & vbCrLf
                Target += "            SqlConnection sqlConn = new SqlConnection(" & TempCSharpAppConnectionString & "); " & vbCrLf
                Target += "            System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(""[dbo].[" & CountStoredProcName & "]"", sqlConn); " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                var withBlock = sqlCmd; " & vbCrLf
                Target += "                withBlock.CommandType = CommandType.StoredProcedure; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)

                    Dim CounterType As String = ""
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            CounterType = "VarChar"
                        Case "nvarchar"
                            CounterType = "NVarChar"
                        Case "date"
                            CounterType = "Date"
                        Case "ntext"
                            CounterType = "NText"
                        Case "char"
                            CounterType = "Char"
                        Case "nchar"
                            CounterType = "NChar"
                        Case "text"
                            CounterType = "Text"
                        Case "money"
                            CounterType = "Money"
                        Case "bit"
                            CounterType = "Bit"
                        Case "int"
                            CounterType = "Int"
                        Case "datetime"
                            CounterType = "DateTime"
                        Case "uniqueidentifier"
                            CounterType = "UniqueIdentifier"
                        Case Else
                            CounterType = DataReader.GetDataTypeName(counter).ToString
                    End Select

                    'HttpContext.Current.Response.Write("adam = " & DataReader.GetSchemaTable.Rows(counter).Item(5) & "" & vbCrLf) 
                    If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "text" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Then
                        Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                    Else
                        If DataReader.GetDataTypeName(counter).ToString = "uniqueidentifier" Then
                            Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = new Guid(" & DataReader.GetName(counter) & ");" & vbCrLf
                        Else
                            Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                        End If
                    End If
                Next

                Target += "            } " & vbCrLf
                Target += " " & vbCrLf
                Target += "            SqlDataReader DataReader; " & vbCrLf
                Target += "            try " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                sqlCmd.Connection.Open(); " & vbCrLf
                Target += "                DataReader = sqlCmd.ExecuteReader(); " & vbCrLf
                Target += "                while (DataReader.Read()) " & vbCrLf
                Target += "                { " & vbCrLf
                Target += "                    if (DataReader[""ReturnCount""] != System.DBNull.Value) " & vbCrLf
                Target += "                        RetVal = (string)DataReader[""ReturnCount""]; " & vbCrLf
                Target += "                    else " & vbCrLf
                Target += "                        RetVal = ""0""; " & vbCrLf
                Target += "                } " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            catch (Exception ex) " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                throw new System.Exception(ex.ToString()); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            finally " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                if (sqlConn.State == System.Data.ConnectionState.Open) " & vbCrLf
                Target += "                    sqlConn.Close(); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "        } " & vbCrLf
                Target += "    } " & vbCrLf


                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                Target += vbCrLf & vbCrLf & vbCrLf


            End If


            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Function


    Public Function MakeIUDALClass(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0

            If Not chk_csharp.Checked Then

                Target += vbCrLf & vbCrLf & vbCrLf

                Target += "Public NotInheritable Class " & IU2Use & " " & vbCrLf

                Target += "Public RetVal As String" & vbCrLf
                For counter = 0 To (rept_column_count - 1)

                    Dim TempType As String = ""
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "bit"
                            TempType = "Boolean"
                        Case Else
                            TempType = "String"
                    End Select

                    Target += "Public " & DataReader.GetName(counter) & " As " & TempType & " " & vbCrLf
                Next

                Target += "Public Function ExecuteProc() " & vbCrLf
                Target += "	Dim sqlConn As New SqlConnection(" & TempAppConnectionString & ") " & vbCrLf
                Target += "	Dim sqlCmd As New SqlClient.SqlCommand(""[dbo].[" & StoredProcName & "]"", sqlConn) " & vbCrLf
                Target += "	Dim output_value As SqlParameter " & vbCrLf
                Target += "	With sqlCmd " & vbCrLf
                Target += "		.CommandType = CommandType.StoredProcedure " & vbCrLf
                Target += "		output_value = .Parameters.Add(New SqlClient.SqlParameter(""@RetVal"", SqlDbType.Int)) " & vbCrLf
                Target += "		output_value.Direction = ParameterDirection.Output " & vbCrLf

                For counter = 0 To (rept_column_count - 1)
                    'HttpContext.Current.Response.Write("adam = " & DataReader.GetSchemaTable.Rows(counter).Item(5) & "" & vbCrLf)
                    If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "text" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Then
                        Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                    Else
                        Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                    End If
                Next

                Target += "End With " & vbCrLf
                Target += "Try " & vbCrLf
                Target += "	sqlCmd.Connection.Open() " & vbCrLf
                Target += "	sqlCmd.ExecuteReader() " & vbCrLf
                Target += "Catch ex As System.Exception " & vbCrLf
                Target += "	Throw New System.Exception(ex.ToString()) " & vbCrLf
                Target += "Finally " & vbCrLf
                Target += "	If IsDBNull(output_value.Value) Then " & vbCrLf

                For counter = 0 To (rept_column_count - 1)
                    If DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                        Target += "		RetVal = " & DataReader.GetName(counter) & " " & vbCrLf
                    End If
                Next

                Target += "	Else " & vbCrLf
                Target += "		RetVal = output_value.Value " & vbCrLf
                Target += "	End If " & vbCrLf
                Target += "	If sqlConn.State = Data.ConnectionState.Open Then " & vbCrLf
                Target += "		sqlConn.Close() " & vbCrLf
                Target += "	End If " & vbCrLf
                Target += "End Try " & vbCrLf

                Target += "End Function " & vbCrLf


                Target += "End Class" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

            Else

                Target += "public sealed class " & IU2Use & " " & vbCrLf
                Target += "    { " & vbCrLf
                Target += "        public string RetVal; " & vbCrLf
                For counter = 0 To (rept_column_count - 1)

                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nvarchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "char"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "text"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "ntext"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "bit"
                            Target += "public bool " & DataReader.GetName(counter) & "; " & vbCrLf
                        Case "int"
                            Target += "public int? " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "datetime"
                            Target += "public DateTime " & DataReader.GetName(counter) & " = Convert.ToDateTime(""1/1/1974 00:00:00.000""); " & vbCrLf
                        Case Else
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                    End Select

                Next
                Target += " " & vbCrLf
                Target += "        public void ExecuteProc() " & vbCrLf
                Target += "        { " & vbCrLf
                Target += "            SqlConnection sqlConn = new SqlConnection(" & TempCSharpAppConnectionString & "); " & vbCrLf
                Target += "            System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(""[dbo].[" & StoredProcName & "]"", sqlConn); " & vbCrLf
                Target += "            SqlParameter output_value; " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                var withBlock = sqlCmd; " & vbCrLf
                Target += "                withBlock.CommandType = CommandType.StoredProcedure; " & vbCrLf
                Target += "                output_value = withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@RetVal"", SqlDbType.Int)); " & vbCrLf
                Target += "                output_value.Direction = ParameterDirection.Output; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)

                    Dim CounterType As String = ""
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            CounterType = "VarChar"
                        Case "nvarchar"
                            CounterType = "NVarChar"
                        Case "date"
                            CounterType = "Date"
                        Case "ntext"
                            CounterType = "NText"
                        Case "char"
                            CounterType = "Char"
                        Case "nchar"
                            CounterType = "NChar"
                        Case "text"
                            CounterType = "Text"
                        Case "money"
                            CounterType = "Money"
                        Case "bit"
                            CounterType = "Bit"
                        Case "int"
                            CounterType = "Int"
                        Case "datetime"
                            CounterType = "DateTime"
                        Case "uniqueidentifier"
                            CounterType = "UniqueIdentifier"
                        Case Else
                            CounterType = DataReader.GetDataTypeName(counter).ToString
                    End Select

                    'HttpContext.Current.Response.Write("adam = " & DataReader.GetSchemaTable.Rows(counter).Item(5) & "" & vbCrLf) 
                    If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "text" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Then
                        Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                    Else
                        If DataReader.GetDataTypeName(counter).ToString = "uniqueidentifier" Then
                            Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = new Guid(" & DataReader.GetName(counter) & ");" & vbCrLf
                        Else
                            Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                        End If
                    End If
                Next

                Target += "            } " & vbCrLf
                Target += "            try " & vbCrLf
                Target += "            {" & vbCrLf
                Target += "                sqlCmd.Connection.Open(); " & vbCrLf
                Target += "                sqlCmd.ExecuteReader(); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            catch (Exception ex) " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                throw new System.Exception(ex.ToString()); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            finally " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                if (output_value.Value == System.DBNull.Value) " & vbCrLf

                For counter = 0 To (rept_column_count - 1)
                    If DataReader.GetSchemaTable.Rows(counter).Item("IsIdentity") Then
                        Target += "		RetVal = Convert.ToString(" & DataReader.GetName(counter) & "); " & vbCrLf
                    End If
                Next

                Target += "                else " & vbCrLf
                Target += "                    RetVal = output_value.Value.ToString(); " & vbCrLf
                Target += "                if (sqlConn.State == System.Data.ConnectionState.Open) " & vbCrLf
                Target += "                    sqlConn.Close(); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "        } " & vbCrLf
                Target += "    } " & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                Target += vbCrLf & vbCrLf & vbCrLf

            End If


            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Function


    Public Function MakeBLClass(ByVal DBName As String, ByVal TableName As String, ByVal ClassNameHere As String, ByVal StoredProcName As String, ByVal IU2Use As String, ByVal LoadStoredProcName As String, ByVal Switch As String, ByVal Count2Use As String, ByVal CountStoredProcName As String, ByVal TempAppConnectionString As String, ByVal TempCSharpAppConnectionString As String) As String
        Dim strSQL As String = ""
        Dim sqlCmd As New SqlCommand
        Dim sqlConn As New SqlConnection
        Dim DataReader As SqlDataReader
        Dim rept_column_count As Integer
        Dim numloops As Integer
        Dim counter As Integer
        Dim Target As String = ""


        strSQL += "Select * From " & TableName


        sqlConn.ConnectionString = DBConStr
        sqlCmd = New SqlClient.SqlCommand(strSQL, sqlConn)

        Try
            sqlCmd.Connection.Open()
            DataReader = sqlCmd.ExecuteReader

            rept_column_count = DataReader.FieldCount
            counter = 0

            If Not chk_csharp.Checked Then


                Target += "Public Class " & ClassNameHere & "" & vbCrLf


                For counter = 0 To (rept_column_count - 1)
                    Target += "Public " & DataReader.GetName(counter) & " As String " & vbCrLf
                Next

                Target += "Public JSONData As String " & vbCrLf

                Target += vbCrLf & vbCrLf & vbCrLf

                Target += "Public Function xCount() As Integer" & vbCrLf
                Target += "Dim output As Integer" & vbCrLf
                Target += "Dim generic As New Count_Database." & Count2Use & "" & vbCrLf
                Target += "With generic" & vbCrLf
                For counter = 0 To (rept_column_count - 1)
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar", "int", "nvarchar", "nchar", "char", "text", "ntext"
                            Target += "." & DataReader.GetName(counter) & " = " & DataReader.GetName(counter) & "" & vbCrLf
                    End Select
                Next
                Target += "End With" & vbCrLf

                Target += "generic.ExecuteProc()" & vbCrLf
                Target += "output = generic.RetVal" & vbCrLf
                Target += "Return output" & vbCrLf
                Target += "generic = Nothing" & vbCrLf
                Target += "End Function" & vbCrLf & vbCrLf & vbCrLf


                Target += "<BR>" & vbCrLf

                Target += "Public Function save() As Integer" & vbCrLf
                Target += "Dim output As Integer" & vbCrLf
                Target += "Dim generic As New IU_Database." & IU2Use & "" & vbCrLf
                Target += "With generic" & vbCrLf
                For counter = 0 To (rept_column_count - 1)
                    Target += "." & DataReader.GetName(counter) & " = " & DataReader.GetName(counter) & "" & vbCrLf
                Next
                Target += "End With" & vbCrLf

                Target += "generic.ExecuteProc()" & vbCrLf
                Target += "output = generic.RetVal" & vbCrLf
                Target += "Return output" & vbCrLf
                Target += "generic = Nothing" & vbCrLf
                Target += "End Function" & vbCrLf & vbCrLf & vbCrLf


                Target += "Public Function load()" & vbCrLf

                Target += "Dim sqlConn As New SqlConnection(" & TempAppConnectionString & ")" & vbCrLf
                Target += "Dim sqlCmd As New SqlClient.SqlCommand(""[dbo].[" & LoadStoredProcName & "]"", sqlConn)" & vbCrLf
                Target += "With sqlCmd" & vbCrLf
                Target += ".CommandType = CommandType.StoredProcedure" & vbCrLf


                For counter = 0 To (rept_column_count - 1)

                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar", "nvarchar", "nchar", "char", "text", "ntext"
                            Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                        Case "int"
                            Target += ".Parameters.Add(New SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """,SqlDbType." & DataReader.GetDataTypeName(counter).ToString & ")).Value = " & DataReader.GetName(counter) & "" & vbCrLf
                    End Select


                Next

                Target += "End With" & vbCrLf

                Target += "Dim DataReader As SqlDataReader" & vbCrLf
                Target += "Try" & vbCrLf
                Target += "sqlCmd.Connection.Open()" & vbCrLf
                Target += "DataReader = sqlCmd.ExecuteReader()" & vbCrLf
                Target += "Do While DataReader.Read()" & vbCrLf

                If Not chk_api.Checked Then
                    For counter = 0 To (rept_column_count - 1)
                        Target += "If Not DataReader.Item(""" & DataReader.GetName(counter) & """) Is Nothing Then" & vbCrLf
                        Target += "     If Not IsDBNull(DataReader.Item(""" & DataReader.GetName(counter) & """)) Then" & vbCrLf
                        Target += DataReader.GetName(counter) & " = DataReader.Item(""" & DataReader.GetName(counter) & """)" & vbCrLf
                        Target += "     End If" & vbCrLf
                        Target += "End If" & vbCrLf
                    Next
                Else
                    Target += "If Not DataReader.Item(""JSONData"") Is Nothing Then" & vbCrLf
                    Target += "     If Not IsDBNull(DataReader.Item(""JSONData"")) Then" & vbCrLf
                    Target += "         JSONData = DataReader.Item(""JSONData"")" & vbCrLf
                    Target += "     End If" & vbCrLf
                    Target += "End If" & vbCrLf
                End If



                Target += "Loop" & vbCrLf
                Target += "Catch ex As System.Exception" & vbCrLf
                Target += "Throw New System.Exception(ex.ToString())" & vbCrLf
                Target += "Finally" & vbCrLf
                Target += "If sqlConn.State = Data.ConnectionState.Open Then" & vbCrLf
                Target += "sqlConn.Close()" & vbCrLf
                Target += "End If" & vbCrLf
                Target += "End Try" & vbCrLf

                Target += "End Function" & vbCrLf & vbCrLf & vbCrLf

                Target += "End Class " & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************





            Else

                '*********************************************************************************************************************************
                '*********************************************************************************************************************************

                Target += " public class " & ClassNameHere & " " & vbCrLf
                Target += "    { " & vbCrLf

                For counter = 0 To (rept_column_count - 1)

                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nvarchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "nchar"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "char"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "text"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "ntext"
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "bit"
                            Target += "public bool " & DataReader.GetName(counter) & "; " & vbCrLf
                        Case "int"
                            Target += "public int? " & DataReader.GetName(counter) & " = null; " & vbCrLf
                        Case "datetime"
                            Target += "public DateTime " & DataReader.GetName(counter) & "; " & vbCrLf
                        Case Else
                            Target += "public string " & DataReader.GetName(counter) & " = null; " & vbCrLf
                    End Select

                Next

                Target += "        public string JSONData; " & vbCrLf
                Target += " " & vbCrLf
                Target += " " & vbCrLf
                Target += "        public string XCount() " & vbCrLf
                Target += "        { " & vbCrLf
                Target += "            string output; " & vbCrLf
                Target += "            Count.Count_Database." & Count2Use & " generic = new Count.Count_Database." & Count2Use & "(); " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                var withBlock = generic; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)
                    Target += "withBlock." & DataReader.GetName(counter) & " = " & DataReader.GetName(counter) & "; " & vbCrLf
                Next

                Target += "            } " & vbCrLf
                Target += " " & vbCrLf
                Target += "            generic.ExecuteProc(); " & vbCrLf
                Target += "            string retVal = generic.RetVal; " & vbCrLf
                Target += "            output = retVal; " & vbCrLf
                Target += "            return output; " & vbCrLf
                Target += "        } " & vbCrLf
                Target += " " & vbCrLf
                Target += " " & vbCrLf
                Target += " " & vbCrLf
                Target += "        public string Save() " & vbCrLf
                Target += "        { " & vbCrLf
                Target += "            string output; " & vbCrLf
                Target += "            IU.IU_Database." & IU2Use & " generic = new IU.IU_Database." & IU2Use & "(); " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                var withBlock = generic; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)
                    Target += "withBlock." & DataReader.GetName(counter) & " = " & DataReader.GetName(counter) & "; " & vbCrLf
                Next

                Target += "            } " & vbCrLf
                Target += " " & vbCrLf
                Target += "            generic.ExecuteProc(); " & vbCrLf
                Target += "            string retVal = generic.RetVal; " & vbCrLf
                Target += "            output = retVal; " & vbCrLf
                Target += "            return output; " & vbCrLf
                Target += " " & vbCrLf
                Target += "        } " & vbCrLf
                Target += " " & vbCrLf
                Target += " " & vbCrLf
                Target += "        public void Load() " & vbCrLf
                Target += "        { " & vbCrLf
                Target += "            SqlConnection sqlConn = new SqlConnection(" & TempCSharpAppConnectionString & "); " & vbCrLf
                Target += "            System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(""[dbo].[" & LoadStoredProcName & "]"", sqlConn); " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                var withBlock = sqlCmd; " & vbCrLf
                Target += "                withBlock.CommandType = CommandType.StoredProcedure; " & vbCrLf

                For counter = 0 To (rept_column_count - 1)

                    Dim CounterType As String = ""
                    Select Case DataReader.GetDataTypeName(counter).ToString
                        Case "varchar"
                            CounterType = "VarChar"
                        Case "nvarchar"
                            CounterType = "NVarChar"
                        Case "date"
                            CounterType = "Date"
                        Case "ntext"
                            CounterType = "NText"
                        Case "char"
                            CounterType = "Char"
                        Case "nchar"
                            CounterType = "NChar"
                        Case "text"
                            CounterType = "Text"
                        Case "money"
                            CounterType = "Money"
                        Case "bit"
                            CounterType = "Bit"
                        Case "int"
                            CounterType = "Int"
                        Case "datetime"
                            CounterType = "DateTime"
                        Case "uniqueidentifier"
                            CounterType = "UniqueIdentifier"
                        Case Else
                            CounterType = DataReader.GetDataTypeName(counter).ToString
                    End Select


                    'HttpContext.Current.Response.Write("adam = " & DataReader.GetSchemaTable.Rows(counter).Item(5) & "" & vbCrLf) 
                    If DataReader.GetDataTypeName(counter).ToString = "varchar" Or DataReader.GetDataTypeName(counter).ToString = "nvarchar" Or DataReader.GetDataTypeName(counter).ToString = "nchar" Or DataReader.GetDataTypeName(counter).ToString = "char" Or DataReader.GetDataTypeName(counter).ToString = "text" Or DataReader.GetDataTypeName(counter).ToString = "ntext" Then
                        Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ", " & DataReader.GetSchemaTable.Rows(counter).Item("ColumnSize") & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                    Else
                        If DataReader.GetDataTypeName(counter).ToString = "uniqueidentifier" Then
                            Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = new Guid(" & DataReader.GetName(counter) & ");" & vbCrLf
                        Else
                            If CounterType = "DateTime" Then
                                Target += " //withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                            Else
                                Target += " withBlock.Parameters.Add(new System.Data.SqlClient.SqlParameter(""@" & DataReader.GetName(counter) & """, SqlDbType." & CounterType & ")).Value = " & DataReader.GetName(counter) & ";" & vbCrLf
                            End If


                        End If
                    End If
                Next

                Target += "            } " & vbCrLf
                Target += " " & vbCrLf
                Target += "            SqlDataReader DataReader; " & vbCrLf
                Target += "            try " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                sqlCmd.Connection.Open(); " & vbCrLf
                Target += "                DataReader = sqlCmd.ExecuteReader(); " & vbCrLf
                Target += "                while (DataReader.Read()) " & vbCrLf
                Target += "                { " & vbCrLf




                If Not chk_api.Checked Then
                    For counter = 0 To (rept_column_count - 1)


                        Dim DataReaderType As String = ""
                        Select Case DataReader.GetDataTypeName(counter).ToString
                            Case "varchar"
                                DataReaderType = "string"
                            Case "nvarchar"
                                DataReaderType = "string"
                            Case "nchar"
                                DataReaderType = "string"
                            Case "char"
                                DataReaderType = "string"
                            Case "text"
                                DataReaderType = "string"
                            Case "ntext"
                                DataReaderType = "string"
                            Case "int"
                                DataReaderType = "int"
                            Case "datetime"
                                DataReaderType = "DateTime"
                            Case "uniqueidentifier"
                                DataReaderType = "string"
                            Case Else
                                DataReaderType = "string"
                        End Select


                        Target += "                    if (DataReader[""" & DataReader.GetName(counter) & """] != null) " & vbCrLf
                        Target += "                    { " & vbCrLf
                        Target += "                        if (DataReader[""" & DataReader.GetName(counter) & """] != System.DBNull.Value) " & vbCrLf
                        Target += "                            " & DataReader.GetName(counter) & " = (" & DataReaderType & ")DataReader[""" & DataReader.GetName(counter) & """]; " & vbCrLf
                        Target += "                    } " & vbCrLf

                    Next
                Else
                    Target += "                    if (DataReader[""JSONData""] != null) " & vbCrLf
                    Target += "                    { " & vbCrLf
                    Target += "                        if (DataReader[""JSONData""] != System.DBNull.Value) " & vbCrLf
                    Target += "                            JSONData = (string)DataReader[""JSONData""]; " & vbCrLf
                    Target += "                    } " & vbCrLf
                End If


                Target += " " & vbCrLf

                Target += "                } " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            catch (Exception ex) " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                throw new System.Exception(ex.ToString()); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "            finally " & vbCrLf
                Target += "            { " & vbCrLf
                Target += "                if (sqlConn.State == System.Data.ConnectionState.Open) " & vbCrLf
                Target += "                    sqlConn.Close(); " & vbCrLf
                Target += "            } " & vbCrLf
                Target += "        } " & vbCrLf
                Target += "    } " & vbCrLf


                '*********************************************************************************************************************************
                '*********************************************************************************************************************************


            End If

            Target += vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf
            Target = Target.Replace("2147483647", "8000")


            Return Target

        Catch ex As System.Exception
            Throw New System.Exception(ex.ToString())
        Finally
            If sqlConn.State = Data.ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Function

    Public Function LoadFromAnyDDLB(ByVal vddl As DropDownList, ByVal AppConnectionString As String, ByVal strSQL As String, ByVal sDefault As String, ByVal sValue As String, ByVal sText As String)
        'vddl = DropDownListBox Ojbect
        'sqlCmd = pre-built sql to fill listbox
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

        If sqlConn.State = Data.ConnectionState.Open Then
            sqlConn.Close()
            sqlConn.Dispose()
        End If
        Return vddl

    End Function

End Class