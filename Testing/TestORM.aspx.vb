Public Class TestORM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then

            'Create a variable for start time:
            Dim TimerStart As DateTime
            TimerStart = Now

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Test GET by multiple values or by record ID only...
            Dim TestGet As New Classes.tbl_Name
            With TestGet
                .FirstName = "adam"
                .LastName = "kiger"
                .load()
            End With

            Response.Write("ID = " & TestGet.ID & "<br><br>")
            TestGet = Nothing

            Dim TestGet1 As New Classes.tbl_Name
            With TestGet1
                .ID = 1
                .load()
            End With

            Response.Write("FirstName = " & TestGet1.FirstName & "<br>LastName = " & TestGet1.LastName & "<br><br>")
            TestGet1 = Nothing

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'TEST INSERT of new record...
            Dim TestSaveInsert As New Classes.tbl_Name
            With TestSaveInsert
                .FirstName = "mr. minx"
                .LastName = "kiger"
                '.save() 'without retrieving an ID of the new inserted record
            End With

            'retrieving an ID of the new inserted record
            Dim GenericID As String = TestSaveInsert.save()
            Response.Write("NewID is = " & GenericID & "<br><br>")

            TestSaveInsert = Nothing

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Test GET firstname value of record before the update...
            Dim TestGet2 As New Classes.tbl_Name
            With TestGet2
                .ID = 4
                .load()
            End With

            Response.Write("FirstName Before = " & TestGet2.FirstName & "<br>")
            TestGet1 = Nothing

            'Test UPDATE of record
            Dim TestSaveUpdate As New Classes.tbl_Name
            With TestSaveUpdate
                .ID = 4
                .FirstName = "Madison"
                .LastName = "Kiger"
                .save()
            End With

            TestSaveUpdate = Nothing


            'Test GET firstname value of record after the update...
            Dim TestGet3 As New Classes.tbl_Name
            With TestGet3
                .ID = 4
                .load()
            End With

            Response.Write("FirstName After = " & TestGet3.FirstName & "<br><br>")
            TestGet1 = Nothing

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Test COUNT of records
            Dim TestCount As New Classes.tbl_Name
            With TestCount
                .FirstName = "a"
            End With

            'retrieving the count of records from the query above
            Dim xCount As String = TestCount.xCount()
            Response.Write("Total number of records is = " & xCount & "<br><br>")


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim TimeSpent As System.TimeSpan
            TimeSpent = Now.Subtract(TimerStart)
            Response.Write("* " & TimeSpent.TotalMilliseconds.ToString() & " milliseconds spent on these tasks (7 DB Calls)...")


        End If


    End Sub

End Class