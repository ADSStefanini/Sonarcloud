<%@ Page CodeBehind="resol-embargo-veh.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resol_embargo_veh" %>
<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Auto de Mandamiento de Pago</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="coactivo.css" type="text/css">
		<style>
			table{
				border-spacing: 0px;
				border: 1px solid black;
				border-collapse:collapse;
			}
			th, td{					
				border: 1px solid black;
				border-top:none;
				border-right:none;
				padding-left:2px;
				padding-right:2px;
				width:234px;
			}
			th{
				border-top: 1px solid black;
				text-align:center;				
			}				
		</style>
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
				<div style="border: 1px solid black; width:600px;">
					RESOLUCION DE EMBARGO DE VEHICULO No. 
					<% Response.Write(NumProceso) %>
				</div>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Santa Marta, septiembre 30 de 2002
				</div>
				
			</div>
			<div id="datos-cabecera" style="height:100px;">
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
				<div class="etiqueta-cabecera">
					CUANTIA
				</div>
				<div class="campo-cabecera">
					: $ 
					<% 
						If objDataReader("veh_avaluo") > 0 Then 
							Response.Write(FormatNumber(objDataReader("veh_avaluo")*0.025,0))
						Else
							Response.Write("295,970")
						End If
					%>
				</div>
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					El suscrito funcionario ejecutor del Cobro Administrativo Coactivo del Departamento, en uso de sus  facultades , y   con 
					fundamento en la Liquidación Oficial de Aforo No. <b><% Response.Write(NumProceso) %></b> de fecha 
					06 de julio de 2002 ejecutoriada  por el  funcionario ejecutor del Grupo de Liquidación Oficial, 
					contentivo de la obligación tributaria que a continuación se discrimina:
					<br /><br />
					
				</div>
				<div id="contenido-texto">
					<table border=1 cellspacing=0 cellpadding=0>
                    	<tr>
                        	<th>
                            	IMPUESTO
                            </th>
                            <th>
                            	PERIODO
                            </th>
                            <th>
                            	CUANTIA
                            </th>                            
                        </tr>  
                        <tr>
                        	<td>
                        		<br />
                            	&nbsp;Sobre Vehículos Automotores 
                            	<br /><br />
                            </td>                            
                            <td>
								<br />
                            	&nbsp;Vigencia 2004 y  Anteriores
                            	<br /><br />
                            </td>
                            <td style="text-align:right">
								<br />
                            	<% 
									If objDataReader("veh_avaluo") > 0 Then 
										Response.Write(FormatNumber(objDataReader("veh_avaluo")*0.025,0))
									Else
										Response.Write("295,970")
									End If
								%>
								&nbsp;
								<br /><br />
                            </td>                            
                        </tr>                                                 
                    </table>
                    <br /><br />
					y teniendo en cuenta que el deudor(a) ha sido renuente a cumplir con el pago del impuesto correspondiente,
				</div>
				<div id="titulo-reporte" style="padding-top:10px;">
					RESUELVE
				</div>
				<div id="contenido-texto">
					<label style="font-weight:bold;">PRIMERO:</label>
					Ordenar el embargo del vehículo automotor de placa No.  
					<b><% Response.Write(Trim(objDataReader("veh_placa"))) %></b>
					de propiedad de <b><% Response.Write(Trim(objDataReader("nombre"))) %></b>.
					
					<br /><br />					
					<label style="font-weight:bold;">SEGUNDO:</label>
					Comunicar por oficio a la oficinas correspondientes. 
					<br /><br />
				</div>
				<div id="firma">
					<img src="images/firma2.jpg" class="logofirma">
					<div id="firma-texto">
						<span>
							<b>CUMPLASE</b></span>
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
