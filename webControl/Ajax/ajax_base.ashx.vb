Imports System.Web
Imports System.Web.Services

Public MustInherit Class ajax_base
    Implements System.Web.IHttpHandler, IReadOnlySessionState

    Protected sesion As HttpSessionState

    Public Shared MAX_RESULTADOS_AUTOCOMPLETE As Integer = 20

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        sesion = context.Session
        context.Response.Clear()
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = Encoding.UTF8
        context.Response.Expires = 0
        context.Response.ExpiresAbsolute = DateTime.Now
        context.Response.AddHeader("Pragma", "no-cache")
        context.Response.AddHeader("Pragma", "no-store")
        context.Response.AddHeader("cache-control", "no-cache")
        context.Response.CacheControl = "no-cache"
        context.Response.Cache.SetExpires(DateTime.Now)
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        context.Response.Cache.SetNoServerCaching()
        context.Response.Write(ValorResultado(context.Request))
        context.Response.End()
    End Sub

    MustOverride Function ValorResultado(ByVal request As HttpRequest) As String

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    'Protected i_d_mensajes As d_tablas_lis = New d_tablas_lis()
    'Protected Function GetTextoMensaje(ByVal aint_idMensaje As Int32) As String
    '    Dim msj As String = i_d_mensajes.GetMensaje(sesion("Idioma"), aint_idMensaje)
    '    If Not msj Is Nothing Then
    '        Return msj
    '    Else
    '        Return String.Format("Mensaje {0} no encontrado.", aint_idMensaje)
    '    End If
    'End Function
End Class