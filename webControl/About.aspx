<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.vb" Inherits="webControl.About" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    

    <h2><%: Title %>.</h2>
    <p>Your app description page.</p>
    <p>Use this area to provide additional information.</p>
    <br />
    <asp:Button ID="btnTest" runat="server" Text="Test MySql" />
    <br />
    <asp:TextBox ID="txtTest" runat="server"></asp:TextBox>
    <br />

<script type="text/javascript">

    var data_insumos = [];

    $(function () {

    });  //***FIN***
   
    data_insumos = <%=str_test_json%>

    $('[id$=txtTest]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../Ajax/ajax_test.ashx",
                dataType: "json",
                data: { termino: request.term },
                success: function (data) {
                    response($.map(data, function (item) {

                        var nombre = new String(item.Alumno);
                        return {
                            label: nombre,
                            id: item.courseid,
                            componente: item
                        };
                    }));
                }
            });
        },
        delay: 10,
        minLength: 2,
        select: function (event, ui) {
            if (ui.item.id == -1) {
                $('[id$=txtTest]').val("");
                return false;
            }
            else {
               
            }
        }
    });

</script>

</asp:Content>
