<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="webControl._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row row-centered">
        <div class="col-centered">
            <div class="contenedor_eventos">
                <asp:Button ID="btnExportar" runat="server" Text="Exportar" CssClass="boton" />
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="boton" />
            </div>
            <div id="div_sgrid" style="width:950px; height:600px;" >
            </div>
        </div>
    </div>


<script type="text/javascript">

    var compsMap = {};
    var data_json = [];
    var colum = [];

    var option = {
        enableCellNavigation: true,
        enableColumnReorder: true,
      };
    
    
    $(function() { 
        
        data_json = <%=str_dt_json%>;

        if (data_json.length <= 1) {
            $('[id$=btnExportar]').prop('disabled', true);
            $('[id$=btnExportar]').addClass('btn_disabled');
        } else {
            $('[id$=btnExportar]').prop('disabled', false);
            $('[id$=btnExportar]').removeClass('btn_disabled');
        }

        colum.push({ name: "Alumno", id: "NomComp", field: "NomComp", minWidth: 250, maxWidth: 350, resizable: true });
        colum.push({ name: "Email 1", id: "Email", field: "Email", minWidth: 150, maxWidth: 250, resizable: true });
        colum.push({ name: "Email 2", id: "Correo2", field: "Correo2", minWidth: 150, maxWidth: 120, resizable: true });
        colum.push({ name: "Telefono 1", id: "Tel1", field: "Tel1", minWidth: 110, maxWidth: 130, resizable: true });
        colum.push({ name: "Telefono 2", id: "Tel2", field: "Tel2", minWidth: 110, maxWidth: 130, resizable: true });
        colum.push({ name: "Estado", id: "Estado", field: "Estado", minWidth: 150, maxWidth: 200, resizable: true });
        colum.push({ name: "Cent. Trab.", id: "CentTrab", field: "CentTrab", minWidth: 180, maxWidth: 350, resizable: true });
        colum.push({ name: "Nivel CT", id: "NivelCT", field: "NivelCT", minWidth: 200, maxWidth: 350, resizable: true });
        colum.push({ name: "Calificación Obt.", id: "CalifObt", field: "CalifObt", minWidth: 110, maxWidth: 110, resizable: true });
        colum.push({ name: "Diplomado", id: "Diplomado", field: "Diplomado", minWidth: 350, maxWidth: 650, resizable: true });
        colum.push({ name: "F. Apertura", id: "FechaApertura", field: "FechaApertura", minWidth: 120, maxWidth: 120, resizable: true });
        colum.push({ name: "Total Insignias", id: "tTotal", field: "tTotal", minWidth: 80, maxWidth: 100, resizable: true });
        colum.push({ name: "Bronce", id: "Bronce", field: "Bronce", minWidth: 100, maxWidth: 150, resizable: true });
        colum.push({ name: "Plata", id: "Plata", field: "Plata", minWidth: 100, maxWidth: 150, resizable: true });
        colum.push({ name: "Oro", id: "Oro", field: "Oro", minWidth: 100, maxWidth: 150, resizable: true });
        colum.push({ name: "Platino", id: "Platino", field: "Platino", minWidth: 100, maxWidth: 150, resizable: true });
        colum.push({ name: "Birrete Oro", id: "Birrete", field: "Birrete", minWidth: 100, maxWidth: 150, resizable: true });
        colum.push({ name: "Mouse Oro", id: "Mouse", field: "Mouse", minWidth: 100, maxWidth: 150, resizable: true });

        sgrid = new Slick.Grid("#div_sgrid", data_json, colum, option);

          
          sgrid.onBeforeEditCell.subscribe(function(e, args) {
            var item = sgrid.getDataItem(args.row);
            if (typeof item === "undefined") return true;

              if(args.cell == 1) return false;
              if(args.cell == 2  && !error) return false;
          });
          
          
          sgrid.onCellChange.subscribe(function(e, args) {
              var item = args.item; 
              var data = sgrid.getData();
            
              sgrid.setData(data);
              sgrid.render();

              var numFila = parseInt(args.row);
              var numCelda = parseInt(args.cell);

          }); 
          
          sgrid.onActiveCellChanged.subscribe(function(e, args) {
              var item = args.item;
              var data = sgrid.getData();
              
              if(sgrid.getColumns()[args.cell]) {
                  var nombreColumna = sgrid.getColumns()[args.cell].field;
                  //Codigo a ejecutar
              } 


          });

          AjustarTamaño();
          
    });  //
   
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

</script>

</asp:Content>
