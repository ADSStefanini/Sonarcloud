<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfiguracionInteresesParafiscales.aspx.vb" Inherits="coactivosyp.ConfiguracionInteresesParafiscales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head>
        <title>CALCULO_INTERESES_PARAFISCALES</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

        <link href="PCGStyleSheet.css" rel="stylesheet" type="text/css" />
        <link id="ThemesCSS" runat="server" type="text/css" rel="stylesheet" />

        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>

        <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.0/jquery-ui.js" type="text/javascript"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>

        <script type="text/javascript">
            $(function() {
                EndRequestHandler();
            });
            function EndRequestHandler() {
                $('#cmdAddNew').button();
                $('#cmdSearch').button();
                $('.GridEditButton').button();

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
        </script>
        <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
		    body{ background-color:#01557C}
        </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="90%" border="0" class="ui-widget-content ui-widget">
                 <tr>
                <td colspan="9" background="images/resultados_busca.jpg" height="42">
                    <div style="color:White; font-weight:bold; width:450px; height:20px; float:left"><span style="font-weight:normal">Usuario Actual ( <asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                    <div style="color:White; width:150px; height:20px; float:right; text-align:right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span id="spancerrarsesion" runat="server">Cerrar sesión&nbsp&nbsp</span>
                    </div>
                    
                    <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" />
                        </asp:LinkButton>
                    </div>
                    
                </td>
            </tr>
                <tr>
                    <td align="right">
                                            <table style="width: 100%">
                                                <tr>
                            <td class="ui-widget-header">
                            desde</td>
                            <td>
                            <asp:TextBox ID="txtSearchdesde" runat="server" ></asp:TextBox></td>
                                                    <td>&nbsp;</td><td>&nbsp;</td>
                                                    <td>
                                                        <asp:DropDownList ID="cboTheme" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Text="base" Value="base"></asp:ListItem>
                                                            <asp:ListItem Text="black-tie" Value="black-tie"></asp:ListItem>
                                                            <asp:ListItem Text="blitzer" Value="blitzer"></asp:ListItem>
                                                            <asp:ListItem Text="cupertino" Value="cupertino"></asp:ListItem>
                                                            <asp:ListItem Text="dark-hive" Value="dark-hive"></asp:ListItem>
                                                            <asp:ListItem Text="dot-luv" Value="dot-luv"></asp:ListItem>
                                                            <asp:ListItem Text="eggplant" Value="eggplant"></asp:ListItem>
                                                            <asp:ListItem Text="excite-bike" Value="excite-bike"></asp:ListItem>
                                                            <asp:ListItem Text="flick" Value="flick"></asp:ListItem>
                                                            <asp:ListItem Text="hot-sneaks" Value="hot-sneaks"></asp:ListItem>
                                                            <asp:ListItem Text="humanity" Value="humanity"></asp:ListItem>
                                                            <asp:ListItem Text="le-frog" Value="le-frog"></asp:ListItem>
                                                            <asp:ListItem Text="mint-choc" Value="mint-choc"></asp:ListItem>
                                                            <asp:ListItem Text="overcast" Value="overcast"></asp:ListItem>
                                                            <asp:ListItem Text="pepper-grinder" Value="pepper-grinder"></asp:ListItem>
                                                            <asp:ListItem Text="redmond" Value="redmond"></asp:ListItem>
                                                            <asp:ListItem Text="smoothness" Value="smoothness"></asp:ListItem>
                                                            <asp:ListItem Text="south-street" Value="south-street"></asp:ListItem>
                                                            <asp:ListItem Text="start" Value="start"></asp:ListItem>
                                                            <asp:ListItem Text="sunny" Value="sunny"></asp:ListItem>
                                                            <asp:ListItem Text="swanky-purse" Value="swanky-purse"></asp:ListItem>
                                                            <asp:ListItem Text="trontastic" Value="trontastic"></asp:ListItem>
                                                            <asp:ListItem Text="ui-darkness" Value="ui-darkness"></asp:ListItem>
                                                            <asp:ListItem Text="ui-lightness" Value="ui-lightness"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button id="cmdSearch" runat="server" Text="Search" cssClass="PCGButton"></asp:Button>
                                                        <asp:Button id="cmdAddNew" runat="server" Text="AddNew" cssClass="PCGButton"></asp:Button>
                                                    </td>
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowPaging="true" AllowSorting="true">
                                                <Columns>
                                                    <asp:BoundField DataField="consec" >
                                                        <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                        <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="p_trimestral" HeaderText="p_trimestral" SortExpression="CALCULO_INTERESES_PARAFISCALES.p_trimestral"></asp:BoundField>
                                                    <asp:BoundField DataField="desde" HeaderText="desde" SortExpression="CALCULO_INTERESES_PARAFISCALES.desde"></asp:BoundField>
                                                    <asp:BoundField DataField="hasta" HeaderText="hasta" SortExpression="CALCULO_INTERESES_PARAFISCALES.hasta"></asp:BoundField>
                                                    <asp:BoundField DataField="t_diaria" HeaderText="t_diaria" SortExpression="CALCULO_INTERESES_PARAFISCALES.t_diaria"></asp:BoundField>
                                                    <asp:ButtonField ButtonType="Button" Text="Edit">
                                                        <ControlStyle CssClass="GridEditButton" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <HeaderStyle CssClass="ui-widget-header"  />
                                                <RowStyle CssClass="ui-widget-content" />
                                                <AlternatingRowStyle/>
                                            </asp:GridView>   
                                        </td>
                                    </tr>
                                </table>
                            </form>
                        </body>
                    </html>
