Imports System.Text
Imports System.IO
Imports System.Configuration
Imports ClosedXML.Excel
Imports AjaxControlToolkit
Imports System.Runtime.Serialization.Json
Imports Newtonsoft.Json


Public Class Ejecutivo
    Inherits Page

    Public str_dtdetalle_json As String = "[]"
    Public str_dtgeneral_json As String = "[]"
    Dim d_consulta As New d_consultas

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        CargarGrupo()

    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        'str_dtdetalle_json = JsonUtils.ConvertirDataTableJson(d_consulta.GetPromedioGrupo())
        'str_dtgeneral_json = JsonUtils.ConvertirDataTableJson(d_consulta.GetPromedioGeneral())
    End Sub

    Private Sub obtenerGrupo()
        Dim dataFiltro = hdnDataFiltro.Value
        For Each ids_ In dataFiltro.Split(",")
            If Not String.IsNullOrEmpty(ids_) Then

                Dim myKey As New Object

                myKey.grupo = Split(ids_, "_")(0)
                myKey.id_curso = Split(ids_, "_")(1)
                myKey.id_item = Split(ids_, "_")(2)
                myKey.valoracion = Split(ids_, "_")(3)

            End If
        Next
    End Sub



    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Dim dt As New DataTable
        dt = JsonConvert.DeserializeObject(Of DataTable)(hdnJsonSgrid.Value)

        Select Case hdnTabSeleccionado.Value
            Case "tab1"
                GetExportarExcel(dt, "Promedio_Grupo", "ReportePromedioGrupo")
            Case "tab2"
                GetExportarExcel(dt, "Promedio_General", "ReportePromedioGeneral")
            Case Else
                GetExportarExcel(dt, "Promedio_Grupo", "ReportePromedioGrupo")
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

    Private Sub CargarGrupo()
        Dim str As String = "SELECT  SUBSTRING(name,1,7) AS grupo, courseid AS id_curso FROM mdl_groups " & _
                          "WHERE CHAR_LENGTH(SUBSTRING(name, 1, 7)) = 7 GROUP BY SUBSTRING(name,1,7) ORDER BY SUBSTRING(name,1,7);"

        Dim dt = d_consulta.GetDataSetDT(str)

        Dim _item As New ListItem("------", -1)
        ddlGrupos.Items.Clear()
        ddlGrupos.Items.Add(_item)

        For Each dtGrupo In dt.Rows
            _item = New ListItem
            _item.Value = dtGrupo("grupo").ToString + "_" + dtGrupo("id_curso").ToString
            _item.Text = dtGrupo("grupo")
            ddlGrupos.Items.Add(_item)
        Next

    End Sub

    Function GetMailItems() As List(Of ItemDDL)
        Dim ItemDDL = New List(Of ItemDDL)
        'ItemDDL.Add(New ItemDDL(dtGrupo("grupo"), dtGrupo("grupo")))
        Return ItemDDL
    End Function

    Public Class ItemDDL

        Public Sub New()

        End Sub

        Private mID As Integer
        Public Property Value() As Integer
            Get
                Return mID
            End Get
            Set(ByVal value As Integer)
                mID = value
            End Set
        End Property

        Private mName As String
        Public Property Text() As String
            Get
                Return mName
            End Get
            Set(ByVal value As String)
                mName = value
            End Set
        End Property

    End Class

End Class
