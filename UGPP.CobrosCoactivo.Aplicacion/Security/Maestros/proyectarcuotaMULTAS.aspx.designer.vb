﻿'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class proyectarcuotaMULTAS
    
    '''<summary>
    '''Control form1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Control ToolkitScriptManager1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    
    '''<summary>
    '''Control lblError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lkbdescargar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lkbdescargar As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control btnNuevo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnNuevo As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control cmdCalcularInteres.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmdCalcularInteres As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btndescargar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btndescargar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnGuardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control txttasainteres.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txttasainteres As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txttasamensual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txttasamensual As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtPorcentajeCuotaini.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtPorcentajeCuotaini As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control txtFechaExig.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaExig As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtfechaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtfechaPago As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtdiasmora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtdiasmora As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtValorDeuda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtValorDeuda As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtimorateresesmora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtimorateresesmora As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtIntCap.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIntCap As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtCuotaI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtCuotaI As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtSaldoDiferir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtSaldoDiferir As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtNCuotas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtNCuotas As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control txtInteresesProyectado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtInteresesProyectado As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control UpdatePanel1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UpdatePanel1 As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control chkexcluircuota.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkexcluircuota As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Control TxtNumAcuerdo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtNumAcuerdo As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtNroProceso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtNroProceso As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control Txt_solicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Txt_solicitante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtNom_Solicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtNom_Solicitante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control CmbSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CmbSolicitante As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control TxtGarante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtGarante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control TxtNom_garante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtNom_garante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control CmbGarante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CmbGarante As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control TxtDescripcionGarantia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents TxtDescripcionGarantia As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control DtgAcuerdos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents DtgAcuerdos As Global.System.Web.UI.WebControls.GridView
End Class