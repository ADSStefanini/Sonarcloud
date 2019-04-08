<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="info_persuasivo.aspx.vb" Inherits="coactivosyp.info_persuasivo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head>
        <title>PERSUASIVO</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                EndRequestHandler();
            });
            function EndRequestHandler() {
                $('#btnprocesar').button();
                $('#cmdSearch').button();

                $('#txtToFecOfi1').datepicker({
                    
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                $('#txtToFecOfi2').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy',
                    firstDay: 1,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
           
        </script>
        <style type="text/css">
            .style1
            {
                width: 125px;
            }
            .style2
            {
                width: 175px;
            }
            .style3
            {
                width: 85px;
            }
            * { font-size:12px; font-family:Arial;}	
        </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="90%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td align="right">
                                    <table style="width: 100%">
                                        <tr>
                            <td class="style1">
                                                <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton" 
                                                    Width="77px"></asp:Button>
                                            </td>
                            <td class="style2">
                                                <asp:Button id="btnprocesar" runat="server" Text="Procesar" cssClass="PCGButton"  Width="77px"></asp:Button>
                                            </td>
                       
                                    </tr>
                                        <tr>
                            <td class="style1">
                            Nro Expediente</td>
                            <td class="style2">
                            <asp:TextBox ID="txtSearchNroExp" runat="server" Width="124px" ></asp:TextBox></td>
                       
                                    </tr>
                                        <tr>
                        <td class="style1">
                                                        Fecha Oficio 1
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="txtToFecOfi1" runat="server" Width="124px"></asp:TextBox>&nbsp;
                        </td>
                                            
                                    </tr>
                                        <tr>
                    <td class="style1">
                        Fecha Oficio 2 </td>
                    <td class="style2">
                        <asp:TextBox ID="txtToFecOfi2" runat="server" Width="124px"></asp:TextBox>&nbsp;
                    </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                    </tr>
                                        <tr>
                    <td class="style1">
                        TipoTipo de informe</td>
                    <td class="style2">
                        <asp:DropDownList ID="ddlinfo" runat="server">
                        </asp:DropDownList>
                    </td>
                                                <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" 
                                        CssClass="PCG-Content" AllowPaging="True"  
                                        Width="941px">
                                        <Columns>
                                            <asp:BoundField DataField="NroExp" HeaderText="Nro Exp" >
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NroOfi1" HeaderText="Nro Ofi1">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FecOfi1" HeaderText="Fec Ofi1" 
                                                DataFormatString="{0:d}">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FecEnvOfi1" HeaderText="Fec Env Ofi1" 
                                                DataFormatString="{0:d}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NoGuiaEnt1" HeaderText="No Guia Ent1">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NroOfi2" HeaderText="Nro Ofi2">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FecOfi2" HeaderText="Fec Ofi2" 
                                                DataFormatString="{0:d}">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FecEnvOfi2" HeaderText="Fec Env Ofi2" 
                                                DataFormatString="{0:d}">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NoGuiaEnt2" HeaderText="No Guia Ent2">
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
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
