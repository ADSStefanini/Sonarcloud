<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page CodeBehind="citacion-mpago-veh.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.citacion_mpago_veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Auto de Mandamiento de Pago</title>
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
				strSQL = "SELECT veh_placa,veh_cedula,veh_avaluo,nombre,direccion FROM maeveh WHERE RTRIM(veh_placa) = '" & pRefeCata & "'"
				
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
				
				Dim NumProceso As String
				
				NumProceso = "04 - 000" & MID(objDataReader("veh_placa"),4,3)
			%>
			<!--cabecera -->
			<div id="header">
				<h1 id="logo-text">GOBERNACION DEL MAGDALENA</h1>
				<img src="images/escudo_magdalena.jpg" class="logorafa" style="WIDTH:70px;HEIGHT:71px">
				<p id="slogan">GRUPO DE COBRO ADMINISTRATIVO COACTIVO</p>
			</div>
			<div id="titulo-reporte">
				CITACION DE NOTIFICACION DE MANDAMIENTO DE PAGO No.
				<% Response.Write(NumProceso) %>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Santa Marta, septiembre 30 de 2002
				</div>
				<hr>
			</div>
			<div id="datos-cabecera">
				<!--campo-->
				<div class="etiqueta-cabecera">
					NOMBRE O RAZON SOCIAL
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("nombre"))) %>
				</div>
				<div class="etiqueta-cabecera">
					IDENTIFICACION (NIT. O CC.)
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("veh_cedula"))) %>
				</div>
				<div class="etiqueta-cabecera">
					DIRECCION
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("direccion"))) %>
				</div>
				<div class="etiqueta-cabecera">
					VEHICULO PLACA
				</div>
				<div class="campo-cabecera">
					:
					<% Response.Write(Trim(objDataReader("veh_placa"))) %>
				</div>
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					Sírvase  comparecer  a las instalaciones de la  Tesorería Departamental - Ventanilla de Liquidación de
					Impuesto de Vehículo del Departamento del Magdalena, ubicada en el primer piso del Palacio Tairona
					de esta ciudad en el  término de  diez  (10)  días  contados a   partir    de   la   presente   citación,
					a   efecto  de  notificarle   personalmente el mandamiento de pago  librado    en   su  contra por
					concepto  de    Impuesto   sobre Vehículos   Automotores dentro del proceso administrativo de 
					cobro coactivo ;  de  conformidad con el  artículo 826 del Estatuto Tributario Nacional.
				</div>
				<div id="contenido-texto">
					
				</div>
				
				<div id="contenido-texto">
					
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
						Funcionario Ejecutor
						<br>
						Grupo de Cobro Administrativo Coactivo
					</div>
					<div id="firma-mecanica" style="MARGIN-LEFT:18px">
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
