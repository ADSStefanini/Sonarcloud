<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page CodeBehind="resolucion-embargo-bienes.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resolucion_embargo_bienes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Resolución de embargo de bienes</title>
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
				RESOLUCION DE EMBARGO DE BIENES <% Response.Write(Trim(objDataReader("numproceso"))) %>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Bucaramanga, Octubre 17 de 2002
				</div>
				<hr>
			</div>
			<div id="contenido" style="height:94px;">
				<div id="contenido-texto">
					El suscrito Tesorero del Distrito de Cartagena de Indias D.T y C. en ejercicio  de la competencia delegada por la Secretaría de Hacienda
					Distrital, en armonía con lo dispuesto en el Estatuto Tributario Distrital, en el Estatuto Tributario Nacional, en el  artículo 91  literal d) 
					numeral 6 de la Ley 136 de 1994  y  en artículo 66 de la Ley  383  de 1997 y,
				</div>
				<div id="titulo-reporte" style="PADDING-TOP:10px">
					CONSIDERANDO
				</div>
			</div>			
			<div id="contenido">
				<div id="contenido-texto">
					1. Que contra el  contribuyente <b><% Response.Write(Trim(objDataReader("nomdeudor"))) %></b>
					identificado con C.C (NIT) <b><% Response.Write(Trim(objDataReader("id_propiet"))) %></b> 
					se  inició  proceso  de  cobro  por  las  obligaciones contenidas en la  resolución No.
					<b><% Response.Write(Trim(objDataReader("numproceso"))) %></b> de 
					de fecha 26 de abril del año  2002  por  medio  de  la  cual  se  declara la existencia  
					de  una  obligación  de  impuesto  predial  unificado  a cargo de un 
					predio, correspondiente  al (los) siguiente(s) periodos(s):					
				</div>
				<div id="datos-cabecera" style="HEIGHT:22px">
					<div class="etiqueta-detalle" style="MARGIN-LEFT:60px; WIDTH:34px">
						Vigencia
					</div>
					<div class="etiqueta-detalle">
						Concepto
					</div>
					<div class="etiqueta-detalle" style="WIDTH:119px">
						Base Gravable
					</div>
					<div class="etiqueta-detalle" style="WIDTH:42px">
						Tarifa
					</div>
					<div class="etiqueta-detalle" style="WIDTH:48px">
						Estrato
					</div>
					<div class="etiqueta-detalle" style="WIDTH:52px">
						Destino
					</div>
					<div class="etiqueta-detalle" style="WIDTH:120px">
						Capital
					</div>
					<div class="etiqueta-detalle" style="WIDTH:102px">
						Intereses
					</div>
					<div class="etiqueta-detalle" style="WIDTH:125px">
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
					<div class="etiqueta-detalle" style="MARGIN-LEFT:60px; WIDTH:78px; TEXT-ALIGN:left">
						<% Response.Write(FormatNumber(objDataReader("vigencia"),0)) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:44px">
						<% Response.Write(Trim(objDataReader("cdg"))) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:120px">
						<% Response.Write(FormatNumber(objDataReader("avluo_ctstral"),0)) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:42px">
						<% Response.Write(Trim(objDataReader("valor"))) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:48px">
						<% Response.Write(Trim(objDataReader("cd"))) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:52px">
						<% Response.Write(FormatNumber(objDataReader("cd2"),0)) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:120px">
						<% Response.Write(FormatNumber(objDataReader("capital"),0)) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:102px">
						<% Response.Write(FormatNumber(objDataReader("interes"),0)) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:124px">
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
						Response.Write(". <br><br>")
						'Dim clase As FuncionesTE = new FuncionesTE
						'Response.Write(clase.Monto(100))
						'Dim ValorEnLetras As String
						'ValorEnLetras = ""
						'Dim TotalEntero As Long
						
						'TotalEntero = Round(TotalGral)
						'ValorEnLetras = clase.Num2Text(TotalEntero) & " PESOS CON 00/100"
						'Response.Write(clase.Monto(100))
						'Response.Write(" <b>")
						'Response.Write(ValorEnLetras) 
						'Response.Write("</b>")
						
					%>
					La  cual presta mérito ejecutivo al tenor del artículo 828 del Estatuto Tributario Nacional y 
					demás normas armónicas y afines del Estatuto Tributario Distrital.
					<br>
					<br>
					2. Que no se obtuvo  el pago de las obligaciones, por lo que, de acuerdo con  el articulo  837 
					del Estatuto Tributario Nacional, este Despacho,					
				</div>
				<div id="titulo-reporte" style="PADDING-TOP:10px">
					RESUELVE
				</div>
				<div id="contenido-texto">
					<label style="FONT-WEIGHT:bold">PRIMERO:</label>
					Ordenar  el embargo y secuestro de los dineros depositados en cuentas corrientes  y/o de ahorro  y/o cualquier 
					producto financiero, en bancos, corporaciones de ahorro y vivienda y compañias de financiamiento comercial en 
					todo  el  pais, a nombre del contribuyente antes indicado, hasta por la suma de $ 
					<b>
						<% Response.Write(FormatNumber(TotalGral,0)) %>
					</b>
					.<br><br>					
					<label style="FONT-WEIGHT:bold">SEGUNDO:</label>
					Ordenar  el embargo y secuestro del bien inmueble registrado en el Folio de Matrícula  Inmobiliaria No.
					<% Response.Write(Trim(objDataReader("id_deudor"))) %> de la Oficina de Registro de Instrumentos Públicos de Cartagena.
					<br>
					<br>
					<label style="FONT-WEIGHT:bold">TERCERO:</label>
					Librar los oficios necesarios para  dar cumplimiento  a la presente resolución.
					<br>
					<br>					
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
