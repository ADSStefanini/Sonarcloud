<%@ Page CodeBehind="resol-liquidacion-veh.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resol_liquidacion_veh" %>
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
				'1. Capturar la placa pasada como par�metro por la URL									
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
					LIQUIDACION OFICIAL DE AFORO No. 
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
					El funcionario ejecutor del Grupo de Liquidaci�n Oficial de la Secretar�a de Gesti�n Financiera Integral haciendo uso de las
					facultades conferidas por el Decreto Departamental No. 145 de fecha 29 de Marzo del a�o 2004 y los art�culos  643, 691, 
					717 y 719 del Estatuto Tributario Nacional; y en concordancia con el Estatuto Rentas del Departamento del Magdalena.,  
					profiere Liquidaci�n Oficial de Aforo del Impuesto Sobre Veh�culos Automotores del(os) periodo(s)  gravable(s) del (os) 
					a�o(s) que a continuaci�n se hace referencia  al contribuyente  <b><% Response.Write(Trim(objDataReader("nombre"))) %></b>
					propietario(a) del veh�culo de placas <b><% Response.Write(Trim(objDataReader("veh_placa"))) %></b>
					<br /><br />
					
				</div>
				<div id="titulo-reporte" style="padding-top:10px;">
					CONSIDERANDO:
				</div>
				<div id="contenido-texto">
					Que el contribuyente arriba en menci�n ha sido notificado el d�a 09 de Mayo de 2002 por correo certificado del 
					acto administrativo Emplazamiento Previo Por no Declarar del Impuesto Sobre Veh�culos Automotores del (os) periodo(s) 
					gravable(s) que a continuaci�n se hace referencia del veh�culo arriba enunciado.
					<br /><br />
					Que se encuentra vencido el t�rmino de un mes otorgado por el emplazamiento notificado.
					<br /><br />
					Que verificando los archivos de la Tesorer�a General del Departamento, no aparece(n) la(s) declaraci�n(es) privada(s) del  
					Impuesto Sobre Veh�culos Automotores  del(os)   periodo(s)  gravable(s) a�o(s) que a continuaci�n se hace referencia del 
					veh�culo de placas <b><% Response.Write(Trim(objDataReader("veh_placa"))) %></b> de propiedad de  
					<b><% Response.Write(Trim(objDataReader("nombre"))) %></b>
				</div>
				<div id="titulo-reporte" style="padding-top:10px;">
					RESUELVE:
				</div>
				<div id="contenido-texto">
					<label style="font-weight:bold;">ARTICULO PRIMERO:</label>
					Liquidar oficialmente el Impuesto Sobre Veh�culos Automotores, a cargo del contribuyente 					
					<b><% Response.Write(Trim(objDataReader("nombre"))) %></b> identificado con (c.c. NIT)
					<b><% Response.Write(Trim(objDataReader("veh_cedula"))) %></b> propietario (a) del veh�culo de placas
					<b><% Response.Write(Trim(objDataReader("veh_placa"))) %></b> correspondiente al(os) periodo(s) 
					gravable(s) de l(os) a�o(s) y como a continuaci�n se establece:										
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
                            	&nbsp;Sobre Veh�culos Automotores 
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
                    <br />
					M�s los intereses de mora y las sanciones que se generen desde la fecha  en que se debi� efectuar la declaraci�n privada y 
					el pago;   hasta la fecha en que se declare y cancele la obligaci�n.
				</div>
				
				<div id="contenido-texto">
					
					<br />			
					<label style="font-weight:bold;">ARTICULO SEGUNDO:</label>
					Contra esta Liquidaci�n Oficial de Aforo procede el Recurso de Reconsideraci�n, el cual podr� 
					interponerse ante el Grupo de Discusi�n de la Secretar�a de Gesti�n Financiera Integral  del Magdalena, dentro de los dos 
					meses siguientes a la notificaci�n de esta liquidaci�n.  
					<br /><br />
					<label style="font-weight:bold;">ARTICULO TERCERO:</label>
					 Notif�quese esta Liquidaci�n Oficial de Aforo por correo, mediante env�o de una copia del acto 
					 correspondiente a la direcci�n informada por el contribuyente, y se entender� surtida en la fecha de recibo del acto 
					 administrativo tributario en la direcci�n informada por el contribuyente, y en concordancia con el art�culo 718 del Estatuto 
					 Tributario Nacional se divulgar� a trav�s de un medio de comunicaci�n de amplia difusi�n. 
					 <br /><br />
					<label style="font-weight:bold;">ARTICULO CUARTO:</label>
					 Se remiten copias de esta Liquidaci�n Oficial de Aforo  al Grupo de Discusi�n y al Grupo de Cobro 
					 Administrativo Coactivo, para que se adelanten los tr�mites pertinentes.
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
						De Conformidad con el Decreto 2150 del 1995, la firma mec�nica que antecede 
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
