var OUTLOOKBAR_DEFINITION = {
	format:{
		target:'main',
		blankImage:'images/b.gif',
		rollback:true,
		animationSteps:3,
		animationDelay:20,
		templates:{
			panel:{
				common:'<table width="100%" height="37" border="0" cellspacing="0" cellpadding="0" background="images/panel_middle_{state}.gif"><tr><td><img src="images/panel_left_{state}.gif" width="10" height="37" /></td><td align="center"><div style="font: bold 11pt trebuchet ms, arial;">{text}</div></td><td align="right"><img src="images/panel_right_{state}.gif" width="10" height="37" /></td></tr></table>',
				normal:{state:'n'},
				rollovered:{state:'r'},
				clicked:{state:'c'}
			},
			item:{
				common:'<table border="0" width="100%"><tr><td><table width="100%" bgcolor="{borderColor}" border="0" cellspacing="1" cellpadding="0"><tr><td><table width="100%" border="0" bgcolor="{backgroundColor}" cellspacing="0" cellpadding="5"><tr align="center"><td><img src="images/icon_{icon}_{state}.gif" width="48" height="48" /></td></tr><tr align="center"><td><span style="font: 9pt verdana;">{text}</span></td></tr></table></td></tr></table></td></tr></table>',
				normal:{borderColor:'#D0D0D0', backgroundColor:'#D0D0D0', state:'n'},
				rollovered:{borderColor:'#0A246A', backgroundColor:'#B6BDD2', state:'r'}
			},
			upArrow:{
				common:'<img src="images/btn_up_{state}.gif" width="24" height="24" />',
				normal:{state:'n'},
				rollovered:{state:'r'},
				clicked:{state:'c'}
			},
			downArrow:{
				common:'<img src="images/btn_down_{state}.gif" width="24" height="24" />',
				normal:{state:'n'},
				rollovered:{state:'r'},
				clicked:{state:'c'}
			}
		}
	},
	panels:[
		{text:"Archivo", url:'',
			items:[
				{text:"Maestro de predios", icon:'01', url:'contenido.html'},
				{text:"Maestro de propietarios", icon:'02', url:'contenido.html'},
				{text:"Abogados", icon:'03', url:'contenido.html'},
				{text:"Tipos de cuenta", icon:'04', url:'contenido.html'},
				{text:"Actualizaci�n de descuentos", icon:'05', url:'contenido.html'}				
			]
		},
		{text:"Movimiento", url:'',
			items:[
				{text:"Liquidaci�n individual", icon:'06', url:'contenido.html'},
				{text:"Recibo de caja", icon:'07', url:'contenido.html'},
				{text:"Duplicados", icon:'08', url:'contenido.html'},
				{text:"Paz y Salvo", icon:'09', url:'contenido.html'},
				{text:"Liquidaci�n de intereses mensuales", icon:'10', url:'contenido.html'},
				{text:"Anulaci�n de documento", icon:'11', url:'contenido.html'},
				{text:"Liquidaci�n general", icon:'12', url:'contenido.html'},
				{text:"Notas cr�dito", icon:'13', url:'contenido.html'},
				{text:"Acuerdos de pago", icon:'14', url:'contenido.html?'},
				{text:"Liquidaci�n de Aforo", icon:'15', url:'contenido.html'},
				{text:"Persuasivo", icon:'16', url:'contenido.html'},
				{text:"Parametrizaci�n de t�rminos", icon:'17', url:'contenido.html'},
				{text:"Registro de d�as festivos", icon:'18', url:'contenido.html'}
			]
		},
		{text:"Reportes", url:'',
			items:[
				{text:"Saldos por contribuyente detallado", icon:'19', url:'contenido.html'},
				{text:"Saldos por contribuyente resumido", icon:'20', url:'saldos-por-contrib-resumidos.aspx'},
				{text:"An�lisis de contribuyente por cobrar", icon:'21', url:'analisis-contrib-por-cobrar.aspx'},
				{text:"Planilla de auditor�as", icon:'22', url:'contenido.html'},
				{text:"Listado bancario", icon:'23', url:'contenido.html'},
				{text:"An�lisis de cartera", icon:'24', url:'contenido.html'},
				{text:"M�dulo de estad�stica", icon:'25', url:'contenido.html'},
				{text:"Hist�rico anual por cliente", icon:'26', url:'contenido.html'},
				{text:"Reportes de declaraciones", icon:'27', url:'contenido.html'}				
			]
		},
		{text:"Consultas", url:'',
			items:[
				{text:"No declaraci�n por contribuyente", icon:'30', url:'contenido.html'},
				{text:"Saldos por contribuyente", icon:'31', url:'contenido.html'},
				{text:"An�lisis de contribuyente por cobrar", icon:'32', url:'contenido.html'},
				{text:"Consulta de movimientos", icon:'33', url:'contenido.html'},
				{text:"Consulta general de cartera", icon:'34', url:'contenido.html'},
				{text:"Consulta de cartera por abogado", icon:'35', url:'contenido.html'}				
			]
		},
		{text:"Utilidades", url:'',
			items:[
				{text:"Interfase o exportaci�n de archivos", icon:'36', url:'archivos-planos.aspx'},
				{text:"Anulaci�n de movimiento", icon:'37', url:'contenido.html'},
				{text:"Auditor�a de Cartera", icon:'38', url:'contenido.html'},
				{text:"Auditor�a de saldos", icon:'39', url:'contenido.html'},
				{text:"Cruce de informaci�n autom�tico", icon:'40', url:'contenido.html'},
				{text:"Configuraci�n de impresi�n", icon:'41', url:'contenido.html'}				
			]
		},		
		{text:"Proc. Tributario", url:'',
			items:[
				{text:"Fiscalizaci�n", icon:'42', url:'contenido.html'},
				{text:"Liquidaci�n Oficial", icon:'43', url:'contenido.html'},
				{text:"Discusi�n", icon:'44', url:'contenido.html'},
				{text:"Cobranzas", icon:'45', url:'cobranzas-vehic.aspx'}
			]
		}
	]
};
