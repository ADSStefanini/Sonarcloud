<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="subirVerificacionPago.aspx.vb" Inherits="coactivosyp.Security.Maestros.subirVerificacionPago" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//ES" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Subir archivo de verificación de pago</title>
        
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <script type="text/javascript">
        $(function() {                
            //Botones de importar            
            $('#cmdImportarcsv').button();            
        });
           
    </script>
    
    <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
		    .numeros { text-align:right; }
		    body{ background-color:#01557C}
        .style1
        {
            width: 203px;
        }
        .style2
        {
            width: 93px;
        }
        </style>
        
            <script type="text/javascript" language="javascript">
                function mostrar_procesar() {
                    document.getElementById('procesando_div').style.display = "";
                    $("#dialog-modal").dialog({
                        height: 150,
                        modal: true
                    });
                    setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
                }
    </script>    
    
   </head>
<body>
    <form id="form1" runat="server">
           <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                <td colspan="5" background="images/resultados_busca.jpg" height="42">
                    <div style="color:White; font-weight:bold; width:450px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
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
            <td>
            </td>
            </tr>
                <tr >
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                    </td>
                    </tr>
                    
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2" >
                        <asp:FileUpload ID="upload" runat="server" cssClass="PCGButton"  />
                        <asp:Button id="cmdImportarcsv" runat="server" Text="Importar archivo de verificación de pagos" OnClientClick="mostrar_procesar();"></asp:Button>                    
                    
                    </td>
                    <td class="style1" >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                                    
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td >                        
                        &nbsp;             
                    </td>
                    <td class="style2" >
                        &nbsp;</td>
                    <td class="style1" >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                                    
                    </table>
                               <div id="dialog-modal" title="Procesando..." style="text-align:center">   
                    <span id= "procesando_div" style="display:none;" >
                        <img src="../images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
                    </span>
        </div>

    </form>
</body>
</html>
