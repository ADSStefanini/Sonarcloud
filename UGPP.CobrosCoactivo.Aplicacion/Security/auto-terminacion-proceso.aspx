<%@ Page CodeBehind="auto-terminacion-proceso.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.auto_terminacion_proceso" %>
<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Auto de Terminación del Proceso</title>
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
				strSQL = "SELECT pr.id_deudor, pr.numproceso, pr.fecharesol, pr.fechamanda, pr.valormanda, pr.nomdeudor, mp.direccion, mp.dir_establ, mp.id_propiet, " & _
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
			%>
			<!--cabecera -->
			<div id="header">
				<h1 id="logo-text">REPUBLICA DE COLOMBIA</h1>
				<img src="images/escudo-bmanga-min.jpg" class="logorafa">
				<p id="slogan">Tesorería Municipal</p>
			</div>
			<div id="ciudad-fecha" style="PADDING-TOP:40px">
				<div id="texto-ciudad-fecha">
					Bucaramanga, septiembre 30 de 2002
				</div>
			</div>
						
			<div id="datos-cabecera">
				<!--campo-->
				<div class="etiqueta-cabecera">
					REFERENCIA
				</div>
				<div class="campo-cabecera">
					: Proceso EJECUTIVO POR JURISDICCION COACTIVA 
				</div>
				<div class="etiqueta-cabecera">
					Demandado
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("nomdeudor"))) %> 
				</div>
				<div class="etiqueta-cabecera">
					Expediente
				</div>
				<div class="campo-cabecera">
					: <% Response.Write(Trim(objDataReader("numproceso"))) %>					  
				</div>								
			</div>
			
			
			<div id="contenido">				
				<div id="titulo-reporte" style="PADDING-TOP:10px">
					AUTO TERMINACION DE PROCESO
				</div>
				
				<div id="contenido-texto">
					De conformidad con lo previsto en la Ley 6 de 1992 artículo 112, en el Código Contencioso Administrativo 
					artículos 68 y 79, en la Ley 100 de 1993 artículo 57, en la ley 446 de 1998 y en la Ley 1066 de 2006 el 
					Funcionario Ejecutor está facultado para adelantar la totalidad del trámite establecido para el proceso 
					de cobro coactivo.
					<br>
					<br>
					Que mediante auto No. <b><% Response.Write(Trim(objDataReader("numproceso"))) %></b> del
					<%							
						Dim DiaManda, MesManda, AnioManda As Integer						
						
						Response.Write("<b>")
						
						DiaManda = Day(objDataReader("fechamanda"))
						Response.Write(DiaManda)
						Response.Write(" de ")						
						MesManda = Month(objDataReader("fechamanda"))
						Response.Write(MonthName(MesManda))
						Response.Write(" de ")						
						AnioManda = Year(objDataReader("fechamanda"))
						Response.Write(AnioManda)
						
						Response.Write("</b>")												
					%>
					se efectuó la liquidación de crédito y costas quedando fijados los valores  de 
					<%
						'Calcular el total de todas las vigencias					
						Dim TotalGral As Decimal		
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
						Response.Write("<b>" & ValorEnLetras & "</b> ($ ")
						Response.Write("<b>" & FormatNumber(TotalEntero,0) & "</b>) ")
					%>
					que por concepto del crédito debe pagar la entidad ejecutada
					<b><% Response.Write(Trim(objDataReader("nomdeudor"))) %></b>
					a favor del MUNICIPIO DE BUCARAMANGA.
					<br>
					<br>
					Que por haberse verificado el efectivo pago de los valores por concepto de crédito establecido por 
					el Funcionario Ejecutor, este Despacho, 
				</div>				
				<div id="titulo-reporte" style="PADDING-TOP:10px">
					RESUELVE
				</div>
				<div id="contenido">
					<div id="contenido-texto">
						PRIMERO. Téngase por terminado el proceso de jurisdicción coactiva No. <b><% Response.Write(Trim(objDataReader("numproceso"))) %></b>
						por las cuotas partes con cargo a la pensión del señor(a) <% Response.Write(Trim(objDataReader("nomdeudor"))) %>
						<br>
						<br>
						SEGUNDO.  Archívese el expediente						
					</div>
					<br>
					<br>
					<img src="images/firma2.jpg" class="logofirma">
					<div id="firma-texto">
						<span>
							<b>NOTIFIQUESE Y CUMPLASE</b>
						</span>
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
				</div>
			</div>
		</div>
	</body>
</HTML>
