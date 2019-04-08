<%@ Page CodeBehind="resolucion-llevar-ejecucion-vehic.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.resolucion_llevar_ejecucion_vehic" %>
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
				
				Dim NumProceso As String
				
				NumProceso = "04 - 000" & MID(objDataReader("veh_placa"),4,3)
			%>
			<!--cabecera -->
			<div id="header">
				<h1 id="logo-text">GOBERNACION DEL MAGDALENA</h1>
				<img src="images/escudo_magdalena.jpg" class="logorafa" style="WIDTH:70px;HEIGHT:71px">
				<p id="slogan">GRUPO DE COBRO ADMINISTRATIVO COACTIVO</p>
			</div>
			<div id="titulo-reporte" style="height:90px">
				<br>
				RESOLUCI�N No.
				<% Response.Write(NumProceso) %>
				<br>
				(30-09-2002)
				<br>
				<br>
				MEDIANTE LA CUAL SE ORDENA LLEVAR ADELANTE LA EJECUCI�N
			</div>
			<div id="ciudad-fecha">
				<div id="texto-ciudad-fecha">
					Santa Marta, septiembre 30 de 2002
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
					<% Response.Write(NumProceso) %>
				</div>
				<div class="etiqueta-cabecera" style="WIDTH:110px;HEIGHT:30px">
					Referencia
				</div>
				<div class="dospuntos" style="HEIGHT:30px">
					:
				</div>
				<div class="campo-cabecera" style="FLOAT:none; WIDTH:620px; HEIGHT:30px">
					<% Response.Write("Proceso de cobro administrativo coactivo adelantado por el Departamento del Magdalena contra " & Trim(objDataReader("nombre"))) %>
				</div>
			</div>
			<div id="contenido">
				<div id="contenido-texto">
					La Tesorer�a Departamental del Magdalena, en uso de las facultades 
					conferidas por la Resoluci�n de delegaci�n N. <b>
						<% Response.Write(NumProceso) %>
					</b>expedida por el se�or Gobernador, art�culo 91 literal d) numeral 6 de la 
					Ley 136 de 1994, el art�culo 66 de la Ley 383 de 1997, en concordancia con el 
					art�culo 5� de la Ley 1066 de 2006, procede a decidir sobre la excepci�n 
					propuesta en el proceso de Cobro Administrativo Coactivo en referencia, seg�n 
					el procedimiento previsto en el art�culo 830 del Estatuto Tributario Nacional, 
					previa las siguientes:
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					ANTECEDENTES
				</div>
				
				<div id="contenido-texto">
					1) La Secretar�a de Hacienda Departamental del Magdalena expidi� la Resoluci�n 
					(declaraci�n, Liquidaci�n oficial etc.) No. <b>
						<% Response.Write(NumProceso) %>
					</b>de fecha <b>
						09 de Mayo de 2002
					</b>en la cual consta una obligaci�n clara, expresa y actualmente exigible, a 
					favor del Departamento del Magdalena y en contra de <b>
						<% Response.Write(Trim(objDataReader("nombre"))) %>
					</b>C.C. / Nit. <b>
						<% Response.Write(FormatNumber(objDataReader("veh_cedula"),0)) %>
					</b>por concepto de (Impuesto Sobre Vehiculos Automotores correspondiente al per�odo 
					fiscal 2004 y anteriores, en cuant�a de $<b>
						<% 
							If objDataReader("veh_avaluo") > 0 Then 
								Response.Write(FormatNumber(objDataReader("veh_avaluo")*0.025,0))
							Else
								Response.Write("295,970")
							End If
						%>
					</b>
					<br>
					<br>
					2) La Resoluci�n mencionada anteriormente fue debidamente notificada, teniendo 
					constancia de ejecutoria y contra la misma no fue interpuesto recurso alguno.
					<br>
					<br>
					3) La Resoluci�n precitada contiene una obligaci�n, clara, expresa y exigible, 
					prestando m�rito ejecutivo de conformidad con lo previsto en el numeral 2� del 
					art�culo 828 del Estatuto Tributario Nacional en virtud de la remisi�n del 
					art�culo 5� de la Ley 1066 de 2006 y en concordancia con el art�culo 68 del 
					C�digo Contencioso Administrativo.
					<br>
					<br>
					4) Que con base en la Resoluci�n descrita anteriormente, este Despacho libr� 
					Mandamiento de Pago de fecha 09 de Mayo de 2002, en contra del deudor por la suma de 
					<% 
						If objDataReader("veh_avaluo") > 0 Then 
							Response.Write(FormatNumber(objDataReader("veh_avaluo")*0.025,0))
						Else
							Response.Write("295,970")
						End If
					%>
					, correspondiente al capital por concepto de (Impuesto Sobre Vehiculos Automotores), 
					correspondiente al per�odo fiscal 2004 y anteriores,
					<br>
					<br>
					5) Que en cumplimiento de lo previsto en el art�culo 826 del Estatuto Tributario, el deudor fue citado para 
					notificarse personalmente del mandamiento de pago mediante oficio enviado a su direcci�n, como quiera que no se 
					notific� personalmente del mandamiento de pago, se procedi� a su notificaci�n por correo.
					<br>
					<br>
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					<hr>
				</div>
				<div id="contenido-texto" style="margin-top:100px">
					6) Que dentro del t�rmino legal, el deudor no interpuso excepciones contra el mandamiento de pago.									
				</div>
				<div id="titulo-reporte" style="padding-top:10px">
					CONSIDERACIONES
				</div>
				<div id="contenido">
					<div id="contenido-texto">
						El Art�culo 59 de la Ley 788 de 2002, dispuso que "los departamentos y municipios aplicar�n los procedimientos 
						establecidos en el Estatuto Tributario Nacional, para la administraci�n, determinaci�n, discusi�n, cobro, 
						devoluciones, r�gimen sancionatorio incluida su imposici�n, a los impuestos por ellos administrados. As� mismo 
						aplicar�n el procedimiento administrativo de cobro a las multas, derechos y dem�s recursos territoriales".
						<br><br>
						Esto es ratificado por la Ley 1066 de 2006, cuando en su art�culo 5�  consagra que: "Las entidades p�blicas que 
						de manera permanente tengan a su cargo el ejercicio de las actividades y funciones administrativas o la prestaci�n 
						de servicios del Estado colombiano y que en virtud de estas tengan que recaudar rentas o caudales p�blicos, del 
						nivel nacional, territorial, incluidos los �rganos aut�nomos y entidades con r�gimen especial otorgado por la 
						Constituci�n Pol�tica, tienen jurisdicci�n coactiva para hacer efectivas las obligaciones exigibles a su favor y, 
						para estos efectos, deber�n seguir el procedimiento descrito en el Estatuto Tributario".
						<br><br>
						De ello se desprende que las entidades territoriales, en el cobro de sus acreencias aplicar�n los procedimientos 
						establecidos en el Estatuto Tributario Nacional.
						<br><br>
						La "Jurisdicci�n Coactiva" ha sido definida en la jurisprudencia de la Corte Constitucional como un privilegio 
						exorbitante de la Administraci�n, que consiste en la facultad de cobrar directamente, sin que medie intervenci�n 
						judicial, las deudas a su favor, adquiriendo la doble calidad de juez y parte, cuya justificaci�n se encuentra en 
						la prevalencia del inter�s general, en cuanto dichos recursos se necesitan con urgencia para cumplir eficazmente 
						los fines estatales.
						<br><br>
						Ahora bien, en el caso que nos ocupa, los llamados presupuestos procesales se encuentran cabalmente satisfechos 
						dentro del presente asunto, por lo cual frente a ellos no cabe hacer ning�n reparo.
						<br><br>
						En t�rminos del art�culo 68 del C�digo Contencioso Administrativo, puede demandarse ante la jurisdicci�n coactiva, 
						entre otros "todo acto administrativo ejecutoriado que imponga a favor de la Naci�n, de una entidad territorial, o 
						de un establecimiento p�blico  de  cualquier  orden, la obligaci�n de  pagar  una  suma l�quida de dinero, en los 
						casos previstos en la ley", siempre que dichas obligaciones sean claras, expresas y exigibles.
						<br><br>
						Es lo que se predica de la Resoluci�n empleada como t�tulo ejecutivo en este asunto.
						<br><br>
						Con base en dicha resoluci�n, el despacho consider� en su oportunidad que una vez analizados los documentos antes 
						relacionados que cumplen con lo previsto en la legislaci�n aplicable, por lo que era procedente librar mandamiento 
						de pago, pues en criterio del Funcionario ejecutor, de dicho instrumento se desprende una obligaci�n clara, expresa 
						y exigible a cargo del deudor que constituye plena prueba en su contra, y de ah� que pueda ejercerse su cobro por 
						la v�a aqu� tramitada, por estar el ejecutante legitimado para perseguir el cobro del cr�dito derivado del t�tulo 
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
						PRIMERO: Seguir adelante la ejecuci�n en contra del deudor.
						<br><br>
						SEGUNDO: Condenar en costas y gastos del proceso al deudor, en un porcentaje del 15% sobre el valor total de la 
						deuda y aquellos gastos directos en que se ha incurrido en la recuperaci�n de esta obligaci�n.
						<br><br>
						TERCERO: Practicar la liquidaci�n del cr�dito y costas, agregando a los anteriores valores los intereses y 
						actualizaciones a que haya lugar, calculados de conformidad con las normas vigentes al momento del pago. Correr 
						traslado de ellas a la ejecutada.
						<br><br>
						CUARTO: Decretar el embargo de los saldos existentes en cuentas corrientes, dep�sitos de ahorro, pr�stamos aprobados, 
						CDT, sobregiros aprobados y cualesquier producto financiero de propiedad de la entidad ejecutada depositados en 
						todos los establecimientos bancarios, crediticios, financieros o similares, en cualquiera de sus oficinas, agencias, 
						sucursales en todo el pa�s, a nombre de la ejecutada, hasta la suma de 
						<% 
							If objDataReader("veh_avaluo") > 0 Then 
								Response.Write(FormatNumber(objDataReader("veh_avaluo")*0.025,0))
							Else
								Response.Write("295,970")
							End If
						%>
					</div>
					<div id="contenido-texto">
						<br>						
						QUINTO: Of�ciese a los establecimientos bancarios, crediticios, financieros o similares, a efectos de que se hagan 
						efectivas las medidas cautelares decretadas. L�brense los oficios respectivos.
						<br><br>
						SEXTO Disponer de los saldos de dinero que se resulten embargados o los que en un futuro se embarguen, en aras al 
						pago de la obligaci�n.
						<br><br>
						SEPTIMO: Notif�quese esta resoluci�n a la parte demandada, en la forma consagrada en el art�culo 566 del Estatuto 
						Tributario, advirti�ndole que contra la presente decisi�n no procede recurso alguno.						
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
						De Conformidad con el Decreto 2150 del 1995, la firma mec�nica que antecede 
						tiene plena validez para todos los efectos legales.
					</div>
				</div>
				<div id="firma">
					
				</div>
			</div>
		</div>
	</body>
</HTML>
