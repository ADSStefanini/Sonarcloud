<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EstadisticaVencidos.aspx.vb" Inherits="coactivosyp.EstadisticaVencidos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Estadística expedientes vencidos</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />  

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    
    <style type="text/css">
        body{ background-color:#01557C}
        * { font-size:12px}
        td { padding:2px;}
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">            
            <tr>
                <td colspan="10" background="images/resultados_busca.jpg" height="42">
                    <div style="color:White; font-weight:bold">&nbsp Estadística de procesos</div>
                </td>
            </tr>
            <tr>
                <td align="right">
                                
                            </td>
                        </tr>
                        <tr>
                            <td>                                
                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" 
                                    CssClass="PCG-Content" AllowSorting="True">
                                    <Columns>
                                        <asp:BoundField DataField="EfiNroExp" HeaderText="Expediente" ></asp:BoundField>
                                        <asp:BoundField DataField="Termino" HeaderText="Término"></asp:BoundField>
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
