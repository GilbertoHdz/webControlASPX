Imports System.Text
Imports System.IO
Imports System.Configuration
Imports ClosedXML.Excel

Public Class _Default
    Inherits Page

    Public str_dt_json As String = "[]"
    Dim d_consulta As New d_consultas

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        str_dt_json = JsonUtils.ConvertirDataTableJson(d_consulta.GetInsignias())
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        GetExportarExcel(d_consulta.GetInsignias(), "Insignias", "ReporteInsignias")
    End Sub

    Private Sub GetExportarExcel(ByVal dt As DataTable, pagName As String, ByVal sheetName As String)
        'Referencia
        'http:// www.aspsnippets.com/Articles/Write-data-to-Excel-file-xls-and-xlsx-in-ASPNet.aspx
        'Paquetes
        'Install-Package DocumentFormat.OpenXml
        'Install-Package ClosedXML
        'Install-Package ExcelFile.net

        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, pagName)

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" + sheetName + Date.Now.ToString("yyyy-MM-dd") + ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub

End Class