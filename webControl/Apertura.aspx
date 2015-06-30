<%@ Page Title="Apertura" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Apertura.aspx.vb" Inherits="webControl.Apertura" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .contenedor_eventos{
            width: 780px;
        }
    </style>

    <h2><%: Title %></h2> <br />

    <div class="row row-centered">
        <div class="col-centered">
            <div id="contenedor_pestanias" style="width:100%;">    
              <table>
                 <tr>
                     <td id="tab_1" class="pestania_seleccionada" onclick="filtrarPestañaTabs('tab1', 1);">
                         <asp:Label ID="lblTab1" runat="server" Text="Detalle" />
                     </td>
                   <td id="tab_2" class="pestania_no_seleccionada" onclick="filtrarPestañaTabs('tab2', 1);">
                       <asp:Label ID="lblTab2" runat="server" Text="General"/>
                   </td>
                </tr>
              </table>
            </div>

            <div id="filtro" class="filtro_oculto" style="width:100%">
                 <div id="tabs" class="contenedor_filtros">
                       
                     <div id="tabs-1" class="tab_invisible">

                     </div>
                              
                     <div id="tabs-2" class="tab_invisible">

                     </div>

                 </div>
            </div> 

            <div class="contenedor_eventos">
                <asp:Button ID="btnExportar" runat="server" Text="Exportar" CssClass="boton" />
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="boton" />
            </div>
            <div id="div_sgrid" style="width:780px; height:600px;" >
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnControlTab" runat="server" Text="" CssClass="invisible" />     
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdnTabSeleccionado" runat="server" />


<script type="text/javascript">

    var compsMap = {};
    var data = [];
    var listaColumnas;
    var sgrid;
    var columns = [];
    var docSelec = 0;

    var headDetalle = [{"alias":"Estado", "nombre_externo":"ESTADO","ancho_columna":"150", "alinear":"left", "id":1} ,
                       {"alias":"Convenio", "nombre_externo":"CONVENIO","ancho_columna":"150", "alinear":"left", "id":2} ,
                       {"alias":"Diplomado", "nombre_externo":"DIPLOMADO","ancho_columna":"280", "alinear":"left", "id":3} ,
                       {"alias":"CtroTrabajo", "nombre_externo":"CENTRO TRABAJO","ancho_columna":"200", "alinear":"left", "id":4}];

    var headGeneral = [{"alias": "Estado", "nombre_externo": "ESTADO", "ancho_columna": "180", "alinear": "left", "id": 1},
                       {"alias": "GrupoApertura", "nombre_externo": "GRUPO", "ancho_columna": "130", "alinear": "left", "id": 2},
                       {"alias": "CtroTrabajo", "nombre_externo": "NIVEL CT", "ancho_columna": "210", "alinear": "left", "id": 3}];

    var option = {
        enableCellNavigation: true,
        enableColumnReorder: true,
      };
    
    
    $(function() {
        Detalle();
        if (data.length <= 1) {
            $('[id$=btnExportar]').prop('disabled', true);
            $('[id$=btnExportar]').addClass('btn_disabled');
        } else {
            $('[id$=btnExportar]').prop('disabled', false);
            $('[id$=btnExportar]').removeClass('btn_disabled');
        }
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


    function Detalle() { 
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
                    if (tipo === 1) { Detalle(); }
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


</script>

</asp:Content>
