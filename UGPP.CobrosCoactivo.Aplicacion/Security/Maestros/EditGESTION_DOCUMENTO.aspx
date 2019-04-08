<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditGESTION_DOCUMENTO.aspx.vb" Inherits="coactivosyp.EditGESTION_DOCUMENTO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Editar Diccionario</title>
    <link href="../../css/main.css" rel="stylesheet" />
    <link href="../../css/list.css" rel="stylesheet" />
    <link href="../../css/form.css" rel="stylesheet" />
    <link href="css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <style type="text/css">
        .form-control-sm {
        }
    </style>
</head>
<body class="internal">
    <form id="form1" runat="server">
        <div class="form-content">
            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td colspan="10" background="images/resultados_busca.jpg" height="42">
                    <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                    <div style="color: White; width: 150px; height: 20px; float: right; text-align: right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span>Cerrar sesión&nbsp&nbsp</span>
                    </div>

                    <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" /></asp:LinkButton>
                    </div>

                </td>
            </tr>
        </table>
            <div class="form-row search-list-form"">
                <div class="form-group row">
                    <span class="ui-widget-header">Documento T&Iacute;tulo</span>
                    <asp:DropDownList ID="ddlDTitulo" runat="server" DataTextField="NOMBRE_DOCUMENTO" DataValueField="ID_DOCUMENTO_TITULO">
                    </asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="-1" ID="Req_ID2" Display="Dynamic" 
    ValidationGroup="SaveValidation" runat="server" ControlToValidate="ddlDTitulo"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group row">
                    <span class="ui-widget-header">Tipo T&Iacute;tulo</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" DataTextField="nombre" DataValueField="codigo">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator InitialValue="-1" ID="Req_ID" Display="Dynamic" 
    ValidationGroup="SaveValidation" runat="server" ControlToValidate="ddlTipo"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <span class="ui-widget-header">Estado</span>
                    <asp:CheckBox ID="chkActivo" runat="server" />
                </div>
                    <div>
                    <span class="ui-widget-header">Obligatorio</span>
                    <asp:CheckBox ID="chkObli" runat="server" />
                </div>
                <div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" ValidationGroup="SaveValidation" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
