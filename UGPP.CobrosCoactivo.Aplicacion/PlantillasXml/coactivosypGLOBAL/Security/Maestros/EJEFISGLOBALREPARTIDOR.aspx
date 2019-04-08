<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EJEFISGLOBALREPARTIDOR.aspx.vb" Inherits="coactivosyp.EJEFISGLOBALREPARTIDOR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>Creación y asignación de expedientes</title>
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
               
                $('#cmdAddNew').button();
                $('#cmdSearch').button();
                $('#cmdFirst').button();
                $('#cmdNext').button();
                $('#cmdLast').button();
                $('#cmdPrevious').button();
                $('#btnExportarGrid').button();
                
                $('.GridEditButton').button();

                $('#lnkNumExpVencer').click(function() {
                    window.open('EstadisticaxVencer.aspx', 'Estadistica de expedientes por vencer', 'width=600,height=250');
                    return false;
                });

                $('#lnkNumExpVencidos').click(function() {
                    window.open('EstadisticaVencidos.aspx', 'Estadistica de expedientes vencidos', 'width=600,height=250');
                    return false;
                });

                $('#lnkMsjNoLeidos').click(function() {
                    window.open('MENSAJES.aspx', 'Visor de mensajes', 'width=780,height=450');
                    return false;
                });

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
            //
            $(function() {
                $("#alnkFiltros").click(function(event) {
                    //event.preventDefault();
                    //$("#divFiltros").slideToggle();
                    //alert($("#alnkFiltros").text());
                    if ($("#alnkFiltros").text() == 'Expandir opciones') {
                        //alert("expandir");
                        $("#alnkFiltros").text('Contraer opciones');
                        $("#divFiltros").slideToggle();
                    } else {
                        //alert("contraer");
                        $("#alnkFiltros").text('Expandir opciones');
                        $("#divFiltros").slideUp();
                    }
                });
                
            });
        </script>
                
        <style type="text/css">
		    body{ background-color:#01557C}
		    * { font-size:12px; font-family:Arial;}
		    td { padding:2px;}
		    .BoundFieldItemStyleHidden { display: none;}
		    .BoundFieldHeaderStyleHidden {display: none;}
		    #divFiltros { display: BLOCK; }
	    </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td colspan="10" background="images/resultados_busca.jpg" height="42">
                        <div style="color:White; font-weight:bold; width:460px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                        <div style="color:White; width:140px; height:20px; float:right; text-align:right">
                            <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                            <span>Cerrar sesión&nbsp&nbsp&nbsp</span>
                        </div>
                        
                        <%--<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                            <asp:LinkButton ID="AInformes" runat="server" ToolTip="Informes">
                                <img alt ="Informes"  src="../images/icons/informes16x16.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Informes" />
                            </asp:LinkButton>
                        </div>--%>
                        
                        <div style="color:White; width:50px; height:20px; float:right; text-align:right; padding-right:0px;">
                            <asp:LinkButton ID="ACambio" runat="server" ToolTip="Cambios de estado">
                                <img alt ="Cambio de estado"  src="../images/icons/cambioestado.png" height="18" width="18" style=" vertical-align:middle" id="img2" title="Cambios de estado" />
                            </asp:LinkButton>
                            <span><%  Response.Write("(" & Session("ssNumSolicitudesCE") & ")")%>&nbsp&nbsp</span>
                        </div>
                        
                        <div style="color:White; width:50px; height:20px; float:right; text-align:right; padding-right:0px;">
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Mensajes">
                                <img alt ="Mensajes"  src="../images/icons/comentarios.png" height="18" width="18" style=" vertical-align:middle" id="img3" title="Mensajes" />
                            </asp:LinkButton>
                            <span><%  Response.Write("(" & Session("ssNumMsgNoLeidos") & ")")%>&nbsp&nbsp</span>
                        </div>
                        
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <a href="#" id="alnkFiltros">Expandir opciones</a>
                            <div id="divFiltros">
                                <table style="width: 100%">
                                    <%--<tr>
                                        <td colspan="5">
                                            <asp:LinkButton ID="ASolCambiosEstado" runat="server" ToolTip="Informes">
                                                <img alt ="Informes"  src="../images/icons/cambioestado.png" height="16" width="16" style=" vertical-align:middle" id="img2" title="Informes" />
                                            </asp:LinkButton>
                                            <span>Solicitudes de cambio de estado</span>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="ui-widget-header">
                                            No. expediente cobranzas
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchEFINROEXP" runat="server" ></asp:TextBox>
                                        </td>
                                        <td class="ui-widget-header">
                                            No. Memorando
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchEFINUMMEMO" runat="server" ></asp:TextBox>
                                        </td>
                                        <td>                                                
                                            <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            C.C. / NIT deudor
                                        </td>
                                        <td>                                            
                                            <asp:TextBox ID="txtSearchEFINIT" runat="server" Columns="20" ></asp:TextBox>
                                        </td>
                                        <td class="ui-widget-header">
                                           Nombre
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchED_NOMBRE" runat="server" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            Estado
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboSearchEFIESTADO" runat="server" AppendDataBoundItems="true">
                                                <asp:ListItem Text="" Value="">
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ui-widget-header">
                                            Gestor / Abogado
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboSearchEFIUSUASIG" runat="server" AppendDataBoundItems="true"><asp:ListItem Text="" Value=""></asp:ListItem></asp:DropDownList>
                                        </td>
                                        <td>
                                                <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            Alerta / Término
                                        </td>
                                        <td>                                            
                                            <asp:TextBox ID="txtTermino" runat="server" Columns="20" ></asp:TextBox>
                                        </td>
                                        <td>
                                            Paginación
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="ui-widget" id="cboNumExp" runat="server" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp
                                        </td>
                                    </tr>
		        </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" 
                                CssClass="PCG-Content" AllowSorting="True">
                                <Columns>
                                    <asp:BoundField DataField="EFINROEXP" >
                                        <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                        <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EFINROEXP" HeaderText="No. expediente" SortExpression="EJEFISGLOBAL.EFINROEXP"></asp:BoundField>
                                    <asp:BoundField DataField="EFIFECHAEXP" HeaderText="Fecha recep. título" SortExpression="EJEFISGLOBAL.EFIFECHAEXP" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="EFINUMMEMO" HeaderText="No. Memorando" SortExpression="EJEFISGLOBAL.EFINUMMEMO"></asp:BoundField>
                                    <asp:BoundField DataField="EFIEXPORIGEN" HeaderText="No. Expediente origen" SortExpression="EJEFISGLOBAL.EFIEXPORIGEN"></asp:BoundField>
                                    <asp:BoundField DataField="EFIFECCAD" HeaderText="Fecha entrega CAD" SortExpression="EJEFISGLOBAL.EFIFECCAD" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="ED_Codigo_Nit" HeaderText="Id. Deudor"></asp:BoundField>
                                    <asp:BoundField DataField="ENTES_DEUDORESEFINITED_Nombre" HeaderText="Nombre Deudor" SortExpression="ENTES_DEUDORESEFINIT.ED_Nombre"></asp:BoundField>
                                    <asp:BoundField DataField="USUARIOSEFIUSUASIGnombre" HeaderText="Gestor / Abogado" SortExpression="USUARIOSEFIUSUASIG.nombre"></asp:BoundField>
                                    <asp:BoundField DataField="ESTADOS_PROCESOEFIESTADOnombre" HeaderText="Estado" SortExpression="ESTADOS_PROCESOEFIESTADO.nombre"></asp:BoundField>
                                    <asp:BoundField DataField="EFIFECENTGES" HeaderText="Fec entrega Gestor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                    
                                    <%-- <asp:BoundField DataField="termino" HeaderText="Término" /> --%>
                                    
                                    <asp:TemplateField HeaderText="Término">
                                         <ItemTemplate>
                                             <asp:Label ID="Label1" runat="server" Text='<%# Bind("termino") %>' ToolTip ='<%# Bind("explicacion") %>'></asp:Label>
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:ImageField DataImageUrlField="PictureURL"></asp:ImageField>
                                    
                                    <asp:ButtonField ButtonType="Button" Text="Editar">
                                        <ControlStyle CssClass="GridEditButton" />
                                    </asp:ButtonField>
                                </Columns>
                                <HeaderStyle CssClass="ui-widget-header"  />
                                <RowStyle CssClass="ui-widget-content" />
                                <AlternatingRowStyle/>
                            </asp:GridView>   
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button id="cmdFirst" runat="server" Text="Primero" cssClass="PCGButton"></asp:Button>
                            <asp:Button id="cmdPrevious" runat="server" Text="Anterior" cssClass="PCGButton"></asp:Button>
                            <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
                            <asp:Button id="cmdNext" runat="server" Text="Siguiente" cssClass="PCGButton"></asp:Button>
                            <asp:Button id="cmdLast" runat="server" Text="Ultimo" cssClass="PCGButton"></asp:Button>
                        </td>
                    </tr>
                </table>
                
        </form>
    </body>
</html>