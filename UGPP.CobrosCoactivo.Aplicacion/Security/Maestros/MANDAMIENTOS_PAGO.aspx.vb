Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class MANDAMIENTOS_PAGO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Habilitar el boton de crear el mandamiento de pago SOLO si esta en coactivo
            If NomEstadoProceso = "COACTIVO" Or NomEstadoProceso = "PERSUASIVO" Then
                'cmdAddNew.Enabled = True
            Else
                'cmdAddNew.Enabled = False
                CustomValidator1.Text = "Solo se pueden adicionar Mandamientos de Pago en estado PERSUASIVO o COACTIVO"
                CustomValidator1.IsValid = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                'cmdAddNew.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                CustomValidator1.IsValid = False
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    'cmdAddNew.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If

            End If

            BindGrid()
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Mandamientos encontrados " & grd.Rows.Count
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim pExpediente As String
        pExpediente = Request("pExpediente")
        Dim sql As String = "SELECT MANDAMIENTOS_PAGO.*,  EJEFISGLOBAL.EFIESTADO FROM MANDAMIENTOS_PAGO LEFT JOIN EJEFISGLOBAL ON MANDAMIENTOS_PAGO.NroExp = EJEFISGLOBAL.EFINROEXP WHERE MANDAMIENTOS_PAGO.NroExp = '" & pExpediente.Trim & "'"
        Return sql
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    'Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
    '    Dim pExpediente As String
    '    pExpediente = Request("pExpediente")
    '    Response.Redirect("EditMANDAMIENTOS_PAGO.aspx?pExpediente=" & pExpediente.Trim)
    'End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim NroResolMP As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Dim EstadoExpediente As String = grd.Rows(e.CommandArgument).Cells(1).Text
                Dim pExpediente As String
                pExpediente = Request("pExpediente")
                Response.Redirect("EditMANDAMIENTOS_PAGO.aspx?ID=" & NroResolMP & "&pExpediente=" & pExpediente & "&pEstadoExpediente=" & EstadoExpediente)
            End If
        End If
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub
End Class