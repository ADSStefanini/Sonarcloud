Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class SOLICITUDES_CAMBIOESTADO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            'If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            '    'Se inhabilita el botón Adicionar para este perfil
            '    cmdAddNew.Enabled = False
            'Else
            '    'Se habilita el botón Adicionar para usuarios con perfil diferente a Verificador de Pagos
            '    cmdAddNew.Enabled = True
            'End If

            BindGrid()

            If HayCambiosEstadoPendientes(Request("pExpediente")) Then
                CustomValidator1.Text = "Este expediente tiene cambios de estado pendientes"
                CustomValidator1.IsValid = False
                cmdAddNew.Visible = False
            End If
        End If
    End Sub

    Private Function HayCambiosEstadoPendientes(ByVal pExpediente As String) As Boolean
        Dim Respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM SOLICITUDES_CAMBIOESTADO WHERE NroExp = '" & pExpediente & "' AND estadosol = 1"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Respuesta = True
        End If

        'Close the Connection Object 
        Connection.Close()

        Try
            'Se verifica si el estado operativo es EnSolicitud no se permite realizar mas solicitudes
            Dim _tareaAsignadaBLL As New TareaAsignadaBLL
            Dim _tarea = _tareaAsignadaBLL.obtenerTareaAsignadaPorIdExpediente(pExpediente)
            If (_tarea.COD_ESTADO_OPERATIVO = 13) Then
                Respuesta = True
            End If
        Catch

        End Try
        Return Respuesta
    End Function

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "SELECT SOLICITUDES_CAMBIOESTADO.*, " & _
                        "USUARIOSabogado.nombre as USUARIOSabogadonombre, " & _
                        "ESTADOS_SOL_CAM_ESTestadosol.nombre as ESTADOS_SOL_CAM_ESTestadosolnombre, " & _
                        "ESTADOS_PROCESOestado.nombre as ESTADOS_PROCESOestadonombre, " & _
                        "USUARIOSrevisor.nombre as USUARIOSrevisornombre " & _
                    "FROM ((SOLICITUDES_CAMBIOESTADO " & _
                    "LEFT JOIN USUARIOS USUARIOSabogado ON SOLICITUDES_CAMBIOESTADO.abogado = USUARIOSabogado.codigo )  " & _
                    "LEFT JOIN ESTADOS_SOL_CAM_EST ESTADOS_SOL_CAM_ESTestadosol ON SOLICITUDES_CAMBIOESTADO.estadosol = ESTADOS_SOL_CAM_ESTestadosol.codigo ) " & _
                    "LEFT JOIN ESTADOS_PROCESO ESTADOS_PROCESOestado ON SOLICITUDES_CAMBIOESTADO.estado = ESTADOS_PROCESOestado.codigo " & _
                    "LEFT JOIN USUARIOS USUARIOSrevisor ON SOLICITUDES_CAMBIOESTADO.revisor = USUARIOSrevisor.codigo "

        sql = sql & "WHERE NroExp = '" & Request("pExpediente") & "' "

        If Len(Session("SOLICITUDES_CAMBIOESTADO.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("SOLICITUDES_CAMBIOESTADO.SortExpression") & " " & Session("SOLICITUDES_CAMBIOESTADO.SortDirection")

        End If
        Return sql
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditSOLICITUDES_CAMBIOESTADO.aspx
        Response.Redirect("EditSOLICITUDES_CAMBIOESTADO.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim idunico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim pExpediente As String = grd.Rows(e.CommandArgument).Cells(1).Text
            Response.Redirect("EditSOLICITUDES_CAMBIOESTADO.aspx?ID=" & idunico & "&pExpediente=" & pExpediente)
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("SOLICITUDES_CAMBIOESTADO.SortDirection"))
            Case "ASC"
                Session("SOLICITUDES_CAMBIOESTADO.SortDirection") = "DESC"
            Case "DESC"
                Session("SOLICITUDES_CAMBIOESTADO.SortDirection") = "ASC"
            Case Else
                Session("SOLICITUDES_CAMBIOESTADO.SortDirection") = "ASC"
        End Select

        Session("SOLICITUDES_CAMBIOESTADO.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub

End Class