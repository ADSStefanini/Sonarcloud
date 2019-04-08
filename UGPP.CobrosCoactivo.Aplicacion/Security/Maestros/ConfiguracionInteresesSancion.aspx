<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfiguracionInteresesSancion.aspx.vb"
    Inherits="coactivosyp.Security.Maestros.ConfiguracionInteresesSancion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>CONFIGURACION DE INTERESES SANCION </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            EndRequestHandler();
        });
        function EndRequestHandler() {
            //                $('#cmdAddNew').button();
            //                $('#cmdSearch').button();
            $('.GridEditButton').button();
            $("#tabs").tabs();

        };   
    </script>

    <style type="text/css">
        *
        {
            font-size: 11px;
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
        }
        body
        {
            background-color: #01557C;
        }
        .style1
        {
            width: 168px;
        }
        .style2
        {
            width: 104px;
        }
        .style3
        {
            border: 1px solid #4297d7;
            background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
            color: #ffffff;
            font-weight: bold;
            width: 84px;
        }
    </style>
    
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td colspan="9" background="images/resultados_busca.jpg" height="42">
                <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left">
                    <span style="font-weight: normal">Usuario Actual (
                        <asp:Label ID="lblNomPerfil" runat="server" Text="Label" />)</span></div>
            </td>
        </tr>
    </table>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Configurar Interes Multa 1438</a></li>
            <li><a href="#tabs-2">Generar Proyección Masiva Multas 1438</a></li>
        </ul>
        <div id="tabs-1">
           <table>
                <%-- <tr>
                     <td align="left">
                                            <table style="width: 100%">
                                               <tr>
                            <td class="style3">
                            Fecha inicial</td>
                            <td class="style1" >
                            <asp:TextBox ID="txtSearchdesde" runat="server" ></asp:TextBox>
                            </td>
                            <td class="style2">
                            <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                            </td>
                            <td>
                           <%-- <asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
                            </td>                            
                                                    
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>--%>
                <tr>
                    <td>
                        <p style="color: Blue; font-size: 10px; text-transform: uppercase; font-weight: bold">
                            Ventana para configurar porcentaje de interes multas 1438</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content"
                            AllowPaging="True" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="CODIGO">
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                                </asp:BoundField>
                                <asp:BoundField DataField="p_anual" HeaderText="TASA ANUAL"></asp:BoundField>
                                <asp:BoundField DataField="p_mensual" HeaderText="TASA MENSUAL"></asp:BoundField>
                                <asp:ButtonField ButtonType="Button" Text="EDITAR">
                                    <ControlStyle CssClass="GridEditButton" />
                                </asp:ButtonField>
                            </Columns>
                            <HeaderStyle CssClass="ui-widget-header" />
                            <RowStyle CssClass="ui-widget-content" />
                            <AlternatingRowStyle />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-2">
            <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                <iframe src="capturarinteresesmulta1438Masivo.aspx" width="960" height="740" scrolling="no"
                    frameborder="0"></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
