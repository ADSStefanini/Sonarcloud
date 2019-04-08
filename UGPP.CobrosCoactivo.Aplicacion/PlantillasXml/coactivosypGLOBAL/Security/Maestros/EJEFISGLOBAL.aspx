<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EJEFISGLOBAL.aspx.vb" Inherits="coactivosyp.EJEFISGLOBAL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>EJEFISGLOBAL</title>
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
                $('#cmdMostrarVencer').button();
                $('#cmdMostrarVencidos').button();
                $('.GridEditButton').button();
                $('#cmdInformesGestion').button();
                $('#btnExportarGrid').button();
                $('#cmdMostrarEstadisticas').button();

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

                $('#cmdInformesGestion').click(function() {
                    window.open('info_persuasivo.aspx', 'Estadistica de expedientes vencidos', 'width=400,height=250');
                    return false;
                });
                
                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });                
                
                /////               
                $("#txtSearchFECTITULO").keypress(function(event) { event.preventDefault(); });            
                $("#txtSearchFECTITULO").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });                        
                $('#txtSearchFECTITULO').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: new Date,
                    minDate: new Date(2007, 6, 12),
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                /////
                $("#txtSearchEFIFECENTGES").keypress(function(event) { event.preventDefault(); });
                $("#txtSearchEFIFECENTGES").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtSearchEFIFECENTGES').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: new Date,
                    minDate: new Date(2007, 6, 12),
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            }
            
            $(function() {
                var perfil = <%  Response.Write(Session("mnivelacces") ) %> ;
                //alert(perfil);
                if(perfil > 3) {
                    $("#lnkInformes").css("display", "none");
                }
            });
            
        </script>
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
        <style type="text/css">
		    body{ background-color:#01557C}
		    * { font-size:12px; font-family:Arial; }
		    th { padding-left:8px; padding-right:8px;}
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden { display:none; }
	    </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
        <asp:ToolkitScriptManager ID="tsm" runat="server"></asp:ToolkitScriptManager>
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td colspan="10" background="images/resultados_busca.jpg" height="42">
                        <div style="color:White; font-weight:bold; width:500px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                        
                        <div style="color:White; width:580px; height:20px; float:right; text-align:right">    
                            
                            
                            <!-- Solicitudes de cambio de estado para revisores y supervisores -->
                            <div id="divCambioEstado" style="color:White; width:60px; height:20px; float:left; text-align:right; " runat="server">
                                <asp:LinkButton ID="ACambio" runat="server" ToolTip="Cambios de estado">
                                    <img alt ="Cambio de estado"  src="../images/icons/cambioestado.png" height="18" width="18" style=" vertical-align:middle" id="img2" title="Cambios de estado" />
                                </asp:LinkButton>
                                <span><%  Response.Write("(" & Session("ssNumSolicitudesCE") & ")")%>&nbsp&nbsp</span>
                            </div>
                        
                                                
                            <!-- Mensajes -->
                            <div style="color:White; width:60px; height:20px; float:left; text-align:right; margin-left:20px; ">
                                <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Mensajes">
                                    <img alt ="Mensajes"  src="../images/icons/comentarios.png" height="18" width="18" style=" vertical-align:middle" id="img3" title="Mensajes" />
                                </asp:LinkButton>
                                <span><%  Response.Write("(" & Session("ssNumMsgNoLeidos") & ")")%>&nbsp&nbsp</span>
                            </div>
                            
                            <!-- Informes -->
                            <div style="color:White; width:60px; height:20px; float:left; text-align:right; ">
                                <asp:LinkButton ID="lnkInformes" runat="server" ToolTip="Informes" OnClientClick="mostrar_procesar();" ><img alt ="" src="../images/icons/informes16x16.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>
                            
                            <!-- Consultar pagos -->
                            <div style="color:White; width:60px; height:20px; float:left; text-align:right; ">
                                <asp:LinkButton ID="lnkConsultarPagos" runat="server" ToolTip="Consultar pagos"><img alt ="" src="../images/icons/plata.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>
                            
                            <!-- Capturar intereses -->
                            <div style="color:White; width:60px; height:20px; float:left; text-align:right; ">
                                <asp:LinkButton ID="lnkInteres" runat="server" ToolTip="Capturar intereses"><img alt ="" src="../images/icons/intereses.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>
                            
                            <!-- Intereses de multas -->
                            <div style="color:White; width:50px; height:20px; float:left; text-align:right">
                                <asp:LinkButton ID="lnkInterMultas" runat="server" 
                                    ToolTip="Capturar intereses de multas"><img alt ="" src="../images/icons/intermultas.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>
                            
                            <!-- Subir SQL -->                          
                            <div style="color:White; width:50px; height:20px; float:left; text-align:right">
                                <asp:LinkButton ID="lnkSql" runat="server" ToolTip="Subir sql"><img alt ="" src="../images/icons/sql.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>                                                        
                            
                            <!-- Cerrar sesion -->
                            <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                            <span>Cerrar sesión&nbsp</span>
                        </div>                        
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table style="width: 100%">
                            <tr>
                                <td class="ui-widget-header">
                                    No. expediente cobranzas
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchEFINROEXP" runat="server" ></asp:TextBox>
                                </td>
                                <td class="ui-widget-header">
                                    Nombre deudor
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchED_NOMBRE" runat="server" ></asp:TextBox>
                                </td>
                        <td>
                            <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                        </td>
                                            <td>                                                
                                                Paginación
                                            </td>
                                            <td>                                                
                                                <asp:DropDownList CssClass="ui-widget" id="cboNumExp" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </td>
                                            <td>                                                
                                                &nbsp;
                                            </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            NIT / C.C.
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchEFINIT" runat="server" ></asp:TextBox>
                                        </td>
                                        <td class="ui-widget-header">Estado actual</td>
                                        <td>
                                            <asp:DropDownList CssClass="ui-widget" id="cboEFIESTADO" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button id="cmdInformesGestion" runat="server" Text="Informes" cssClass="PCGButton"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" />
                                        </td>
                                        <td>
                                            &nbsp;<asp:Button ID="cmdMostrarEstadisticas" runat="server" Text="Estadisticas" />
                                        </td>
                                        <td>
                                            &nbsp;<asp:Button ID="cmdMasivo" runat="server" Text="Masivos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            Gestor
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cboSearchEFIUSUASIG" runat="server" AppendDataBoundItems="true"><asp:ListItem Text="" Value=""></asp:ListItem></asp:DropDownList>
                                        </td>
                                        <td class="ui-widget-header">Alerta / Término </td>
                                        <td>                                            
                                            <asp:TextBox ID="txtTermino" runat="server" Columns="20" ></asp:TextBox>
                                        </td>
                                        <td class="ui-widget-header">
                                            Tipo de título 
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="ui-widget" id="cboMT_tipo_titulo" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ui-widget-header">
                                            Fecha recepción Título
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchFECTITULO" runat="server" style="width:90px;"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnBorraFechaRT" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de recepción del título" />
                                        </td>
                                        <td class="ui-widget-header">
                                            Fec entrega gestor
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchEFIFECENTGES" runat="server" style="width:90px;"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnBorraFechaEG" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de entrega al gestor" />
                                        </td>
                                    </tr>
                                </table>
                             </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True">
                                        <Columns>
                                            <asp:BoundField DataField="EFINROEXP" >
                                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EFINROEXP" HeaderText="Expediente" SortExpression="EJEFISGLOBAL.EFINROEXP"></asp:BoundField>
                                            <asp:BoundField DataField="EFIFECHAEXP" HeaderText="Fecha recepción título" SortExpression="EJEFISGLOBAL.EFIFECHAEXP" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="ED_NOMBRE" HeaderText="Nombre del deudor" SortExpression="ENTES_DEUDORES.ED_NOMBRE"></asp:BoundField>
                                            <asp:BoundField DataField="EFINIT" HeaderText="NIT / CC" SortExpression="EJEFISGLOBAL.EFINIT"></asp:BoundField>
                                            <asp:BoundField DataField="NomTipoTitulo" HeaderText="Tipo de Título" SortExpression="TITULOSEJECUTIVOS.NomTipoTitulo">
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="EFIFECCAD" HeaderText="Fecha entrega al CAD" SortExpression="EJEFISGLOBAL.EFIFECCAD" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>--%>
                                            <asp:BoundField DataField="EFIESTADO" HeaderText="Estado actual" />                                            
                                            <asp:BoundField DataField="EFIVALDEU" HeaderText="Valor Deuda" DataFormatString="{0:N0}" >
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="EFIPAGOSCAP" HeaderText="Pagos capital" DataFormatString="{0:N0}" >
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="EFISALDOCAP" HeaderText="Saldo capital actual" DataFormatString="{0:N0}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EFIESTUP" HeaderText="Estado actual último pago" />
                                            <asp:BoundField DataField="GESTOR" HeaderText="Gestor"></asp:BoundField>
                                            <asp:BoundField DataField="EFIFECENTGES" HeaderText="Fec entrega Gestor" SortExpression="EJEFISGLOBAL.EFIFECENTGES" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            
                                            <%--<asp:BoundField DataField="termino" HeaderText="Término" />--%>
                                            
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
                        <div id="Totales" style=" background-color:White; color:Red; margin-top:20px; margin-top: 0px; padding-top: 0px;">
                            <table id="tblTotales" class="ui-widget-content">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Total Deuda
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtTotalDeuda" runat="server" MaxLength="20" CssClass="ui-widget" style=" text-align:right; " ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Total pagos capital
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtTotalPagos" runat="server" MaxLength="20" CssClass="ui-widget" style=" text-align:right; " ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="ui-widget-header">
                                        Total saldo capital actual
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtSaldoCapital" runat="server" MaxLength="20" CssClass="ui-widget" style=" text-align:right; " ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>    
                              </table>
                        </div>                
        
        
        <asp:Panel ID="pnlError" runat="server" style="width: 524px; position:static; display: none; margin-top:0px; margin-left:30px">
              
              <div style="margin: 0  0 5px 0; ">
                 <% 
                     If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                         Response.Write(ViewState("Erroruseractivo"))
                     End If
                 %>
              </div>
    		  <hr />	
		    				    
			  <asp:Button style="width: 100px; margin-left:150px; margin-top:-40px" id="btnNoerror"
				    runat="server" Text="Iniciar sesión" Height="23px"></asp:Button>    
        </asp:Panel>
		
		<asp:Button ID="Button3" runat="server" Text="Button" style="visibility:hidden" />
		
        <asp:ModalPopupExtender ID="ModalPopupError" runat="server" 
            TargetControlID="Button3"
            PopupControlID="pnlError"
            CancelControlID="btnNoerror"
            DropShadow="false" 
            >
        </asp:ModalPopupExtender>
        
           <script type="text/javascript">
               function mpeSeleccionOnCancel() {
                   var pagina = '../../Login.aspx'
                   location.href = pagina
                   return false;
               }
        </script>
        
        </form>
        <div id="dialog-modal" title="Por favor espere..." style="text-align:center">   
                    <span id= "procesando_div" style="display:none;" >
                        <img src="../images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
                    </span>
        </div>    
    </body>
</html>