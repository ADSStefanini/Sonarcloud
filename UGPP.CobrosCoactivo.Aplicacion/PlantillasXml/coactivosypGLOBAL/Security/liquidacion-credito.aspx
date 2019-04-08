<%@ Page CodeBehind="liquidacion-credito.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.liquidacion_credito" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Math"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Liquidación de Credito</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="coactivo.css" type="text/css">
	</HEAD>
	<body>
		<div id="wrap">
			<%	
				'1. Capturar la placa pasada como parámetro por la URL									
				Dim pRefeCata As String
				Me.Parameter = Request.QueryString
				If Me.Parameter.Keys.Count > 0 Then
					pRefeCata = Me.Parameter.Item(0).ToString() ' Valor del Parametro
				End If
				
				'2. Traer los datos de conexion al servidor
				Dim NomServidor, Usuario, Clave, BaseDatos, cmd, userDE As String
				NomServidor = ConfigurationManager.AppSettings("ServerName")
				Usuario = ConfigurationManager.AppSettings("BD_User")
				Clave = ConfigurationManager.AppSettings("BD_pass")
				BaseDatos = ConfigurationManager.AppSettings("BD_name")			
				
				'3. Conectarse a la base de datos
				Dim connString As String				
				connString = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
							 ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave
				
				'Objeto de conexion
				Dim objConnection As System.Data.SqlClient.SqlConnection				
				objConnection = New System.Data.SqlClient.SqlConnection
				objConnection.ConnectionString = connString
				
				objConnection.Open()   'Abre la conexion
				
				'Comando SQL
				Dim strSQL As String				
				strSQL = "SELECT pr.id_deudor, pr.numproceso, pr.fechamanda, pr.valormanda, pr.nomdeudor, mp.direccion, mp.dir_establ, mp.id_propiet, " & _
						"vg.vigencia, vg.cdg, vg.avluo_ctstral, vg.valor, vg.cd, vg.cd2, vg.avluo_ctstral * (vg.valor/1000) AS capital, "  & _
						"vg.interes, vg.avluo_ctstral* (vg.valor/1000)+vg.interes AS totalvigencia FROM procesos pr, maepre mp, vigencias vg WHERE " & _
						"pr.id_deudor = mp.refe_cata AND pr.id_deudor = vg.refe_cata AND pr.id_deudor = '" & pRefeCata & "' ORDER BY vigencia"
				
				'Crea el objeto Command
				Dim objCommand As sqlCommand
				objCommand = New sqlCommand(strSQL, objConnection)
				
				'Llena el datareader
				Dim objDataReader As sqlDataReader
				objDataReader = objCommand.ExecuteReader()
				
				Dim lcEstadoGrupo As String 																																														
				If objDataReader.Read() Then																
					'lcEstadoGrupo = Trim(objDataReader("estadogrupo"))
				End If															
																			
				'Variable que va a sumar el valor de las vigencias
				Dim TotalGral As Decimal		
				TotalGral = 0
			%>
						
			<!--cabecera -->
			<div id="header">				
				<h1 id="logo-text">REPUBLICA DE COLOMBIA</h1>
				<img src="images/escudo-bmanga-min.jpg" class="logorafa">
				<p id="slogan">Tesorería Municipal</p>
			</div>
			<div id="titulo-reporte">
				RESOLUCION No.<% Response.Write(Trim(objDataReader("numproceso"))) %>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					<!-- Bucaramanga, septiembre 30 de 2002-->
				</div>
				<hr>
			</div>
			
			<div id="datos-cabecera">
				<!--campo-->
				<div class="etiqueta-cabecera">
					Contribuyente
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("nomdeudor"))) %> 
				</div>
				<div class="etiqueta-cabecera">
					Identificación
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("id_propiet"))) %> 
				</div>
				<div class="etiqueta-cabecera">
					Dirección
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("direccion"))) %>
					  <% Response.Write(Trim(objDataReader("dir_establ"))) %>					
				</div>
				<div class="etiqueta-cabecera">
					Ref. Catastral
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("id_deudor"))) %>
				</div>				
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					"Por medio de la cual se declara la existencia de una obligación a cargo del predio con referencia 
					catastral No. <b><% Response.Write(Trim(objDataReader("id_deudor"))) %></b> 
					, ubicado en 
					<% Response.Write(Trim(objDataReader("direccion"))) %>
					<% Response.Write(Trim(objDataReader("dir_establ"))) %>
					". <br><br>
					La suscrita Secretaria de Hacienda Distrital, en uso de las facultades legales, en especial de las conferidas 
					por los artículos 18 y 413 del Acuerdo 30 del 31 de diciembre de 2001.					
				</div>
				<div id="titulo-reporte" style="padding-top:10px;">
					CONSIDERANDO
				</div>
				<div id="contenido-texto">
					Que conforme al certificado expedido por la profesional especializada, coordinadora del Impuesto Predial, 
					el cual reposa en el  expediente, el predio antes identificado, adeuda al Distrito de Cartagena la suma que 
					más adelante se detalla, más los intereses de mora y las sanciones a que haya lugar:
					<br>
				</div>
				<div id="datos-cabecera" style="height:22px;">					
					<div class="etiqueta-detalle" style="margin-left:60px; width:34px;">
						Vigencia
					</div>	
					<div class="etiqueta-detalle">
						Concepto
					</div>
					<div class="etiqueta-detalle" style="width:119px;">
						Base Gravable
					</div>
					<div class="etiqueta-detalle" style="width:42px;">
						Tarifa
					</div>
					<div class="etiqueta-detalle" style="width:48px;">
						Estrato
					</div>
					<div class="etiqueta-detalle" style="width:52px;">
						Destino
					</div>
					<div class="etiqueta-detalle" style="width:120px;">
						Capital
					</div>
					<div class="etiqueta-detalle" style="width:102px;">
						Intereses
					</div>
					<div class="etiqueta-detalle" style="width:125px;">
						Total
					</div>
				</div>
				
				<!-- Aca va el ciclo del detalle de las vigencias -->
				<!-- contenido del detalle -->				
					
					<% 
					'TotalGral = TotalGral + objDataReader("totalvigencia")
					Do 
					%>
						
						<div id="datos-detalle-pack">
						
						<div class="etiqueta-detalle" style="margin-left:60px; width:78px; text-align:left">
							<% Response.Write(FormatNumber(objDataReader("vigencia"),0)) %>
						</div>	
						<div class="etiqueta-detalle" style="width:44px;">
							<% Response.Write(Trim(objDataReader("cdg"))) %>
						</div>
						<div class="etiqueta-detalle" style="width:120px;">
							<% Response.Write(FormatNumber(objDataReader("avluo_ctstral"),0)) %>
						</div>
						<div class="etiqueta-detalle" style="width:42px;">
							<% Response.Write(Trim(objDataReader("valor"))) %>
						</div>
						<div class="etiqueta-detalle" style="width:48px;">
							<% Response.Write(Trim(objDataReader("cd"))) %>
						</div>
						<div class="etiqueta-detalle" style="width:52px;">
							<% Response.Write(FormatNumber(objDataReader("cd2"),0)) %>
						</div>
						<div class="etiqueta-detalle" style="width:120px;">
							<% Response.Write(FormatNumber(objDataReader("capital"),0)) %>
						</div>
						<div class="etiqueta-detalle" style="width:102px;">
							<% Response.Write(FormatNumber(objDataReader("interes"),0)) %>
						</div>
						<div class="etiqueta-detalle" style="width:124px;">
							<% Response.Write(FormatNumber(objDataReader("totalvigencia"),0)) %>
						</div>
						
						</div>
						
					<%
						TotalGral = TotalGral + objDataReader("totalvigencia")
						
						Loop While objDataReader.Read() 
					%>
				
				
				<%
				'dim textoprueba as string 
				'textoprueba = 0
				objDataReader.Close()
				objDataReader = objCommand.ExecuteReader()																																														
				objDataReader.Read()
				%>
				<div id="contenido-texto">
					Todo por cuantía de $ 
					<% 	
						Response.Write(FormatNumber(TotalGral,0))											
						Dim clase As FuncionesTE = new FuncionesTE
						'Response.Write(clase.Monto(100))
						Dim ValorEnLetras As String
						ValorEnLetras = ""
						Dim TotalEntero As Long
						
						TotalEntero = Round(TotalGral)
						ValorEnLetras = clase.Num2Text(TotalEntero) & " PESOS CON 00/100"
						'Response.Write(clase.Monto(100))
						Response.Write(" <b>")
						Response.Write(ValorEnLetras) 
						Response.Write("</b>")
					%>					
					Que esta liquidación tiene como fundamento de derecho, el Artículo 18 del Acuerdo 30 del 31 de diciembre
					de 2001 y demás normas concordantes.
					<br /><br />
					En mérito de lo expuesto:
				</div>
				<div id="titulo-reporte" style="padding-top:10px;">
					RESUELVE
				</div>
				<div id="contenido-texto">
					<label style="font-weight:bold;">PRIMERO:</label>
					Declárese a cargo del propietario o poseedor del predio identificado con la referencia
					<b><% Response.Write(Trim(objDataReader("id_deudor"))) %></b> 
					la existencia de la obligación de pagar a favor del Municipio la suma de 
					<% Response.Write(FormatNumber(TotalEntero,0)) %> , más los intereses de mora y las sanciones a que haya lugar, por las razones expuestas 
					en la parte considerativa de la presente resolución.
					<br />
					<br />
					
					<label style="font-weight:bold;">SEGUNDO:</label>
					Contra la presente resolución procede el recurso de reposición, el cual podrá 
					interponerse ante la autoridad que la profiere, dentro de los cinco (5) días siguientes a su notificación.				
					<br />
					<br />
					
					<label style="font-weight:bold;">TERCERO:</label>
					Una vez ejecutoriada la presente resolución, comuníquesele a la Tesorería Municipal para lo de su competencia. 
					<br />
					<br />
					
								
					<br />
					<br />
				</div>
				
				
				<div id="firma">
					<img src="images/firma2.jpg" class="logofirma">
					<div id="firma-texto">
						<span>
							<b>NOTIFIQUESE Y CUMPLASE</b></span>
						<br>
						<br>
						__________________________
						<br>
						Tesorero Municipal<br>						
					</div>
					<div id="firma-mecanica">						
						De Conformidad con el Decreto 2150 del 1995, la firma mecánica que antecede 
						tiene plena validez para todos los efectos legales.
					</div>
					<!--
					<div id="cuadro-notificacion">
						<span>
							<b>NOTIFIQUESE Y CUMPLASE</b></span>
						<br>
						<br>
						__________________________
						<br>
						Tesorero Municipal<br>						
					</div>
					-->
				</div>								
			</div>
		</div>
	</body>
</HTML>
