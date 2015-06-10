<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="webControl._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="margin-top: 10em;" >
        <div id="div_sgrid" style="width:600px; height:500px;" ></div>
    </div>



<script type="text/javascript">

    var compsMap = {};
    var data_json = [];
    var colum = [];

    var option = {
        enableCellNavigation: true,
        enableColumnReorder: false
      };
    
    
    $(function() { 
        if ($('[id$=vsmReparacionListado]').text().trim().length > 0) { $('#errores').show(); }

        data_json = <%=str_dt_json%>
        
        colum.push({ name: "Alumno", id: "Alumno", field: "Alumno", minWidth: 200, maxWidth: 350, resizable: true });
        colum.push({ name: "Nombre", id: "fullname", field: "fullname", minWidth:200, maxWidth: 450, resizable: true });
        colum.push({ name: "Curso", id: "courseid", field: "courseid", minWidth: 100, maxWidth: 150, cannotTriggerInsert: true, resizable: true });
    
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

          
    });  //
   
    
    


    


    
    function UltimoRenglon(){
             var ultimo = 0;
              $.each(sgrid.getData(), function(idx, item) {                    
                   if (parseInt(item.renglonorden) > ultimo){
	                  ultimo = item.renglonorden;
                   }
              });
              return parseInt(ultimo) + 1;
    }

</script>

</asp:Content>
