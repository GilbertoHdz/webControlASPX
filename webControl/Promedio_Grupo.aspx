<%@ Page Title="Promedio Grupo" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Promedio_Grupo.aspx.vb" Inherits="webControl.Promedio_Grupo" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Title %></h2>

    <style>
        .contenedor_eventos{
            width: 900px;
        }
        .contenedor_filtros {	 
            height:auto;
            width: 900px;
        }
    </style>

    <div class="row row-centered">
        <div class="col-centered">

            <div id="contenedor_pestanias" style="width:100%;">    
              <table>
                 <tr>
                     <td id="tab_1" class="pestania_seleccionada" onclick="filtrarPestañaTabs('tab1', 1);">
                         <asp:Label ID="lblTab1" runat="server" Text="Promedios por Grupo" />
                     </td>
                   <td id="tab_2" class="pestania_no_seleccionada" onclick="filtrarPestañaTabs('tab2', 1);">
                       <asp:Label ID="lblTab2" runat="server" Text="Promedios General"/>
                   </td>
                </tr>
              </table>
            </div>

            <div id="filtro" class="" style="width:100%">
                 <div id="tabs" class="contenedor_filtros">
                       
                     <div id="tabs-1">

                         <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td><asp:Label ID="lblGrupo" runat="server" Text="Grupos" CssClass="texto" />:</td>
                                            <td><asp:DropDownList ID="ddlGrupos" runat="server" class="ui-multiselect ui-widget ui-state-default ui-corner-all"  ></asp:DropDownList></td>
                                        </tr>
                                        <tr id="trContenedorGruposAgregados" style="display:none;">
                                            <td class="tdb_etiquetas"></td>
                                            <td colspan="4">
                                                <table id="tblGruposAgregados" style="width: 330px !important;">
                                                    <thead>
                                                        <tr>
                                                            <th class="TRHead" style="display: none;">idSolicito</th><th class="TRHead"><asp:Label ID="lblGrupos" runat="server" Text="GRUPOS AGREGADOS" /></th><th class="TRHead"></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                                <asp:HiddenField ID="hdnIdGrupo" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                       </table>

                     </div>
                              
                     <div id="tabs-2" class="tab_invisible">

                     </div>

                 </div>
            </div> 

            <div class="contenedor_eventos">
                <asp:Button ID="btnExportar" runat="server" Text="Exportar" OnClientClick="jsonSgrid();" CssClass="boton" />
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClientClick="obtenerDatos();" CssClass="boton" />
            </div>
            <div id="div_sgrid" style="width:900px; height:600px;" >
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnControlTab" runat="server" Text="" CssClass="invisible" />     
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdnTabSeleccionado" runat="server" />
    <asp:HiddenField ID="hdnDataFiltro" runat="server" />
    <asp:HiddenField ID="hdnJsonSgrid" runat="server" />

<script type="text/javascript">

    var compsMap = {};
    var data = [];
    var listaColumnas;
    var sgrid;
    var columns = [];
    var docSelec = 0;

    var headDetalle = [{ "alias": "Usuario", "nombre_externo": "USUARIO", "ancho_columna": "100", "alinear": "left", "id": 1 },
                         { "alias": "Contrasenia", "nombre_externo": "CONTRASEÑA", "ancho_columna": "100", "alinear": "left", "id": 2 },
                         { "alias": "Convenio", "nombre_externo": "CONVENIO", "ancho_columna": "200", "alinear": "left", "id": 3 },
                         { "alias": "NombComp", "nombre_externo": "NOMBRE COMPLETO", "ancho_columna": "350", "alinear": "left", "id": 4 },
                         { "alias": "CorreoE", "nombre_externo": "CORREO ELECTRONICO", "ancho_columna": "150", "alinear": "left", "id": 5 },
                         { "alias": "phone1", "nombre_externo": "TELEFONO", "ancho_columna": "150", "alinear": "center", "id": 6 },
                         { "alias": "phone2", "nombre_externo": "CELULAR", "ancho_columna": "150", "alinear": "center", "id": 7 },
                         { "alias": "Estado", "nombre_externo": "ESTADO", "ancho_columna": "150", "alinear": "left", "id": 8 },
                         { "alias": "ClvCentroTrabajo", "nombre_externo": "CLV. CENT. TRABJ.", "ancho_columna": "150", "alinear": "left", "id": 9 },
                         { "alias": "CentroTrabajo", "nombre_externo": "CENTRO TRABAJO", "ancho_columna": "150", "alinear": "left", "id": 10 },
                         { "alias": "NvlCentroTrabajo", "nombre_externo": "NVL.CENTRO TRABAJO", "ancho_columna": "150", "alinear": "left", "id": 11 },
                         { "alias": "DiasSinIngresar", "nombre_externo": "DIAS SIN INGR.", "ancho_columna": "150", "alinear": "left", "id": 12 },
                         { "alias": "IngresoPlataforma", "nombre_externo": "INGR: PLATAFORMA", "ancho_columna": "150", "alinear": "left", "id": 13 },
                         { "alias": "EstPago", "nombre_externo": "EDO. PAGO", "ancho_columna": "150", "alinear": "left", "id": 14 },
                         { "alias": "Grupo", "nombre_externo": "GRUPO", "ancho_columna": "150", "alinear": "left", "id": 15 },
                         { "alias": "Diplomado", "nombre_externo": "DIPLOMADO", "ancho_columna": "150", "alinear": "left", "id": 16 },
                         { "alias": "Actividad", "nombre_externo": "ACTIVIDAD", "ancho_columna": "150", "alinear": "left", "id": 17 },
                         { "alias": "CalifActual", "nombre_externo": "CAL. ACTUAL", "ancho_columna": "150", "alinear": "right", "id": 18 },
                         { "alias": "EstAcademico", "nombre_externo": "EDO. ACADEMICO", "ancho_columna": "150", "alinear": "left", "id": 19 }];

    var headGeneral = [{ "alias": "Usuario", "nombre_externo": "USUARIO", "ancho_columna": "100", "alinear": "left", "id": 1 },
                         { "alias": "Contrasenia", "nombre_externo": "CONTRASEÑA", "ancho_columna": "100", "alinear": "left", "id": 2 },
                         { "alias": "Convenio", "nombre_externo": "CONVENIO", "ancho_columna": "200", "alinear": "left", "id": 3 },
                         { "alias": "NombComp", "nombre_externo": "NOMBRE COMPLETO", "ancho_columna": "350", "alinear": "left", "id": 4 },
                         { "alias": "CorreoE", "nombre_externo": "CORREO ELECTRONICO", "ancho_columna": "150", "alinear": "left", "id": 5 },
                         { "alias": "phone1", "nombre_externo": "TELEFONO", "ancho_columna": "150", "alinear": "center", "id": 6 },
                         { "alias": "phone2", "nombre_externo": "CELULAR", "ancho_columna": "150", "alinear": "center", "id": 7 },
                         { "alias": "Estado", "nombre_externo": "ESTADO", "ancho_columna": "150", "alinear": "left", "id": 8 },
                         { "alias": "ClvCentroTrabajo", "nombre_externo": "CLV. CENT. TRABJ.", "ancho_columna": "150", "alinear": "left", "id": 9 },
                         { "alias": "CentroTrabajo", "nombre_externo": "CENTRO TRABAJO", "ancho_columna": "150", "alinear": "left", "id": 10 },
                         { "alias": "NvlCentroTrabajo", "nombre_externo": "NVL.CENTRO TRABAJO", "ancho_columna": "150", "alinear": "left", "id": 11 },
                         { "alias": "DiasSinIngresar", "nombre_externo": "DIAS SIN INGR.", "ancho_columna": "150", "alinear": "left", "id": 12 },
                         { "alias": "IngresoPlataforma", "nombre_externo": "INGR: PLATAFORMA", "ancho_columna": "150", "alinear": "left", "id": 13 },
                         { "alias": "EstPago", "nombre_externo": "EDO. PAGO", "ancho_columna": "150", "alinear": "left", "id": 14 },
                         { "alias": "Grupo", "nombre_externo": "GRUPO", "ancho_columna": "150", "alinear": "left", "id": 15 },
                         { "alias": "Diplomado", "nombre_externo": "DIPLOMADO", "ancho_columna": "150", "alinear": "left", "id": 16 },
                         { "alias": "Actividad", "nombre_externo": "ACTIVIDAD", "ancho_columna": "150", "alinear": "left", "id": 17 },
                         { "alias": "CalifActual", "nombre_externo": "CAL. OBTENIDA", "ancho_columna": "150", "alinear": "right", "id": 18 },
                         { "alias": "EstAcademico", "nombre_externo": "EDO. ACADEMICO", "ancho_columna": "150", "alinear": "left", "id": 19 },
                         { "alias": "FechaApertura", "nombre_externo": "F.APERTURA", "ancho_columna": "150", "alinear": "center", "id": 20 },
                         { "alias": "GrupoApertura", "nombre_externo": "GPO.APERTURA", "ancho_columna": "150", "alinear": "left", "id": 21 }];


    var option = {
        enableCellNavigation: true,
        enableColumnReorder: true,
    };


    $(function () {
        Grupo();
        reconstruirParametroGrupo();

        if (data.length <= 1) {
            $('[id$=btnExportar]').prop('disabled', true);
            $('[id$=btnExportar]').addClass('btn_disabled');
        } else {
            $('[id$=btnExportar]').prop('disabled', false);
            $('[id$=btnExportar]').removeClass('btn_disabled');
        }

        $('[id$=ddlGrupos]').change(function () {
            var result = $(this).val();

            agregarParametroTabla(result, result, "trContenedorGruposAgregados", "tblGruposAgregados", "hdnIdGrupo");
            $('[id$=ddlGrupos]').val(-1);
        });

    });

    function AjustarTamaño() {
        if (sgrid.getDataLength() < 20) {
            $('[id$=div_sgrid]').height(25 * (sgrid.getDataLength() + 2))
            $('.slick-viewport').css('height', 25 * (sgrid.getDataLength() + 1));
            $('.grid-canvas').css('height', 25 * (sgrid.getDataLength()));

        } else {
            $('[id$=div_sgrid]').height(500)
        }
        sgrid.resizeCanvas();
    }


    function CargarGrid() {

        //if (docSelec) {
        //    fotmatoFilas();
        //}

        sgrid = new Slick.Grid("#div_sgrid", data, columns, option);

        sgrid.onBeforeEditCell.subscribe(function (e, args) {
            var item = sgrid.getDataItem(args.row);
            if (typeof item === "undefined") return true;
            if (args.cell == 1) return false;
            if (args.cell == 2 && !error) return false;
        });

        sgrid.onCellChange.subscribe(function (e, args) {
            var item = args.item;
            var data = sgrid.getData();

            sgrid.setData(data);
            sgrid.render();

            var numFila = parseInt(args.row);
            var numCelda = parseInt(args.cell);

        });

        sgrid.onActiveCellChanged.subscribe(function (e, args) {
            var item = args.item;
            var data = sgrid.getData();

            if (sgrid.getColumns()[args.cell]) {
                var nombreColumna = sgrid.getColumns()[args.cell].field;
                //Codigo a ejecutar
            }
        });

        AjustarTamaño();
    }


    function CrearColumnas(tipoSelec) {
        var columnas = [];
        $.each(listaColumnas, function (idx, item) {
            if (item) {
                var ancho = parseInt(item.ancho_columna);

                switch (tipoSelec) {
                    case 1:
                        columnas.push({ name: item.nombre_externo, id: item.alias, field: item.alias, minWidth: ancho, maxWidth: ancho + 100, cssClass: item.alineacion });
                        break;
                    case 2:
                        columnas.push({ name: item.nombre_externo, id: item.alias, field: item.alias, minWidth: ancho, maxWidth: ancho + 100, cssClass: item.alineacion });
                        break;
                    default: columnas.push({ name: item.nombre_externo, id: item.alias, field: item.alias, minWidth: ancho, maxWidth: ancho + 100, cssClass: item.alineacion });
                }

            }
        });
        return columnas;
    }


    function Grupo() {
        listaColumnas = headDetalle;
        columns = CrearColumnas(1);
        data = <%=str_dtdetalle_json%>
        docSelec = 1;
        CargarGrid();
    }

    function General() {
        listaColumnas = headGeneral;
        data = <%=str_dtgeneral_json%>
        columns = CrearColumnas(2);
        docSelec = 0;
        CargarGrid();
    }

    function filtrarPestañaTabs(tab, tipo) {

        if (tab) {
            switch (tab) {
                case 'tab1':
                    $('[id$=hdnTabSeleccionado]').val(tab);
                    $('[id$=tabs-1]').removeClass('tab_invisible');
                    $('[id$=tabs-2]').addClass('tab_invisible');

                    $('[id$=tab_1]').removeClass('pestania_no_seleccionada pestania_seleccionada').addClass('pestania_seleccionada');
                    $('[id$=tab_2]').removeClass('pestania_seleccionada pestania_no_seleccionada').addClass('pestania_no_seleccionada');
                    if (tipo === 1) { Grupo(); }
                    $('#<%= btnControlTab.ClientID %>').click();
                    break;
                case 'tab2':
                    $('[id$=hdnTabSeleccionado]').val(tab);
                    $('[id$=tabs-1]').addClass('tab_invisible');
                    $('[id$=tabs-2]').removeClass('tab_invisible');
                    $('[id$=tab_1]').removeClass('pestania_seleccionada pestania_no_seleccionada').addClass('pestania_no_seleccionada');
                    $('[id$=tab_2]').removeClass('pestania_no_seleccionada pestania_seleccionada').addClass('pestania_seleccionada');
                    if (tipo === 1) { General(); }
                    $('#<%= btnControlTab.ClientID %>').click();
                    break;

                default: console.log(tab);
            }
        }
    }


    function reconstruirParametroGrupo() {
        var grupos = $('[id$=hdnIdGrupo]').val();
        if (grupos != "" && grupos != null) {
            var grupos_buscados = grupos.split('&');
            $.each(grupos_buscados, function (key, grupo) {
                if (grupo != "" && grupo != null) {
                    var grupos_buscados = grupo.split('|')
                    agregarParametroTabla(grupos_buscados[0], grupos_buscados[1], "trContenedorGruposAgregados", "tblGruposAgregados", "hdnIdGrupo");
                }
            });
        }
    }


    /* 
      $(':checkbox:checked').map(function() {return this.value;}).get().join(',');
    */


    function obtenerDatos() {
        $('[id$=hdnDataFiltro]').val($(':checkbox:checked').map(function () { return this.value; }).get().join(','));
    }

    function jsonSgrid() {
        $('[id$=hdnJsonSgrid]').val(JSON.stringify(sgrid.getData()));
    }


    //Ajax
    function getItems(ids) {
        var nombre = ids.split(' ')[0] + ids.split(' ')[1];
        $.ajax({
            url: "../webControl/Ajax/ajax_items_por_curso.ashx",
            dataType: "json",
            data: { termino: ids.split('_')[1] },
            success: function (data) {
                agregarSubCombo(ids, nombre, data)
            }
        });

    }

    function agregarSubCombo(ids, nombre, data) {
        var combo = $("<select></select>").attr("id", nombre).attr("name", nombre).attr("class", 'ddlMultiple').css("width", "225");

        $(data).each(function () {
            combo.append($("<option>").attr('value', ids.split('_')[0] + '_' + this.id_curso + '_' + this.itemid + '_' + this.valoracion).text(this.itemname));
        });

        $("#" + ids.split(" ")[1] + "").append(combo);
        $('.ddlMultiple').multiselect();

        return true;
    }

    /*Constructores para los agregados en tablas */
    function agregarParametroTabla(idParametro, nombreParametro, id_contenedor, id_tabla, id_ocultoIdsAlmacenados, filtroUtilizado) {
        idParametro = idParametro.replace(/,+$/, "");
        var data = idParametro.split(',');
        for (var i in data) {
            idParametro = (data[i]);

            if (!estaAgregadoParametroTabla(idParametro, id_tabla)) {
                $('[id$=' + id_contenedor + ']').show();
                var parametrosEliminar = "'" + idParametro + "', '" + id_contenedor + "', '" + id_tabla + "', '" + id_ocultoIdsAlmacenados + "'";
                if (filtroUtilizado) {
                    parametrosEliminar += ", '" + filtroUtilizado + "'"
                }

                var fila = "<tr>" +
                               "<td style='display: none;'>" + idParametro + "</td>" +
                               "<td style='font-weight: bold;'>" + idParametro.split("_")[0] + " <div id='" + idParametro.split(" ")[1] + "' style='float:right;'></div> </td>" +
                               "<td style='text-align:right;'><span class='glyphicon glyphicon-remove' width='25px' height='25px' " +
                                    " style='cursor: pointer;' " +
                                    'onclick="eliminarParametroTabla(' + parametrosEliminar + ');"></span>' +
                               "</td>" +
                           "<tr>";

                $('[id$=' + id_tabla + '] tbody').append(fila);

                getItems(idParametro);

                var idAgregados = "";
                $("[id$=" + id_tabla + "] tbody tr").each(function (index) {
                    if ($(this).children("td").length > 0) {
                        var id = $(this).find("td:first").html();
                        idAgregados = idAgregados + id + ",";
                    }
                })
                $("[id$=" + id_ocultoIdsAlmacenados + "]").val(idAgregados);
            }
        }
    }

    function estaAgregadoParametroTabla(idAgregar, id_tabla) {
        var estaAgregado = false;
        $("[id$=" + id_tabla + "] tbody tr").each(function (index) {
            if ($(this).children("td").length > 0) {
                var id = $(this).find("td:first").html();
                if (id == idAgregar) {
                    estaAgregado = true;
                }
            }
        });
        return estaAgregado;
    }

    function eliminarParametroTabla(idEliminar, id_contenedor, id_tabla, id_ocultoIdsAlmacenados, filtroUtilizado) {
        var idAgregados = "";
        var elementoEliminar;
        $("[id$=" + id_tabla + "] tbody tr").each(function (index) {
            if ($(this).children("td").length > 0) {
                var id = $(this).find("td:first").html();
                if (id == idEliminar) {
                    elementoEliminar = this;
                } else {
                    idAgregados = idAgregados + id + ",";
                }
            }
        });
        $(elementoEliminar).remove();
        $("[id$=" + id_ocultoIdsAlmacenados + "]").val(idAgregados);
        if (idAgregados == "") {
            $('[id$=' + id_contenedor + ']').hide();
        }
    }



</script>

</asp:Content>
