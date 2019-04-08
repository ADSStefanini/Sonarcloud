<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Math"%>
<%@ Page CodeBehind="oficio-notificacion-resejec.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.oficio_notificacion_resejec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Oficio que notifica la Resolución que ordena llevar adelante la ejecución</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="coactivo.css" type="text/css" rel="stylesheet">
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
				<IMG class="logorafa" src="images/escudo-bmanga-min.jpg">
				<p id="slogan">Tesorería Municipal</p>
			</div>
			<div id="contenido">
				<div id="contenido-texto"><br>
					Señor
					<br>
					<% Response.Write(Trim(objDataReader("nomdeudor"))) %>
					<br>
					<% Response.Write(Trim(objDataReader("direccion"))) %>
					<% Response.Write(Trim(objDataReader("dir_establ"))) %>
					<br>
					Ciudad
				</div>
				<div id="datos-cabecera">
					<!--campo--><br>
					<br>
					<br>
					<div class="etiqueta-cabecera">REFERENCIA
					</div>
					<div class="campo-cabecera">: Proceso Administrativo Coactivo del Distrito de 
						Cartagena
					</div>
					<div class="etiqueta-cabecera">Contra
					</div>
					<div class="campo-cabecera">:
						<% Response.Write(Trim(objDataReader("nomdeudor"))) %>
					</div>
					<div class="etiqueta-cabecera">Identificación
					</div>
					<div class="campo-cabecera">:
						<% Response.Write(Trim(objDataReader("id_propiet"))) %>
					</div>
					<!--
					<div class="etiqueta-cabecera">
						No. Mandamiento de Pago:
					</div>
					<div class="campo-cabecera">
						: <% Response.Write(Trim(objDataReader("numproceso"))) %>
					</div>				
					--></div>
				<div id="contenido-texto"><br>
					<br>
					<br>
					<br>
					<br>
					Por medio de la presente, le estamos remitiendo copia de la resolución proferida por la Tesorería 
					municipal de Bucaramanga, en la cual se ordena llevar adelante la ejecución dentro del proceso 
					administrativo coactivo de la referencia.
					<br>
					<br>
					La notificación de la citada resolución quedará surtida en la fecha de recibo del presente oficio.
					<br>
					<br>
					
					<br>
					<br>
					Cordialmente,
				</div>
				<div id="firma"><IMG class="logofirma" src="images/firma2.jpg">
					<div id="firma-texto"><br>
						<br>
						__________________________
						<br>
						Tesorero Municipal<br>
					</div>
					<div id="firma-mecanica">De Conformidad con el Decreto 2150 del 1995, la firma 
						mecánica que antecede tiene plena validez para todos los efectos legales.
					</div>
					<div id="cuadro-notificacion">__________________________
						<br>
						El notificado<br>
						Firma, cédula y huella
					</div>
				</div>
			</div>
		</div>
	</body>
</HTML>
