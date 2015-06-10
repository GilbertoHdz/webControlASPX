Public Class _Default
    Inherits Page

    Public str_dt_json As String = "[]"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim test As New test
            str_dt_json = JsonUtils.ConvertirDataTableJson(test.getTest())
        End If

    End Sub

End Class