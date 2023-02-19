Imports System.Web.Mail
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Xml
Imports System.Net
Namespace Classes

    '****************************************************************************************************************************'
    '****************************************************************************************************************************'
    '*  PageID or Fix Number:
    '*       
    '*  Site Affected:
    '*       AK2 Global Components - 
    '*  Developer:
    '*       Adam Kiger
    '*  Date:
    '*       
    '*  Description:
    '*       
    '*  Cause of Bug:
    '*       n/a
    '*	Solution or Flow:
    '*		 
    '*		 
    '*		 
    '*	     
    '****************************************************************************************************************************'
    '****************************************************************************************************************************'

    '****************************************************************************************************
    '****************************  AK2 Production Site Business Classes ******************************
    '****************************************************************************************************	


#Region "AK2"

    Public Class tbl_Name

        Public ID As String
        Public FirstName As String
        Public LastName As String
        Public JSONData As String

        Public Function xCount() As Integer
            Dim output As Integer
            Dim generic As New Count_Database.Count_tbl_Name
            With generic
                .ID = ID
                .FirstName = FirstName
                .LastName = LastName
            End With

            generic.ExecuteProc()
            output = generic.RetVal
            Return output

            generic = Nothing
        End Function

        Public Function save() As Integer
            Dim output As Integer
            Dim generic As New IU_Database.IU_tbl_Name
            With generic
                .ID = ID
                .FirstName = FirstName
                .LastName = LastName
            End With

            generic.ExecuteProc()
            output = generic.RetVal
            Return output

            generic = Nothing
        End Function

        Public Function load()
            Dim sqlConn As New SqlConnection(ConfigurationManager.AppSettings("AppConnectionString"))
            Dim sqlCmd As New SqlClient.SqlCommand("[dbo].[prc_Get_tbl_Name]", sqlConn)
            With sqlCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = ID
                .Parameters.Add(New SqlClient.SqlParameter("@FirstName", SqlDbType.VarChar, 50)).Value = FirstName
                .Parameters.Add(New SqlClient.SqlParameter("@LastName", SqlDbType.VarChar, 50)).Value = LastName
            End With
            Dim DataReader As SqlDataReader
            Try
                sqlCmd.Connection.Open()
                DataReader = sqlCmd.ExecuteReader()

                Do While DataReader.Read()
                    If Not DataReader.Item("ID") Is Nothing Then
                        If Not IsDBNull(DataReader.Item("ID")) Then
                            ID = DataReader.Item("ID")
                        End If
                    End If
                    If Not DataReader.Item("FirstName") Is Nothing Then
                        If Not IsDBNull(DataReader.Item("FirstName")) Then
                            FirstName = DataReader.Item("FirstName")
                        End If
                    End If
                    If Not DataReader.Item("LastName") Is Nothing Then
                        If Not IsDBNull(DataReader.Item("LastName")) Then
                            LastName = DataReader.Item("LastName")
                        End If
                    End If
                Loop

            Catch ex As System.Exception
                Throw New System.Exception(ex.ToString())
            Finally
                If sqlConn.State = Data.ConnectionState.Open Then
                    sqlConn.Close()
                End If
            End Try
        End Function

    End Class


#End Region

End Namespace