Imports System.Web
Imports System.Web.Services

Public Class ajax_items_por_curso
    Inherits ajax_base

    Dim d_query As New d_consultas

    Private Function getArrayJson(ByVal a_id_curso As String) As String
        If Not String.IsNullOrEmpty(a_id_curso) Then
            Dim dt_test As DataTable = d_query.GetItemsPorCursos(a_id_curso)
            If dt_test.Rows.Count > 0 Then
                Return JsonUtils.ConvertirDataTableJson(dt_test)
            Else
                Return "[{""itemname"":""Sin Items"", ""itemid"":""-1""}]"
            End If
        End If
        Return "[]"
    End Function

    Public Overrides Function ValorResultado(ByVal request As System.Web.HttpRequest) As String
        Dim str_termino As String = request.QueryString("termino")
        Return getArrayJson(str_termino)
    End Function

End Class