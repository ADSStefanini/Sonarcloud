Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class MAESTRO_TITULOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            BindGrid()

            'If ModoAddEditRepartidor = "ADICIONAR" Then
            '    'cmdAddNew.Enabled = False
            '    'cmdAddNew.Visible = False
            'End If

            'Solo los repartidores puede ingresar titulos
            If Session("mnivelacces") <> 5 Then
                cmdAddNew.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdAddNew.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                CustomValidator1.IsValid = False
            End If
        End If

    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Títulos encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select MAESTRO_TITULOS.*, TIPOS_TITULOMT_tipo_titulo.nombre as TIPOS_TITULOMT_tipo_titulonombre, " &
            "FORMAS_NOTIFICACIONMT_for_notificacion_titulo.nombre as FORMAS_NOTIFICACIONMT_for_notificacion_titulonombre " &
            "from ([dbo].[MAESTRO_TITULOS] " &
            "left join [TIPOS_TITULO] TIPOS_TITULOMT_tipo_titulo on [dbo].[MAESTRO_TITULOS].MT_tipo_titulo = TIPOS_TITULOMT_tipo_titulo.codigo )  " &
            "left join [FORMAS_NOTIFICACION] FORMAS_NOTIFICACIONMT_for_notificacion_titulo " &
            "on [dbo].[MAESTRO_TITULOS].MT_for_notificacion_titulo = FORMAS_NOTIFICACIONMT_for_notificacion_titulo.codigo " &
            " WHERE MAESTRO_TITULOS.mt_expediente = '" & Request("pExpediente") & "'"
        Return sql
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditMAESTRO_TITULOS.aspx
        ModoAddEditTitulo = "ADICIONAR"
        Response.Redirect("EditMAESTRO_TITULOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim MT_nro_titulo As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Dim MT_id_unico As String = grd.Rows(e.CommandArgument).Cells(8).Text
                ModoAddEditTitulo = "EDITAR"
                Response.Redirect("EditMAESTRO_TITULOS.aspx?ID=" & MT_nro_titulo & "&pExpediente=" & Request("pExpediente") & "&pIdUnico=" & MT_id_unico)
            End If
        End If
    End Sub
End Class