<%@ Page CodeBehind="resolucion-terminacion-proceso.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resolucion_terminacion_proceso" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Math"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Resolución de Terminación de Proceso</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="coactivo.css" type="text/css">
	</HEAD>
	<body>
		<div id="wrap">
			<%	
				'1. Capturar la placa pasada como parámetro por la URL									
				Dim pRefeCata As String
			    Me.Parameter = Request.QueryString("")
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
				RESOLUCION DE DESEMBARGO No.<% Response.Write(Trim(objDataReader("numproceso"))) %>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Bucaramanga, diciembre 30 de 2002
				</div>
				<hr>
			</div>
			<div id="datos-cabecera">
				<!--campo-->
				<div class="etiqueta-cabecera">
					Contribuyente
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("nomdeudor"))) %>
				</div>
				<div class="etiqueta-cabecera">
					Identificación
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("id_propiet"))) %>
				</div>				
				<div class="etiqueta-cabecera">
					Ref. Catastral
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("id_deudor"))) %>
				</div>
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					Se  desprende  del estudio del presente expediente, que la obligación que dio origen  a la gestión de cobro
					contra el contribuyente de la referencia se encuentra cancelada en su totalidad, por lo que ha desaparecido
					la causa  para proseguir con la misma.
					<br><br>
					Por la anterior, este Despacho:					
				</div>
				<div id="titulo-reporte" style="PADDING-TOP:10px">
					RESUELVE
				</div>
				<div id="contenido-texto">
					1.  Dar  por  terminadas  las actuaciones que  se vienen adelantando contra el contribuyente señalado en la
					referencia por  las obligaciones contenidas en la resolución No.<% Response.Write(Trim(objDataReader("numproceso"))) %>				
					de fecha 26 de abril del  año  2002  por  medio  de  la cual se declara la existencia de unas obligaciones de 
					impuesto predial unificado a cargo de un predio, correspondiente  al (los) siguientes(s) periodo(s):
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
						'Dim clase As FuncionesTE = new FuncionesTE
						'Response.Write(clase.Monto(100))
						'Dim ValorEnLetras As String
						'ValorEnLetras = ""
						Dim TotalEntero As Long
						
						'TotalEntero = Round(TotalGral)
						'ValorEnLetras = clase.Num2Text(TotalEntero) & " PESOS CON 00/100"
						'Response.Write(clase.Monto(100))
						'Response.Write(" <b>")
						'Response.Write(ValorEnLetras) 
						'Response.Write("</b>")
					%>					
				</div>
				<div id="contenido-texto">					
					<br>				
					2. Decretar el desembargo de los bienes trabados en este asunto.
					<br>
					<br>
					3. Endosar a favor del contribuyente los títulos de depósito judicial que existieran.
					<br>
					<br>
					4. Líbrense los oficios a que haya lugar.
					<br>
					<br>
					Archívese el expediente.
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
