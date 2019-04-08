<%@ Page Language="vb" AutoEventWireup="false" Codebehind="subirexpedientes2.aspx.vb" Inherits="coactivosyp.subirexpedientes2" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Actualización de expedientes</title>
		<script src="event.js" type="text/javascript"></script>
		<script type="text/javascript" src="datepickercontrol.js"></script>
		<link type="text/css" rel="stylesheet" href="datepickercontrol.css" />
		<link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
		<script type="text/javascript" src="jquery-1.4.2.min.js"></script>
		
		<script type="text/javascript">
	        jQuery(document).ready(function($){
		        $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		        });
	        });
        </script>
         
        <style type="text/css">
            #message_box 
            {
            position: absolute;
            top:200px; left: 0;
            z-index: 10;
            background-color:#094194;
            
            background-image:url(imagenes/MenuflotaIzfondo.png);
            padding:0px;margin:0px 0px 0px;
            border:1px solid #0E0F10;
            -moz-box-shadow: 5px 5px 10px #000; /* Firefox */
            -webkit-box-shadow: 5px 5px 10px #000; /* Safari y Chrome */
            box-shadow: 5px 5px 10px #000;
            }
            
            #message_box ul {padding:0px;margin:0px;}
            #message_box li {list-style-type:none;margin:0px;padding:0px;}
            #message_box li a {margin:0px;padding:0px;text-decoration:none;display: block;}
            #message_box li a img {border: none;}
        </style>
		
	</HEAD>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" onload="document.forms.Form1.txtEnte.focus()"
		marginheight="0" marginwidth="0">
		
		<!-- Definicion del menu -->  
        <div id="message_box">
            <ul>
             <li style="height:36px;width:36px;">
                <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
             </li>
             <li style="height:152px;width:36px;">
                <a href="menu.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
             </li>
            </ul>
         </div>
		
		<script type="text/javascript">
		//<![CDATA[	

		IncrementalSearch = function(input, callback, className){
			var i, $ = this;
			($.input = input).autocomplete = "off", $.callback = callback || function(){},
			$.className = className || "", $.hide(), $.visible = 0;
			for(i in {keydown: 0, focus: 0, blur: 0, keyup: 0, keypress: 0})
				addEvent(input, i, $._handler, $);
		};
		with({p: IncrementalSearch.prototype}){
			p.show = function(){
				for(var $ = this, s = $.c.style, o = $.input, x = o.offsetLeft,
					y = o.offsetTop + o.offsetHeight; o = o.offsetParent; x += o.offsetLeft, y += o.offsetTop);
				s.left = x + "px", s.top = y + "px",
				$.l.length ? (s.display = "block", !$.visible && ($._callEvent("onshow"), ++$.visible), $.highlite(0)) : s.display = "none";
			};
			p.hide = function(){
				var $ = this, d = document, s = ($.c && $.c.parentNode.removeChild($.c),
				$.c = d.body.appendChild(d.createElement("div"))).style;
				$.l = [], $.i = -1, $.c.className = $.className, s.position = "absolute", s.display = "none";
				$._old = null, $.visible && ($._callEvent("onhide"), --$.visible);
			};
			p.add = function(s, x, data){
				var $ = this, l = 0, d = document, i = $.l.length, v = $.input.value.length,
					o = ($.l[i] = [s, data, $.c.appendChild(d.createElement("div"))])[2];
				if(x instanceof Array || (x = [x]), o.i = i, o.className = "normal", !isNaN(x[0]))
					for(var j = -1, k = x.length; ++j < k; o.appendChild(d.createTextNode(
						s.substring(l, x[j]))).parentNode.appendChild(d.createElement(
						"span")).appendChild(d.createTextNode(s.substring(x[j],
						l = x[j] + v))).parentNode.className = "highlited");
				for(x in o.appendChild(d.createTextNode(s.substr(l))), {click: 0, mouseover: 0})
					addEvent(o, x, $._handler, $);
			};
			p.highlite = function(i){
				var $ = this;
				$._invalid(i) || ($._invalid($.i) || ($.l[$.i][2].className = "normal"),
				$.l[$.i = i][2].className += " selected", $._callEvent("onhighlite", $.l[i][0], $.l[i][1]));
			};
			p.select = function(i){
				var $ = this;
				$._invalid(i = isNaN(i) ? $.i : i) || ($._callEvent("onselect",
					$.input.value = $.l[$.i][0], $.l[i][1]), $.hide());
			};
			p.next = function(){
				var $ = ($ = this, $.highlite(($.i + 1) % $.l.length));
			};
			p.previous = function(){
				var $ = ($ = this, $.highlite((!$.i ? $.l.length : $.i) - 1));
			};
			p._fadeOut = function(){
				var f = (f = function(){arguments.callee.x.hide();}, f.x = this, setTimeout(f, 200));
			};
			p._handler = function(e){
				var $ = this, t = e.type, k = e.key;
				t == "focus" || t == "keyup" ? k != 40 && k != 38 && k != 13 && $._old != $.input.value && ($.hide(), $.callback($, $.input.value))
				: t == "keydown" ? k == 40 ? $.next() : k == 38 ? $.previous() : $._old = $.input.value
				: t == "keypress" ? k == 13 && (e.preventDefault(), $.select())
				: t == "blur" ? $._fadeOut() : t == "click" ? $.select()
				: $.highlite((/span/i.test((e = e.target).tagName) ? e.parentNode : e).i);
			};
			p._invalid = function(i){
				return isNaN(i) || i < 0 || i >= this.l.length;
			}
			p._callEvent = function(e){
				var $ = this;
				return $[e] instanceof Function ? $[e].apply($, [].slice.call(arguments, 1)) : undefined;
			};
		}
		//]]>
		</script>
		<style type="text/css">.autocomplete { BORDER-RIGHT: #999 1px solid; BORDER-TOP: medium none; BACKGROUND: #eee; BORDER-LEFT: #999 1px solid; CURSOR: pointer; BORDER-BOTTOM: #999 1px solid }
	.autocomplete .normal { BORDER-TOP: #999 1px solid }
	.autocomplete .selected { BACKGROUND: #ddf }
	.autocomplete .highlited { FONT-WEIGHT: bold; COLOR: #008 }
	</style>
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td width="50%"></td>
					<td background="images/bg_izdo.jpg"><IMG src="images/bg_izdo.jpg" width="32"></td>
					<td vAlign="top" width="780" bgColor="#618ce4" height="100%">
						<!-- Tabla del centro del diseño -->
						<table height="100%" cellSpacing="0" cellPadding="0" width="780" border="0">
							<!-- segunda fila de la tabla central tiene una sola celda (resultados_busca.jpg)-->
							<tr>
								<td width="780" background="images/resultados_busca.jpg" height="42"><font style="FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #ffffff; FONT-FAMILY: verdana">&nbsp; 
										Subida de expedientes al servidor y registro de los archivos en la base de 
										datos </font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td vAlign="middle" align="center" width="780">
									<DIV ms_positioning="GridLayout">
										<TABLE height="564" cellSpacing="0" cellPadding="0" width="747" border="0" ms_2d_layout="TRUE">
											<TR vAlign="top">
												<TD width="1" height="8"></TD>
												<TD width="31"></TD>
												<TD width="80"></TD>
												<TD width="56"></TD>
												<TD width="40"></TD>
												<TD width="56"></TD>
												<TD width="136"></TD>
												<TD width="347"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="32"></TD>
												<TD colSpan="3">
													<asp:HyperLink id="HyperLink4" runat="server" Font-Size="X-Small" Font-Names="Verdana" ForeColor="White"
														NavigateUrl="consultarentes.aspx">Consultar expedientes</asp:HyperLink></TD>
												<TD colSpan="2">
													<asp:HyperLink id="HyperLink5" runat="server" Font-Size="X-Small" Font-Names="Verdana" ForeColor="White"
														NavigateUrl="subirexpedientes.aspx">Actualizar expedientes</asp:HyperLink></TD>
												<TD>
													<asp:HyperLink id="HyperLink6" runat="server" Font-Size="X-Small" Font-Names="Verdana" ForeColor="White"
														NavigateUrl="consultardocumentos2.aspx">Consulta diaria</asp:HyperLink></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="4" height="3"></TD>
												<TD colSpan="4" rowSpan="2"><INPUT id="txtEnte" type="text" size="74" name="txtEnte" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD colSpan="2"><asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Deudor</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="4" height="3"></TD>
												<TD colSpan="3" rowSpan="2"><INPUT id="txtFechaRad" name="txtFechaRad" datepicker="true" datepicker_format="DD/MM/YYYY"
														readonly type="text" size="14" runat="server"></TD>
												<TD rowSpan="3"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD colSpan="2"><asp:label id="Label13" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de radicación</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="32"></TD>
												<TD colSpan="5"><asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Por favor subir los archivos de una sola actuación</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD height="32"></TD>
												<TD colSpan="7"><asp:customvalidator id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="X-Small"
														ErrorMessage="CustomValidator" Width="680px" Font-Bold="True"></asp:customvalidator></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen1" type="file" size="60" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label3" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 1</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen2" type="file" size="60" name="File1" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label4" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 2</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen3" type="file" size="60" name="File2" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label5" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 3</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen4" type="file" size="60" name="File3" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label6" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 4</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen5" type="file" size="60" name="File4" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label7" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 5</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen6" type="file" size="60" name="File5" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label8" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 6</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen7" type="file" size="60" name="File6" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label9" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 7</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen8" type="file" size="60" name="File7" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label10" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 8</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen9" type="file" size="60" name="File8" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="29"></TD>
												<TD><asp:label id="Label11" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 9</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="3"></TD>
												<TD colSpan="5" rowSpan="2"><INPUT id="imagen10" type="file" size="60" name="File9" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="37"></TD>
												<TD><asp:label id="Label12" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Imagen 10</asp:label></TD>
											</TR>
											<TR Align="center">
												<TD colSpan="8">
												  <asp:button id="Button2" runat="server" Width="113px" 
                                                        Text="Aceptar"></asp:button>&nbsp;
                                                  <br /><br /><br />
                                                  <p style="color:White;font-family:Verdana;font-size:11px;padding-top:3px;border-top:2px solid #fff"><b>Nota :</b> Puede utilizar otro método para subir expedientes pulsando <a href="subirexpedientes2.aspx"> aquí.</a></p>      
                                                  <br /><br /><br />
                                                </TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<!-- fin de la tabla central --></table>
					</td>
					<td background="images/bg_dcho.jpg"><IMG src="images/bg_dcho.jpg" width="32"></td>
					<td width="50%"></td>
				</tr>
			</table>
		</form>
		<%	
				Dim NomServidor, Usuario, Clave, BaseDatos, cmd, userDE As String
				NomServidor = ConfigurationManager.AppSettings("ServerName")
				Usuario = ConfigurationManager.AppSettings("BD_User")
				Clave = ConfigurationManager.AppSettings("BD_pass")
				BaseDatos = ConfigurationManager.AppSettings("BD_name")
				
				Dim X As Integer
				'Cadena de conexion a SQL Server
				Dim connString as String			
				'connString = "Provider=sqloledb;Data Source=portatilrafa;Initial Catalog=tareas;User Id=usertareas;Password=123456;"
				connString ="workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
                             ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave
                             
				'Objeto de conexion
				Dim objConnection as SqlConnection
				objConnection = New SqlConnection(connString)
				
				'-------------------------------Llenar la lista de entidades-------------------------------'
				objConnection.Open()   'Abre la conexion
				
				'Comando SQL
				Dim strSQL as String
				strSQL = "SELECT codigo_nit, nombre FROM entesdbf WHERE cobrador = '" & Session("mcobrador") & "'"
				
				'Crea el objeto Command
				Dim objCommand as SqlCommand
				objCommand = New SqlCommand(strSQL, objConnection)
				
				'Llena el datareader
				Dim objDataReader as SqlDataReader 
				objDataReader = objCommand.ExecuteReader()
							
				'Escribir comienzo de script javascript
				Response.Write("<script type=""text/javascript"">")
				
				Response.Write("var list = [")
				
				X = 0
				Do while objDataReader.Read()
					If X > 0 Then
						Response.write(",")
					End If
					Response.write("'" & objDataReader("nombre").trim() & "::" & objDataReader("codigo_nit").trim() &  "'")
					X = X + 1
				Loop
				
				'Escribir fin de script javascript
				Response.write("].sort();")
				Response.write("</script>")			
				
				objDataReader.close()
				objConnection.Close()							
				'------------------------------- Final la lista de entes-------------------------------'
			%>
		<script type="text/javascript">
			//<![CDATA[

			//-- Busca múltiples ocurrencias ----
			function getNames(o, search){
				if(search = search.toLowerCase())
					for(var i = -1, l = list.length; ++i < l;){
						/*Busca todas las ocurrencias de "search" y adiciona los índices en un array					*/
						for(var j = 0, indices = []; j = list[i].toLowerCase().indexOf(search, j) + 1;
							indices[indices.length] = j - 1);
						/*si alguna ocurrencia fue encontrada, adiciona un item y pasa la posicion de la misma*/
						if(indices.length)
							o.add(list[i], indices);
					}
				o.show();
			}
						
			new IncrementalSearch(document.forms[0].txtEnte, getNames, "autocomplete");

			//]]>
		</script>
	</body>
</HTML>
