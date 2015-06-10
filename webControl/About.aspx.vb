Imports System.Globalization
Imports System.Web.Services
Imports AjaxControlToolkit
Imports System.IO

Public Class About
    Inherits Page

    Public str_test_json As String = "[]"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Dim test As New test
        test.getTest()
        str_test_json = JsonUtils.ConvertirDataTableJson(test.getTest())
    End Sub

End Class

