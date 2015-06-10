Imports MySql.Data.MySqlClient
Imports System.Configuration

Public Class test
    Dim connectionString = ConfigurationManager.ConnectionStrings("cnnDiplomado").ConnectionString
    Dim cn As New MySqlConnection(connectionString)
    Dim sqladapter As New MySqlDataAdapter
    Dim sqlcmd As New MySqlCommand
    Dim dr As MySqlDataReader
    Dim dt As New DataTable

    Public Function getTest()
        Dim query As String = "SELECT CONCAT(firstname,'',lastname)as Alumno, courseid, fullname,FROM_UNIXTIME(timeaccess)" & _
                            " FROM mdl_user_lastaccess mlu" & _
                            " INNER JOIN mdl_course mc ON mc.id = mlu.courseid " & _
                            " INNER JOIN mdl_user mu ON mu.id=mlu.userid" & _
                            " ORDER BY courseid;"
        Try
            cn.Open()
            sqlcmd = New MySqlCommand(query, cn)
            dr = sqlcmd.ExecuteReader()
            dt.Load(dr)

            'Using reader As MySqlDataReader = sqlcmd.ExecuteReader()
            '    'While reader.Read()
            '    '    Console.WriteLine(String.Format("{0}, {1}", reader(0), reader(1)))
            '    'End While
            '    dt.Load(reader)
            'End Using

            cn.Close()
            Return dt
        Catch ex As MySqlException
            cn.Close()
            Console.WriteLine("Error: " & ex.ToString())
            Return False
        End Try

    End Function

End Class
