<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page CodeBehind="analisis_cont_cobrar_report.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.analisis_cont_cobrar_report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Analisis de Cartera</title>
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
				strSQL = "SELECT pr.id_deudor, pr.numproceso, pr.fechamanda, pr.valormanda, pr.nomdeudor, mp.direccion, mp.dir_establ, mp.id_propiet " & _						
						"FROM procesos pr, maepre mp WHERE pr.id_deudor = mp.refe_cata AND pr.valormanda >= " & pRefeCata & ""
				
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
				ANALISIS DE CONTRIBUYENTES POR COBRAR (ANALISIS DE CARTERA)
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					<!-- Bucaramanga, septiembre 30 de 2002 -->
				</div>
				<hr>
			</div>
			<div id="contenido">
				<div id="datos-cabecera" style="HEIGHT:22px">
					<div class="etiqueta-detalle" style="MARGIN-LEFT:60px; WIDTH:134px; TEXT-ALIGN:left">
						Ref. Catastral
					</div>
					<div class="etiqueta-detalle" style="WIDTH:219px; TEXT-ALIGN:left">
						Propietario
					</div>
					<div class="etiqueta-detalle" style="WIDTH:276px; TEXT-ALIGN:left">
						Dirección del predio
					</div>
					<div class="etiqueta-detalle" style="WIDTH:100px">
						Saldo deuda
					</div>
				</div>
				<!-- Aca va el ciclo del detalle de las vigencias -->
				<!-- contenido del detalle -->
				<% 
					'TotalGral = TotalGral + objDataReader("totalvigencia")
					Do 
					%>
				<div id="datos-detalle-pack">
					<div class="etiqueta-detalle" style="MARGIN-LEFT:60px; WIDTH:134px; TEXT-ALIGN:left">
						<% Response.Write(objDataReader("id_deudor")) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:219px; TEXT-ALIGN:left">
						<% Response.Write(Trim(objDataReader("nomdeudor"))) %>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:276px; TEXT-ALIGN:left">
						<% 
								Response.Write(objDataReader("direccion")) 
								Response.Write("|") 
								Response.Write(objDataReader("dir_establ")) 								
							%>
					</div>
					<div class="etiqueta-detalle" style="WIDTH:100px">
						<% Response.Write(FormatNumber(objDataReader("valormanda"),0)) %>
					</div>
				</div>
				<%												
				Loop While objDataReader.Read() 
					
				'dim textoprueba as string 
				'textoprueba = 0
				objDataReader.Close()
				objDataReader = objCommand.ExecuteReader()																																														
				objDataReader.Read()
				%>
			</div>
		</div>
	</body>
</HTML>
