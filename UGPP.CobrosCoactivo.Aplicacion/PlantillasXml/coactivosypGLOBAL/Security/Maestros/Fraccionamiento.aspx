<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Fraccionamiento.aspx.vb" Inherits="coactivosyp.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Página sin título</title>
            <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        <script type ="text/jscript">
            $(function() {
            $('.GridEditButton').button();
            $('#cmdCancel').button();
            $('#cmd_fraccionamiento').button();
            $('#cmd_adicionar').button();
            });
                       
        
        </script>
<style type="text/css">
	        * { font-size:12px; font-family:Arial}				 
	    .style2
    {
        border: 1px solid #a6c9e2;
        background: #fcfdfd url('css/redmond/images/ui-bg_inset-hard_100_fcfdfd_1x100.png') repeat-x 50% bottom;
        color: #222222;
        width: 706px;
        height: 230px;
    }
	    .style3
    {
        width: 1000px;
    }
	    </style>
</head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditTDJ" class="style2">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. de Título Judicial <span lang="es">&nbsp;Principal</span></td>
                    <td class="style3">
                        <asp:TextBox id="txtNroTituloPrincipal" runat="server" MaxLength="20" 
                            CssClass="ui-widget" Width="137px" Enabled="False" ReadOnly="True"></asp:TextBox>
                        <span lang="es">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Valor del Título <span lang="es">Principal</span></td>
                    <td class="style3">
                        <asp:TextBox id="txtValorTDJPrincipal" runat="server" CssClass="ui-widget" 
                            Width="137px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" colspan="2">
                        GESTIÓN DEL TÍTULO DE DEPÓSITO JUDICIAL
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        <span lang="es">Valor Liquidacion </span>
                    </td>
                    <td class="style3">
                        <asp:TextBox id="txt_Liquidacion" runat="server" MaxLength="20" 
                            CssClass="ui-widget" ReadOnly="True" Width="138px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="ui-widget-header">
                        <span lang="es">Fraccionado </span>
                    </td>
                    <td class="style3">
                        <asp:GridView ID="grid_fraccionamiento" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" Height="137px" 
                            Width="567px">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:BoundField DataField="Nro Deposito" HeaderText="Nro Deposito" />
                                <asp:BoundField HeaderText="Nro Titulo" DataField="nro titulo" />
                                <asp:BoundField HeaderText="Valor Titulo" DataField="valor titulo" />
                                <asp:BoundField HeaderText="Estado" DataField="estado" />
                                <asp:BoundField HeaderText="Resolucion" DataField="resolucion" />
                                <asp:BoundField HeaderText="Fecha de resolucion" DataFormatString ="{0:dd/MM/yyyy}" 
                                    DataField="fecha de resolucion" />
                                <asp:ButtonField ButtonType="Button" Text="Aplicar" CommandName    =  "Aplicar">  <ControlStyle CssClass="GridEditButton"/>  </asp:ButtonField>
                                <asp:ButtonField ButtonType="Button" Text="Devolver" CommandName   =  "Devolver">  <ControlStyle CssClass="GridEditButton"/> </asp:ButtonField>
                                <asp:ButtonField ButtonType="Button" Text="Editar" CommandName = "Editar" > <ControlStyle CssClass="GridEditButton"/> </asp:ButtonField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
                 <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <%--<asp:Button id="cmdDelete" runat="server" Text="Delete" cssClass="PCGButton"></asp:Button>--%>
                    </td>
                    <td class="style3">
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmd_fraccionamiento" runat="server" Text="Fraccionar" 
                            cssClass="PCGButton" Visible="False"></asp:Button>
                        <span lang="es">&nbsp;
                        <asp:Button ID="cmd_adicionar" runat="server" Text="Adicionar" />
                        </span>
                    </td>
                </tr>
            </table>
        </form>
    <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
  </div>
    </body>

</html>
