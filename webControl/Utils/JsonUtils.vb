Imports System.Text
Imports System.IO
Imports System.Runtime.Serialization.Json
Imports Newtonsoft.Json

''' <summary>
''' Clase para operaciones en formato JSON
''' </summary>
''' 
Public Class JsonUtils

    ''' <summary>
    ''' Conversión de objetos pertenecientes a una clase DataContract con propiedades DataMember,
    ''' a un arreglo de objetos formato JSON
    ''' </summary>
    Public Shared Function ConvertirArrayJson(ByVal _list_objetos As IList) As String
        Dim str_array_objetos As New StringBuilder()
        str_array_objetos.Append("[")
        Dim i_num_objetos As Integer = _list_objetos.Count
        Dim i_cont_objetos As Integer = 1
        For Each obj In _list_objetos
            Dim stream As MemoryStream = New MemoryStream
            Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(obj.GetType)
            serializer.WriteObject(stream, obj)
            stream.Position = 0
            Dim sr As StreamReader = New StreamReader(stream)
            str_array_objetos.Append(sr.ReadToEnd)
            If i_cont_objetos < i_num_objetos Then ' Si existen más elementos a agregar
                str_array_objetos.Append(",")
            End If
            i_cont_objetos = i_cont_objetos + 1
        Next
        str_array_objetos.Append("]")
        Return str_array_objetos.ToString
    End Function

    Public Shared Function ConvertirDictJson(ByVal dict As IDictionary) As String
        If dict Is Nothing OrElse dict.Count = 0 Then
            Return "{}"
        End If
        Dim str_map_objetos As New StringBuilder()
        Dim kSer As DataContractJsonSerializer = New DataContractJsonSerializer(dict.Keys(0).GetType)
        Dim vSer As DataContractJsonSerializer = New DataContractJsonSerializer(dict.Values(0).GetType)
        Dim stream As MemoryStream
        Dim sr As StreamReader
        str_map_objetos.Append("{")
        For Each k In dict.Keys
            stream = New MemoryStream()
            kSer.WriteObject(stream, k)
            stream.Position = 0
            sr = New StreamReader(stream)
            str_map_objetos.Append(sr.ReadToEnd)
            str_map_objetos.Append(":")
            stream = New MemoryStream()
            vSer.WriteObject(stream, dict.Item(k))
            stream.Position = 0
            sr = New StreamReader(stream)
            str_map_objetos.Append(sr.ReadToEnd)
            str_map_objetos.Append(",")
        Next
        str_map_objetos.Remove(str_map_objetos.Length - 1, 1)
        str_map_objetos.Append("}")
        Return str_map_objetos.ToString
    End Function

    Public Shared Function ObtenerListaDesdeJson(Of T)(ByVal astr_json As String) As List(Of T)
        Dim lista As New List(Of T)
        Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(lista.GetType())
        Dim stream As New MemoryStream(Encoding.Unicode.GetBytes(astr_json))
        lista = serializer.ReadObject(stream)
        Return lista
    End Function

    Private Shared dtSerializer As New JsonSerializer
    Shared Sub New()
        dtSerializer.NullValueHandling = NullValueHandling.Ignore
        dtSerializer.MissingMemberHandling = MissingMemberHandling.Ignore
        dtSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace
        dtSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        dtSerializer.Converters.Add(New Converters.DataTableConverter())
    End Sub

    Public Shared Function ConvertirDataTableJson(ByVal dt As DataTable) As String
        Dim sw As New StringWriter()
        Dim writer As New JsonTextWriter(sw)
        writer.Formatting = Formatting.None
        writer.QuoteChar = """"
        dtSerializer.Serialize(writer, dt)
        Dim resultado As String = sw.ToString()
        Return resultado
    End Function

End Class

