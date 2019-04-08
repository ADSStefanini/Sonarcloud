<%@ Page CodeBehind="acta-mandamiento-pago.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.acta_mandamiento_pago" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Math"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Acta de Mandamiento de Pago</title>
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
				ACTA DE MANDAMIENTO DE PAGO No.<% Response.Write(Trim(objDataReader("numproceso"))) %>
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					<!-- Bucaramanga, septiembre 30 de 2002 -->
				</div>
				<hr>
			</div>
			<%
					'Calcular el total de todas las vigencias					
					'Dim TotalGral As Decimal		
					TotalGral = 0
					Do
						TotalGral = TotalGral + objDataReader("totalvigencia")
					Loop While objDataReader.Read()
					'Hay que devolver el datareader
					objDataReader.Close()
					objDataReader = objCommand.ExecuteReader()																																														
					objDataReader.Read()
					
					'Crear la variable del valor en letras					
					Dim clase As FuncionesTE = new FuncionesTE
					Dim ValorEnLetras As String					
					Dim TotalEntero As Long
					
					TotalEntero = Round(TotalGral)
					ValorEnLetras = clase.Num2Text(TotalEntero) & " PESOS "
										
				%>
			
			<div id="contenido">
				<div id="contenido-texto">
					
					En ciudad de Bucaramanga, en la fecha de <b><% Response.Write(Trim(objDataReader("fechamanda"))) %></b> 
					se presentó al Despacho el señor <b><% Response.Write(Trim(objDataReader("nomdeudor"))) %></b>
					identificado con c.c. No. <b><% Response.Write(Trim(objDataReader("id_propiet"))) %></b>
					con el fin de recibir la notificación enunciada en el oficio No. <b><% Response.Write(Trim(objDataReader("numproceso"))) %></b> 
					de fecha <b><% Response.Write(Trim(objDataReader("fechamanda"))) %></b>
					. Con base en lo dispuesto en el artículo 826 del Estatuto Tributario se procede a notificarle el AUTO DE MANDAMIENTO DE PAGO  
					dictado en estas diligencias, fechado el día <% Response.Write(Trim(objDataReader("fechamanda"))) %>
					visto a folio <% Response.Write(Trim(objDataReader("numproceso"))) %>
					, por el cual se ordena pagar a favor del Municipio de Bucaramanga la suma de 
					<%
							Response.Write("<b>" & ValorEnLetras & "</b> $ ")
							Response.Write("<b>" & FormatNumber(TotalEntero,0) & "</b>. ")
					%>
					por concepto de Impuesto Predial Unificado más los intereses de Ley -si los hubiere- desde cuando la 
					obligación se hizo exigible y hasta cuando se realice el pago y las costas del proceso. 
					<b>Se le hace entrega de una copia del Mandamiento.</b>
					Se advierte al notificado que puede denunciar bienes de su propiedad para garantizar el pago 
					de la obligación.
					<br><br>
					Se informa al (los) notificado (s) que cuenta (n) con quince (15) días para pagar o para 
					proponer por escrito excepciones, los cuales se contarán a partir del día siguiente a la notificación 
					del mandamiento de pago de conformidad con lo previsto en el artículo 830 del Estatuto Tributario. 
					No siendo otro el objeto de la presente diligencia, se da por terminada y en consecuencia se firma por 
					los que en ella intervinieron.
				</div>
				
				
				<%
				'dim textoprueba as string 
				'textoprueba = 0
				objDataReader.Close()
				objDataReader = objCommand.ExecuteReader()																																														
				objDataReader.Read()
				%>
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
						Tesorero Municipal<br>						
					</div>
					<div id="firma-mecanica">						
						De Conformidad con el Decreto 2150 del 1995, la firma mecánica que antecede 
						tiene plena validez para todos los efectos legales.
					</div>
					
					<div id="cuadro-notificacion">
						
						__________________________
						<br>
						El notificado<br>						
					</div>
					
				</div>								
			</div>
		</div>
	</body>
</HTML>
