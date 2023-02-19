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
Public NotInheritable Class IU_Database

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
    '****************************  AK2 Production Site IU Classes ******************************
    '****************************************************************************************************	


#Region "AK2"

    Public NotInheritable Class IU_tbl_Name

        Public RetVal As String
        Public ID As String
        Public FirstName As String
        Public LastName As String

        Public Function ExecuteProc()
            Dim sqlConn As New SqlConnection(ConfigurationManager.AppSettings("AppConnectionString"))
            Dim sqlCmd As New SqlClient.SqlCommand("[dbo].[prc_IU_tbl_Name]", sqlConn)
            Dim output_value As SqlParameter
            With sqlCmd
                .CommandType = CommandType.StoredProcedure
                output_value = .Parameters.Add(New SqlClient.SqlParameter("@RetVal", SqlDbType.Int))
                output_value.Direction = ParameterDirection.Output
                .Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = ID
                .Parameters.Add(New SqlClient.SqlParameter("@FirstName", SqlDbType.VarChar, 50)).Value = FirstName
                .Parameters.Add(New SqlClient.SqlParameter("@LastName", SqlDbType.VarChar, 50)).Value = LastName
            End With
            Try
                sqlCmd.Connection.Open()
                sqlCmd.ExecuteReader()
            Catch ex As System.Exception
                Throw New System.Exception(ex.ToString())
            Finally
                If IsDBNull(output_value.Value) Then
                    RetVal = ID
                Else
                    RetVal = output_value.Value
                End If
                If sqlConn.State = Data.ConnectionState.Open Then
                    sqlConn.Close()
                End If
            End Try
        End Function

    End Class


#End Region

End Class
