<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminEstadoProcesal_EtapaProcesal.aspx.vb" Inherits="coactivosyp.AdminRelacion_EstadoProcesal_EtapaProcesal" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Etapa procesal</title>
    <link href="../../css/main.css" rel="stylesheet" />
    <link href="../../css/list.css" rel="stylesheet" />
    <link href="css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('submit,input:button,input:submit').button();
            $(".PCG-Content tr:gt(0)").mouseover(function () {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function () {
                $(this).removeClass("ui-state-highlight");
            });
        });
    </script>
</head>
<body class="internal">
    <form id="form1" runat="server">
        <div class="col">
        <asp:Label ID="LabelTitulo" runat="server" Text="Captura de datos de la Relacion Estado Proceso-Estado Procesal" CssClass="ui-widget-header"></asp:Label>
    </div>
    <asp:Panel ID="pnlSearchEstadoProceso" runat="server" CssClass="form-row search-list-form" ClientIDMode="Static" DefaultButton="cmdSearch">

        <div class="col">
            <asp:Label ID="lblSelectProceso" runat="server" Text="Estado Proceso: " CssClass="ui-widget-header"></asp:Label>
            <asp:DropDownList ID="ddlEstados" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0"> -</asp:ListItem>
            </asp:DropDownList>
             <asp:Button ID="cmdSearch" runat="server" Text="Guardar" CssClass="PCGButton button"></asp:Button>
        </div>
      
    </asp:Panel>
         <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
        <asp:GridView ID="gvwMatriz" runat="server"  CssClass="PCG-Content list tbl-indentation" AutoGenerateColumns="False" DataKeyNames="ID_ETAPA_PROCESAL">
                        <Columns>
                <asp:templatefield HeaderText="Seleccione" >
                    <itemtemplate>
                        <asp:checkbox ID="chkSelect"   Enabled="true"
                        runat="server"></asp:checkbox>
                    </itemtemplate>
                </asp:templatefield>
                           <asp:BoundField DataField="ID_ETAPA_PROCESAL" HeaderText="Código">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
            </asp:BoundField>
            <asp:BoundField DataField="codigo" HeaderText="">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
            </asp:BoundField>
            <asp:BoundField DataField="VAL_ETAPA_PROCESAL" HeaderText="Etapa Procesal"></asp:BoundField>

            </Columns>
        </asp:GridView>
     </form>
</body>
</html>

