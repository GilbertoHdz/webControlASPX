Imports System.Text
Imports System.IO
Imports System.Configuration
Imports ClosedXML.Excel

Public Class Apertura
    Inherits Page

    Public str_dtdetalle_json As String = "[]"
    Public str_dtgeneral_json As String = "[]"
    Dim d_consulta As New d_consultas

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        str_dtdetalle_json = JsonUtils.ConvertirDataTableJson(d_consulta.GetAperturaDetalle())
        str_dtgeneral_json = JsonUtils.ConvertirDataTableJson(d_consulta.GetAperturaGeneral())
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click

        Select Case hdnTabSeleccionado.Value
            Case "tab1"
                GetExportarExcel(d_consulta.GetAperturaDetalle(), "Apertura_Detalle", "ReporteAperturaDetalle")
            Case "tab2"
                GetExportarExcel(d_consulta.GetAperturaGeneral(), "Apertura_General", "ReporteAperturaGeneral")
            Case Else
                GetExportarExcel(d_consulta.GetAperturaDetalle(), "Apertura_Detalle", "ReporteAperturaDetalle")
        End Select
    End Sub

    Private Sub GetExportarExcel(ByVal dt As DataTable, pagName As String, ByVal sheetName As String)
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