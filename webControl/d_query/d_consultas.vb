Imports MySql.Data.MySqlClient
Imports System.Configuration
Imports ClosedXML.Excel
Imports System.IO

Public Class d_consultas
    Inherits Page

    Dim connectionString = ConfigurationManager.ConnectionStrings("cnnDiplomado").ConnectionString
    Dim cn As New MySqlConnection(connectionString)

    Public Function GetItemsPorCursos(ByVal str_id_curso As String)

        Dim query As String = "SELECT GI.id AS itemid, GI.itemname, GI.courseid AS id_curso, " & _
            " (GI.aggregationcoef * 100) avance " & _
            " FROM mdl_grade_items GI WHERE GI.itemtype = 'mod' AND GI.courseid = " + str_id_curso.TrimEnd(",") & _
            " ORDER BY GI.courseid, GI.id;"

        Return GetDataSetDT(query)
    End Function

    Public Function GetInsignias()
        Return GetDataSetDT(GetQueryInsignias())
    End Function

    Public Function GetAperturaDetalle()
        Return GetDataSetDT(GetQueryAperturaDetalle())
    End Function

    Public Function GetAperturaGeneral()
        Return GetDataSetDT(GetQueryAperturaGeneral())
    End Function

    Public Function GetPromedioGrupo(ByVal str_items As String)
        Return GetDataSetDT(GetQueryPromedioGrupo(str_items))
    End Function

    Public Function GetPromedioGeneral(ByVal str_items As String)
        Return GetDataSetDT(GetQueryPromedioGeneral(str_items))
    End Function

    Public Function GetDataSetDT(ByVal str_query As String)
        cn.Open()

        Dim dt As New DataTable
        Dim ds As New DataSet

        Dim sqlcmd As MySqlCommand = New MySqlCommand(str_query, cn)
        sqlcmd.CommandTimeout = 0

        Dim sqladapter As MySqlDataAdapter = New MySqlDataAdapter(sqlcmd)

        sqladapter.Fill(ds)
        sqladapter.Dispose()
        sqlcmd.Dispose()

        cn.Close()
        dt = ds.Tables(0)
       
        Return dt
    End Function

    Public Function GetDataReader(ByVal str_query As String)
        cn.Open()

        Dim dt As New DataTable
        Dim ds As New DataSet

        Dim sqlcmd As MySqlCommand = New MySqlCommand(str_query, cn)
        sqlcmd.CommandTimeout = 0
        Dim DataReader As MySqlDataReader = sqlcmd.ExecuteReader()

        cn.Close()

        Return DataReader
    End Function


#Region "Query Insignias"

    Private Function GetQueryInsignias()
        Return "SELECT CONCAT(MU.firstname,' ', MU.lastname) AS NomComp, MU.email AS Email " & _
                "	, IDC.data AS Correo2, MU.phone1 AS Tel1, MU.phone2 AS Tel2, IDE.data AS Estado, IDCT.data AS CentTrab " & _
                "    , IDNV.data AS NivelCT, SUM(MGG.finalgrade *  MGI.aggregationcoef) AS CalifObt " & _
                "    , MC.fullname AS Diplomado, FROM_UNIXTIME(MC.startdate) AS FechaApertura, IFNULL(qTotal.total, 0) AS tTotal " & _
                "    , IFNULL(qBRONCE.bronce, 'NO') as Bronce, IFNULL(qPLATA.plata, 'NO') as Plata, IFNULL(qORO.oro, 'NO') as Oro " & _
                "    , IFNULL(qPLATINO.platino, 'NO') as Platino, IFNULL(qBIRRETE.birrete, 'NO') as Birrete, IFNULL(qMOUSE.mouse, 'NO') as Mouse " & _
                "FROM mdl_role  MR " & _
                "INNER JOIN mdl_role_assignments MRASS ON (MRASS.roleid = MR.id) " & _
                "INNER JOIN mdl_user MU ON (MRASS.userid = MU.id ) " & _
                "INNER JOIN mdl_user_info_data IDC ON (IDC.userid = MU.id AND IDC.fieldid = 7) " & _
                "INNER JOIN mdl_user_info_data IDE ON (IDE.userid = MU.id AND IDE.fieldid = 10) " & _
                "INNER JOIN mdl_user_info_data IDCT ON (MU.id = IDCT.userid AND IDCT.fieldid  = 18) " & _
                "INNER JOIN mdl_user_info_data IDNV ON (MU.id = IDNV.userid AND IDNV.fieldid  = 24) " & _
                "INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                "INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "INNER JOIN mdl_course MC ON (MC.id = MGI.courseid) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS platino, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%platino%') " & _
                "	)qPLATINO ON (qPLATINO.userid =mu.id AND qPLATINO.courseid = MC.id ) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS oro, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%oro%') " & _
                "	)qORO ON (qORO.userid=mu.id AND qORO.courseid = MC.id) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS plata, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%plata%') " & _
                "	)qPLATA ON (qPLATA.userid =mu.id AND qPLATA.courseid = MC.id) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS bronce, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%bronce%') " & _
                "	)qBRONCE ON (qBRONCE.userid =mu.id AND qBRONCE.courseid = MC.id)" & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS birrete, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%birrete%') " & _
                "	)qBIRRETE ON (qBIRRETE.userid =mu.id AND qBIRRETE.courseid = MC.id) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.name AS mouse, MB.courseid FROM mdl_badge_issued MBI " & _
                "			INNER JOIN mdl_badge MB ON (MB.id = MBI.badgeid AND MB.name like '%mouse%') " & _
                "	)qMOUSE ON (qMOUSE.userid =mu.id AND qMOUSE.courseid = MC.id) " & _
                "LEFT JOIN (SELECT MBI.userid, MB.courseid, COUNT(MBI.badgeid) AS total FROM mdl_course MC " & _
                "			INNER JOIN mdl_badge MB ON (MB.courseid = MC.id) " & _
                "			INNER JOIN  mdl_badge_issued MBI ON (MBI.badgeid = MB.id) " & _
                "			GROUP BY MBI.userid " & _
                "	)qTotal ON (qTotal.userid =mu.id AND qTotal.courseid = MC.id) " & _
                "WHERE MRASS.roleid = 5 AND MGI.itemtype = 'mod' " & _
                "GROUP BY MGG.userid ORDER BY MGG.userid;"

    End Function

#End Region

#Region "Query Aperturas"

    Private Function GetQueryAperturaDetalle()
        Return "SELECT UIDED.data AS Estado, UIDO.data AS Convenio " & _
                "   , MC.fullname AS Diplomado, UIDNC.data AS CtroTrabajo " & _
                " FROM mdl_user MU " & _
                " INNER JOIN mdl_user_info_data UIDED ON (UIDED.userid = MU.id AND UIDED.fieldid = 10) " & _
                " INNER JOIN mdl_user_info_data UIDO ON (UIDO.userid = MU.id AND UIDO.fieldid = 20) " & _
                " INNER JOIN mdl_user_info_data UIDNC ON (UIDNC.userid = MU.id AND UIDNC.fieldid = 24) " & _
                " INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                " INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                " INNER JOIN mdl_course MC ON (MC.id = MGI.courseid);"
    End Function

    Private Function GetQueryAperturaGeneral()
        Return "SELECT UIDED.data AS Estado " & _
                "    , CASE WHEN (MC.fullname LIKE '%Grupo A%' || MC.fullname LIKE '%Grupo B%') THEN 'Grupo A y B 1' " & _
                "        WHEN (MC.fullname LIKE '%Grupo C%' || MC.fullname LIKE '%Grupo D%') THEN 'Grupo C y D 2' " & _
                "        WHEN (MC.fullname LIKE '%Grupo E%' || MC.fullname LIKE '%Grupo F%') THEN 'Grupo E y F 3' " & _
                "        WHEN (MC.fullname LIKE '%Grupo G%' || MC.fullname LIKE '%Grupo H%') THEN 'Grupo G y H 4' " & _
                "        WHEN (MC.fullname LIKE '%Grupo M%' || MC.fullname LIKE '%Grupo N%') THEN 'Grupo M y N 5' " & _
                "		WHEN (MC.fullname LIKE '%Grupo O%' || MC.fullname LIKE '%Grupo P%') THEN 'Grupo O y P 6' " & _
                "    ELSE 'Grupo No Encontrado' END AS GrupoApertura, UIDNC.data AS CtroTrabajo " & _
                "FROM mdl_user MU " & _
                "INNER JOIN mdl_user_info_data UIDED ON (UIDED.userid = MU.id AND UIDED.fieldid = 10) " & _
                "INNER JOIN mdl_user_info_data UIDNC ON (UIDNC.userid = MU.id AND UIDNC.fieldid = 24) " & _
                "INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                "INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "INNER JOIN mdl_course MC ON (MC.id = MGI.courseid) " & _
                "GROUP BY MGG.userid;"
    End Function

#End Region

#Region "Query Alumno Estado"

    Private Function GetQueryAlumnoEstado()

        Return "SELECT UIDED.userid, CONCAT(MU.firstname,' ', MU.lastname) AS NomComp, UIDED.data AS Estado " & _
                "	, UIDNC.data AS CtroTrabajo " & _
                "    , CASE WHEN (MC.fullname LIKE '%Grupo A%' || MC.fullname LIKE '%Grupo B%') THEN 'Grupo A y B 1' " & _
                "        WHEN (MC.fullname LIKE '%Grupo C%' || MC.fullname LIKE '%Grupo D%') THEN 'Grupo C y D 2' " & _
                "        WHEN (MC.fullname LIKE '%Grupo E%' || MC.fullname LIKE '%Grupo F%') THEN 'Grupo E y F 3' " & _
                "        WHEN (MC.fullname LIKE '%Grupo G%' || MC.fullname LIKE '%Grupo H%') THEN 'Grupo G y H 4' " & _
                "        WHEN (MC.fullname LIKE '%Grupo M%' || MC.fullname LIKE '%Grupo N%') THEN 'Grupo M y N 5' " & _
                "		WHEN (MC.fullname LIKE '%Grupo O%' || MC.fullname LIKE '%Grupo P%') THEN 'Grupo O y P 6' " & _
                "    ELSE 'Grupo No Encontrado' END AS GrupoApertura, MC.fullname AS Diplo " & _
                "FROM mdl_user MU " & _
                "INNER JOIN mdl_user_info_data UIDED ON (UIDED.userid = MU.id AND UIDED.fieldid = 10) " & _
                "INNER JOIN mdl_user_info_data UIDNC ON (UIDNC.userid = MU.id AND UIDNC.fieldid = 24) " & _
                "INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                "INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "INNER JOIN mdl_course MC ON (MC.id = MGI.courseid) " & _
                "GROUP BY MU.id; "

    End Function

#End Region

#Region "Query Promedio"

    Private Function GetQueryPromedioGrupo(ByVal str_items As String)

        Return "SELECT MU.username AS Usuario, IDPW.data AS Contrasenia, IDCV.data AS Convenio, CONCAT(MU.firstname,' ', MU.lastname) AS NombComp " & _
                "	, MU.email AS CorreoE, MU.phone1, MU.phone2, IDE.data AS Estado, IDCVCT.data AS ClvCentroTrabajo, IDCT.data AS CentroTrabajo " & _
                "    , IDNVCT.data AS NvlCentroTrabajo, DATEDIFF(now(),FROM_UNIXTIME(ULAC.timeaccess)) as DiasSinIngresar " & _
                "    , IF(MU.lastaccess=0,'NO','SI') as IngresoPlataforma, IF(IDEDPG.data=0,'NO','SI') AS EstPago, MG.name AS Grupo " & _
                "    , MC.fullname AS Diplomado, qsData.avance AS Actividad, qsData.calificacion AS CalifActual, qsData.estado AS EstAcademico " & _
                "    , FROM_UNIXTIME(MC.startdate) AS FechaApertura," & _
                "  CASE WHEN (MC.fullname LIKE '%Grupo A%' || MC.fullname LIKE '%Grupo B%') THEN 'Grupo A y B 1' " & _
                " WHEN (MC.fullname LIKE '%Grupo C%' || MC.fullname LIKE '%Grupo D%') THEN 'Grupo C y D 2' " & _
                " WHEN (MC.fullname LIKE '%Grupo E%' || MC.fullname LIKE '%Grupo F%') THEN 'Grupo E y F 3' " & _
                " WHEN (MC.fullname LIKE '%Grupo G%' || MC.fullname LIKE '%Grupo H%') THEN 'Grupo G y H 4' " & _
                " WHEN (MC.fullname LIKE '%Grupo M%' || MC.fullname LIKE '%Grupo N%') THEN 'Grupo M y N 5' " & _
                " WHEN (MC.fullname LIKE '%Grupo O%' || MC.fullname LIKE '%Grupo P%') THEN 'Grupo O y P 6' " & _
                " ELSE 'Grupo No Encontrado' END AS GrupoApertura " & _
                "FROM mdl_user MU " & _
                "INNER JOIN mdl_user_info_data IDPW ON (IDPW.userid = MU.id AND IDPW.fieldid  = 23) " & _
                "INNER JOIN mdl_user_info_data IDCV ON (IDCV.userid = MU.id AND IDCV.fieldid = 20) " & _
                "INNER JOIN mdl_user_info_data IDE ON (IDE.userid = MU.id AND IDE.fieldid = 10) " & _
                "INNER JOIN mdl_user_info_data IDCVCT ON (IDCVCT.userid = MU.id AND IDCVCT.fieldid  = 17) " & _
                "INNER JOIN mdl_user_info_data IDCT ON (IDCT.userid = MU.id AND IDCT.fieldid  = 18) " & _
                "INNER JOIN mdl_user_info_data IDNVCT ON (IDNVCT.userid = MU.id AND IDNVCT.fieldid  = 24) " & _
                "INNER JOIN mdl_user_info_data IDEDPG ON (IDEDPG.userid = MU.id AND IDEDPG.fieldid  = 22) " & _
                "INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                "INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "INNER JOIN mdl_course MC ON (MC.id = MGI.courseid) " & _
                "INNER JOIN mdl_user_lastaccess ULAC ON (ULAC.userid = MU.id AND ULAC.courseid = MC.id) " & _
                "INNER JOIN mdl_groups MG ON (MG.courseid = MC.id) " & _
                "INNER JOIN mdl_groups_members MGM ON (MGM.groupid = MG.id AND MGM.userid = MU.id) " & _
                "INNER JOIN (SELECT MGG.userid, ROUND(SUM(ROUND(MGG.finalgrade,2) * ROUND(MGI.aggregationcoef,2)),2) AS calificacion , MGI.courseid, " & _
                "   CASE WHEN (ID.data='BAJA POR SOLICITUD') THEN 'BAJA POR SOLICITUD' " & _
                "   WHEN (ID.data='BAJA POR INACTIVIDAD') THEN 'BAJA POR INACTIVIDAD' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef)) < 60 THEN 'EN RIESGO (1-59)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 60 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 70 ) THEN 'EN RIESGO (60-69)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 70 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 80 ) THEN 'AL CORRIENTE (70-79)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 80 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 100 ) THEN 'AL CORRIENTE (80-100)' " & _
                "   ELSE 'BAJA POR INACTIVIDAD' END AS estado, SUM(MGI.aggregationcoef * 100) avance FROM mdl_grade_grades MGG " & _
                "   INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "   INNER JOIN mdl_user_info_data ID ON (ID.userid = MGG.userid AND ID.fieldid  = 1) " & _
                "   WHERE MGG.itemid IN (" + str_items + ")" & _
                "   GROUP BY MGG.userid " & _
                "   ) qsData ON (qsData.userid = MU.id AND qsData.courseid = MC.id) " & _
                "GROUP BY IDPW.userid " & _
                "ORDER BY MG.name; "

    End Function

    Private Function GetQueryPromedioGeneral(ByVal str_items As String)

        Return "SELECT MU.username AS Usuario, IDPW.data AS Contrasenia, IDCV.data AS Convenio, CONCAT(MU.firstname,' ', MU.lastname) AS NombComp " & _
                "	, MU.email AS CorreoE, MU.phone1, MU.phone2, IDE.data AS Estado, IDCVCT.data AS ClvCentroTrabajo, IDCT.data AS CentroTrabajo " & _
                "    , IDNVCT.data AS NvlCentroTrabajo, DATEDIFF(now(),FROM_UNIXTIME(ULAC.timeaccess)) as DiasSinIngresar " & _
                "    , IF(MU.lastaccess=0,'NO','SI') as IngresoPlataforma, IF(IDEDPG.data=0,'NO','SI') AS EstPago, MG.name AS Grupo " & _
                "    , MC.fullname AS Diplomado, qsData.avance AS Actividad, qsData.calificacion AS CalifActual, qsData.estado AS EstAcademico " & _
                "FROM mdl_user MU " & _
                "INNER JOIN mdl_user_info_data IDPW ON (IDPW.userid = MU.id AND IDPW.fieldid  = 23) " & _
                "INNER JOIN mdl_user_info_data IDCV ON (IDCV.userid = MU.id AND IDCV.fieldid = 20) " & _
                "INNER JOIN mdl_user_info_data IDE ON (IDE.userid = MU.id AND IDE.fieldid = 10) " & _
                "INNER JOIN mdl_user_info_data IDCVCT ON (IDCVCT.userid = MU.id AND IDCVCT.fieldid  = 17) " & _
                "INNER JOIN mdl_user_info_data IDCT ON (IDCT.userid = MU.id AND IDCT.fieldid  = 18) " & _
                "INNER JOIN mdl_user_info_data IDNVCT ON (IDNVCT.userid = MU.id AND IDNVCT.fieldid  = 24) " & _
                "INNER JOIN mdl_user_info_data IDEDPG ON (IDEDPG.userid = MU.id AND IDEDPG.fieldid  = 22) " & _
                "INNER JOIN mdl_grade_grades MGG ON (MGG.userid = MU.id) " & _
                "INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "INNER JOIN mdl_course MC ON (MC.id = MGI.courseid) " & _
                "INNER JOIN mdl_user_lastaccess ULAC ON (ULAC.userid = MU.id AND ULAC.courseid = MC.id) " & _
                "INNER JOIN mdl_groups MG ON (MG.courseid = MC.id) " & _
                "INNER JOIN mdl_groups_members MGM ON (MGM.groupid = MG.id AND MGM.userid = MU.id) " & _
                "INNER JOIN (SELECT MGG.userid, ROUND(SUM(ROUND(MGG.finalgrade,2) * ROUND(MGI.aggregationcoef,2)),2) AS calificacion , MGI.courseid, " & _
                "   CASE WHEN (ID.data='BAJA POR SOLICITUD') THEN 'BAJA POR SOLICITUD' " & _
                "   WHEN (ID.data='BAJA POR INACTIVIDAD') THEN 'BAJA POR INACTIVIDAD' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef)) < 60 THEN 'EN RIESGO (1-59)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 60 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 70 ) THEN 'EN RIESGO (60-69)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 70 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 80 ) THEN 'AL CORRIENTE (70-79)' " & _
                "   WHEN (SUM(MGG.finalgrade * MGI.aggregationcoef) >= 80 AND SUM(MGG.finalgrade * MGI.aggregationcoef) < 100 ) THEN 'AL CORRIENTE (80-100)' " & _
                "   ELSE 'BAJA POR INACTIVIDAD' END AS estado, SUM(MGI.aggregationcoef * 100) avance FROM mdl_grade_grades MGG " & _
                "   INNER JOIN mdl_grade_items MGI ON (MGI.id = MGG.itemid) " & _
                "   INNER JOIN mdl_user_info_data ID ON (ID.userid = MGG.userid AND ID.fieldid  = 1) " & _
                "   WHERE MGG.itemid IN (" + str_items + ")" & _
                "   GROUP BY MGG.userid " & _
                "   ) qsData ON (qsData.userid = MU.id AND qsData.courseid = MC.id) " & _
                "GROUP BY IDPW.userid " & _
                "ORDER BY MG.name; "

    End Function

#End Region

#Region "Query Ejecutivo"



#End Region

    'Public Sub GetExportarExcel(ByVal dt As DataTable, pagName As String, ByVal sheetName As String)
    '    'Referencia
    '    'http:// www.aspsnippets.com/Articles/Write-data-to-Excel-file-xls-and-xlsx-in-ASPNet.aspx
    '    'Paquetes
    '    'Install-Package DocumentFormat.OpenXml
    '    'Install-Package ClosedXML
    '    'Install-Package ExcelFile.net

    '    Using wb As New XLWorkbook()
    '        wb.Worksheets.Add(dt, pagName)

    '        Response.Clear()
    '        Response.Buffer = True
    '        Response.Charset = ""
    '        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    '        Response.AddHeader("content-disposition", "attachment;filename=" + sheetName + Date.Now.ToString("yyyy-MM-dd") + ".xlsx")
    '        Using MyMemoryStream As New MemoryStream()
    '            wb.SaveAs(MyMemoryStream)
    '            MyMemoryStream.WriteTo(Response.OutputStream)
    '            Response.Flush()
    '            Response.End()
    '        End Using
    '    End Using
    'End Sub

End Class



