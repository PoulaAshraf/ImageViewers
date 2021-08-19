Imports System.Data.SqlClient
Module DB_Connection
    Public sqlConn As New SqlClient.SqlConnection


    Public Sub openConnection()
        Dim db, server As String
        If sqlConn.State = 1 Then sqlConn.Close()

        Try
            server = "DESKTOP-QHVSEFV\SQLADV2008R2"
            db = "DB_eFileTask"
            sqlConn.ConnectionString = "server=" & server & ";database=" & db & ";integrated security=true"
            sqlConn.Open()
        Catch ex As Exception

            ' MsgBox(ex.Message, "فشل فى الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            MsgBox(ex.Message, "فشل فى الاتصال")
            sqlConn.Close()
            End

        End Try
    End Sub
End Module
