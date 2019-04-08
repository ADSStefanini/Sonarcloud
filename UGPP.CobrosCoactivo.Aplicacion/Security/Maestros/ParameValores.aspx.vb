Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica


Partial Public Class ParameValores
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Sub mostrarMensaje(ByVal sMensaje As String)
        Dim script As String = String.Format("alert('{0}');", sMensaje)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Page_Load", script, True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            PintarGridViewValores()
        End If

    End Sub
    ''' <summary>
    ''' Metodo que ejecuta el llenado y pinta el GridView
    ''' </summary>
    Protected Sub PintarGridViewValores()
        Dim MetodoSel As ValoresBLL = New ValoresBLL()
        Dim ValoresTraer As List(Of Valores) = New List(Of Valores)
        ValoresTraer = MetodoSel.ConsultarDatosValores()
        grdValores.DataSource = ValoresTraer
        grdValores.DataBind()
    End Sub
    ''' <summary>
    ''' Accion Click del BtnGuardar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub BtnGuardarValores(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        For Each row As GridViewRow In grdValores.Rows
            Dim CodTipoObligacion As String = row.Cells(0).Text
            Dim ChkVALOR_OBLIGACION As CheckBox = CType(row.FindControl("ChkValObliga"), CheckBox)
            Dim ChkPARTIDA_GLOBAL As CheckBox = CType(row.FindControl("ChkPartidaGlobal"), CheckBox)
            Dim ChkSANCION_OMISION As CheckBox = CType(row.FindControl("ChkSancionOmisio"), CheckBox)
            Dim ChkSANCION_INEXACTITUD As CheckBox = CType(row.FindControl("ChkSancionInexa"), CheckBox)
            Dim MetodoInsert As ValoresBLL = New ValoresBLL()
            MetodoInsert.InsertDatValores(CodTipoObligacion, ChkVALOR_OBLIGACION.Checked, ChkPARTIDA_GLOBAL.Checked, ChkSANCION_OMISION.Checked, ChkSANCION_INEXACTITUD.Checked, False)
        Next
        mostrarMensaje("Guardado realizado exitosamente.")
        PintarGridViewValores()
    End Sub


    ''' <summary>
    ''' logout
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub
    ''' <summary>
    ''' atras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub
End Class