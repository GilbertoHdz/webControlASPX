Imports System.Web
Imports System.Web.Services

Public Class ajax_test
    Inherits ajax_base

    Dim d_test As New test

    Private Function getArrayJsonTest(ByVal astr_termino As String) As String
        If Not String.IsNullOrEmpty(astr_termino) Then
            Dim dt_test As DataTable = d_test.getTest()
            If dt_test.Rows.Count > 0 Then
                Return JsonUtils.ConvertirDataTableJson(dt_test)
            Else
                Return "[{""descripcion"":""No Encontrado""}]"
            End If
        End If
        Return "[]"
    End Function

    Public Overrides Function ValorResultado(ByVal request As System.Web.HttpRequest) As String
        Dim astr_termino As String = request.QueryString("termino")
        Return getArrayJsonTest(astr_termino)
    End Function

End Class