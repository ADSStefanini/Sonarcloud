<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ESTADOS_PERSONA.aspx.vb" Inherits="coactivosyp.ESTADOS_PERSONA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>ESTADOS_PERSONA</title>
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
                $('#cmdAddNew').click(function() {
                    document.getElementById('ID').value = '';
                    $("#txtDialogcodigo").val('');
                    $("#txtDialognombre").val('');
                    $('#dialog').dialog('open');
                });
                $('#cmdSearch').button();
                $('.GridEditButton').button();

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
                $("#dialog").dialog({
                    bgiframe: true,
                    resizable: false, // true is currently buggy across browsers(but toleratable)
                    width: screen.width * 0.4,
                    //height: 100,
                    modal: true,
                    autoOpen: false,
                    buttons: {
                        'Guardar': function() {
                        $("#txtcodigo").val($("#txtDialogcodigo").val());
                        $("#txtnombre").val($("#txtDialognombre").val());
                        $(this).dialog('close');
                        $("#cmdSave").click();
                    },
                    Cancelar: function() {
                        $(this).dialog('close');
                    }, 
                    'Borrar': function() {
                        $(this).dialog('close');
                        $('#cmdDelete').click();
                    }
                }
            });
        }
        function body_onLoad() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }
        function GridClick( s )
        {
            document.getElementById('ID').value = s;
            $('#cmdLoad').click();
            $('#dialog').dialog('open');
        }
    </script>
    
    <style type="text/css">
        * { font-size:12px; font-family:Arial;}	
        .BoundFieldItemStyleHidden { display:none; }
        .BoundFieldHeaderStyleHidden { display:none; }
        .hidden { display:none; }
    </style>
    
</head>
<body onload="body_onLoad()">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td align="right">
            <input type="button" name="cmdAddNew" value="Adicionar" id="cmdAddNew"/>
                                    <table style="width: 100%">
                                        <tr>
                        <td class="ui-widget-header">
                        Código</td>
                        <td>
                        <asp:TextBox ID="txtSearchcodigo" runat="server" ></asp:TextBox></td>
                    <td class="ui-widget-header">
                    Nombre</td>
                    <td>
                    <asp:TextBox ID="txtSearchnombre" runat="server" ></asp:TextBox></td>
                                            <td>                                                
                                                <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                                
                                            </td>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowPaging="true" AllowSorting="true">
                                        <Columns>
                                            <asp:BoundField DataField="codigo" >
                                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                            </asp:BoundField>
                                            <asp:BoundField DataField="codigo" HeaderText="Código" SortExpression="ESTADOS_PERSONA.codigo"></asp:BoundField>
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="ESTADOS_PERSONA.nombre"></asp:BoundField>
                                            <asp:BoundField HeaderText="Editar" />
                                        </Columns>
                                        <HeaderStyle CssClass="ui-widget-header"  />
                                        <RowStyle CssClass="ui-widget-content" />
                                        <AlternatingRowStyle/>
                                    </asp:GridView>   
                                </td>
                            </tr>
                        </table>
                        <div class="hidden">
                            <div id="dialog" title="Actualizar">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                    <ContentTemplate>
                                        <table id="tblEditDialogESTADOS_PERSONA" class="ui-widget-content">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="ui-widget-header">
                                                    Código
                                                </td>
                                                <td>
                                                    <asp:TextBox id="txtDialogcodigo" runat="server" CssClass="ui-widget"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="ui-widget-header">
                                                    Nombre
                                                </td>
                                                <td>
                                                    <asp:TextBox id="txtDialognombre" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmdLoad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <asp:Button ID="cmdLoad" runat="server" />
                            <asp:Button ID="cmdSave" runat="server" />
                            <asp:Button ID="cmdDelete" runat="server" />
                            <asp:TextBox ID="ID" runat="server" CssClass="hidden"></asp:TextBox>
                        </div>
                        <div class="hidden">
                            <asp:TextBox id="txtcodigo" runat="server"></asp:TextBox>
                            <asp:TextBox id="txtnombre" runat="server"></asp:TextBox>
                        </div>
                    </form>
                </body>
            </html>

