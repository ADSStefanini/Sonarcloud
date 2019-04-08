<%@Import Namespace="System.Math"%>
<%@Import Namespace="coactivosyp"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page CodeBehind="resolucion-ordena-llevar-adelante-ejecucion.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resolucion_ordena_llevar_adelante_ejecucion" %>
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
			<div id="titulo-reporte" style="height:90px">
				<br>
				RESOLUCIÓN No.
				<% Response.Write(Trim(objDataReader("numproceso"))) %>
				<br>
				(30-09-2002)
				<br>
				<br>
				MEDIANTE LA CUAL SE ORDENA LLEVAR ADELANTE LA EJECUCIÓN
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Bucaramanga, septiembre 30 de 2002
				</div>				
			</div>
			<div id="datos-cabecera" style="height:50px">
				<!--campo-->
				<div class="etiqueta-cabecera" style="width:110px">
					Expediente No.
				</div>
				<div class="dospuntos">
					:
				</div>
				<div class="campo-cabecera" style="float:none">
					<% Response.Write(Trim(objDataReader("numproceso"))) %>
				</div>
				<div class="etiqueta-cabecera" style="WIDTH:110px;HEIGHT:30px">
					Referencia
				</div>
				<div class="dospuntos" style="HEIGHT:30px">
					:
				</div>
				<div class="campo-cabecera" style="FLOAT:none; WIDTH:620px; HEIGHT:30px">
					<% Response.Write("Proceso de cobro administrativo coactivo adelantado por el Distrito de Cartagena contra " & Trim(objDataReader("nomdeudor"))) %>
				</div>
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					La Tesorería Distrital de Cartagena de Indias, en uso de las facultades 
					conferidas por la Resolución de delegación N. <b>
						<% Response.Write(Trim(objDataReader("numproceso"))) %>
					</b>expedida por la señora Alcaldesa, artículo 91 literal d) numeral 6 de la 
					Ley 136 de 1994, el artículo 66 de la Ley 383 de 1997, en concordancia con el 
					artículo 5º de la Ley 1066 de 2006, procede a decidir sobre la excepción 
					propuesta en el proceso de cobro administrativo coactivo en referencia, según 
					el procedimiento previsto en el artículo 830 del Estatuto Tributario Nacional, 
					previa las siguientes:
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					ANTECEDENTES
				</div>
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
										
				%>
				<div id="contenido-texto">
					1) La Secretaría de Hacienda Distrital de Cartagena expidió la Resolución 
					(declaración, Liquidación oficial etc.) No. <b>
						<% Response.Write(Trim(objDataReader("numproceso"))) %>
					</b>de fecha <b>
						<% Response.Write(Trim(objDataReader("fecharesol"))) %>
					</b>en la cual consta una obligación clara, expresa y actualmente exigible, a 
					favor del Distrito de Cartagena y en contra de <b>
						<% Response.Write(Trim(objDataReader("nomdeudor"))) %>
					</b>C.C. / Nit. <b>
						<% Response.Write(FormatNumber(objDataReader("id_propiet"),0)) %>
					</b>por concepto de (impuesto predial unificado, impuesto de industria y 
					comercio y su complementarios de avisos y tableros), correspondiente al período 
					fiscal de 2003 al 2007, en cuantía de <b>
						<% Response.Write(ValorEnLetras) %>
					</b>$ <b>
						<% Response.Write(FormatNumber(TotalEntero,0)) %>
					</b>MONEDA LEGAL.
					<br>
					<br>
					2) La Resolución mencionada anteriormente fue debidamente notificada, teniendo 
					constancia de ejecutoria y contra la misma no fue interpuesto recurso alguno.
					<br>
					<br>
					3) La Resolución precitada contiene una obligación, clara, expresa y exigible, 
					prestando mérito ejecutivo de conformidad con lo previsto en el numeral 2º del 
					artículo 828 del Estatuto Tributario Nacional en virtud de la remisión del 
					artículo 5º de la Ley 1066 de 2006 y en concordancia con el artículo 68 del 
					Código Contencioso Administrativo.
					<br>
					<br>
					4) Que con base en la Resolución descrita anteriormente, este Despacho libró 
					Mandamiento de Pago de fecha
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
						
						Response.Write(", en contra del deudor por la suma de ")	
						Response.Write("<b>" & ValorEnLetras & "</b> $ ")
						Response.Write("<b>" & FormatNumber(TotalEntero,0) & "</b> ")
					%>
					MONEDA LEGAL, correspondiente al capital por concepto de (impuesto predial unificado, impuesto de industria y 
					comercio y su complementarios de avisos y tableros), correspondiente al período fiscal de 2003 al 2007,
					<br>
					<br>
					5) Que en cumplimiento de lo previsto en el artículo 826 del Estatuto Tributario, el deudor fue citado para 
					notificarse personalmente del mandamiento de pago mediante oficio enviado a su dirección, como quiera que no se 
					notificó personalmente del mandamiento de pago, se procedió a su notificación por correo.
					<br>
					<br>
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					<hr>
				</div>
				<div id="contenido-texto" style="margin-top:100px">
					6) Que dentro del término legal, el deudor no interpuso excepciones contra el mandamiento de pago.									
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					CONSIDERACIONES
				</div>
				<div id="contenido">
					<div id="contenido-texto">
						El Artículo 59 de la Ley 788 de 2002, dispuso que "los departamentos y municipios aplicarán los procedimientos 
						establecidos en el Estatuto Tributario Nacional, para la administración, determinación, discusión, cobro, 
						devoluciones, régimen sancionatorio incluida su imposición, a los impuestos por ellos administrados. Así mismo 
						aplicarán el procedimiento administrativo de cobro a las multas, derechos y demás recursos territoriales".
						<br><br>
						Esto es ratificado por la Ley 1066 de 2006, cuando en su artículo 5º  consagra que: "Las entidades públicas que 
						de manera permanente tengan a su cargo el ejercicio de las actividades y funciones administrativas o la prestación 
						de servicios del Estado colombiano y que en virtud de estas tengan que recaudar rentas o caudales públicos, del 
						nivel nacional, territorial, incluidos los órganos autónomos y entidades con régimen especial otorgado por la 
						Constitución Política, tienen jurisdicción coactiva para hacer efectivas las obligaciones exigibles a su favor y, 
						para estos efectos, deberán seguir el procedimiento descrito en el Estatuto Tributario".
						<br><br>
						De ello se desprende que las entidades territoriales, en el cobro de sus acreencias aplicarán los procedimientos 
						establecidos en el Estatuto Tributario Nacional.
						<br><br>
						La "Jurisdicción Coactiva" ha sido definida en la jurisprudencia de la Corte Constitucional como un privilegio 
						exorbitante de la Administración, que consiste en la facultad de cobrar directamente, sin que medie intervención 
						judicial, las deudas a su favor, adquiriendo la doble calidad de juez y parte, cuya justificación se encuentra en 
						la prevalencia del interés general, en cuanto dichos recursos se necesitan con urgencia para cumplir eficazmente 
						los fines estatales.
						<br><br>
						Ahora bien, en el caso que nos ocupa, los llamados presupuestos procesales se encuentran cabalmente satisfechos 
						dentro del presente asunto, por lo cual frente a ellos no cabe hacer ningún reparo.
						<br><br>
						En términos del artículo 68 del Código Contencioso Administrativo, puede demandarse ante la jurisdicción coactiva, 
						entre otros "todo acto administrativo ejecutoriado que imponga a favor de la Nación, de una entidad territorial, o 
						de un establecimiento público  de  cualquier  orden, la obligación de  pagar  una  suma líquida de dinero, en los 
						casos previstos en la ley", siempre que dichas obligaciones sean claras, expresas y exigibles.
						<br><br>
						Es lo que se predica de la Resolución empleada como título ejecutivo en este asunto.
						<br><br>
						Con base en dicha resolución, el despacho consideró en su oportunidad que una vez analizados los documentos antes 
						relacionados que cumplen con lo previsto en la legislación aplicable, por lo que era procedente librar mandamiento 
						de pago, pues en criterio del Funcionario ejecutor, de dicho instrumento se desprende una obligación clara, expresa 
						y exigible a cargo del deudor que constituye plena prueba en su contra, y de ahí que pueda ejercerse su cobro por 
						la vía aquí tramitada, por estar el ejecutante legitimado para perseguir el cobro del crédito derivado del título 
						ejecutivo allegado al expediente.
					</div>
				</div>				
				<div id="titulo-reporte" style="padding-top:10px">
					<hr>
				</div>
				<div id="contenido">
					<div id="contenido-texto" style="margin-top:100px">
						Por lo anterior y teniendo en cuenta las razones y consideraciones expuestas, el Funcionario Ejecutor,
					</div>
					<div id="titulo-reporte" style="padding-top:10px">
						RESUELVE
					</div>
					<div id="contenido-texto">
						PRIMERO: Seguir adelante la ejecución en contra del deudor.
						<br><br>
						SEGUNDO: Condenar en costas y gastos del proceso al deudor, en un porcentaje del 15% sobre el valor total de la 
						deuda y aquellos gastos directos en que se ha incurrido en la recuperación de esta obligación.
						<br><br>
						TERCERO: Practicar la liquidación del crédito y costas, agregando a los anteriores valores los intereses y 
						actualizaciones a que haya lugar, calculados de conformidad con las normas vigentes al momento del pago. Correr 
						traslado de ellas a la ejecutada.
						<br><br>
						CUARTO: Decretar el embargo de los saldos existentes en cuentas corrientes, depósitos de ahorro, préstamos aprobados, 
						CDT, sobregiros aprobados y cualesquier producto financiero de propiedad de la entidad ejecutada depositados en 
						todos los establecimientos bancarios, crediticios, financieros o similares, en cualquiera de sus oficinas, agencias, 
						sucursales en todo el país, a nombre de la ejecutada, hasta la suma de 
						<%
							Response.Write("<b>" & ValorEnLetras & "</b> $ ")
							Response.Write("<b>" & FormatNumber(TotalEntero,0) & "</b>. ")
						%>
					</div>
					<div id="contenido-texto">
						<br>						
						QUINTO: Ofíciese a los establecimientos bancarios, crediticios, financieros o similares, a efectos de que se hagan 
						efectivas las medidas cautelares decretadas. Líbrense los oficios respectivos.
						<br><br>
						SEXTO Disponer de los saldos de dinero que se resulten embargados o los que en un futuro se embarguen, en aras al 
						pago de la obligación.
						<br><br>
						SEPTIMO: Notifíquese esta resolución a la parte demandada, en la forma consagrada en el artículo 566 del Estatuto 
						Tributario, advirtiéndole que contra la presente decisión no procede recurso alguno.						
					</div>
					
					<br><br>
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
				</div>
				<div id="firma">
					
				</div>
			</div>
		</div>
	</body>
</HTML>
