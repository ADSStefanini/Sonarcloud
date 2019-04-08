Imports System.Data.SqlClient

Partial Public Class SOLICITUDCAMBIOESTADO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))

            'Puts the previous state of the txtSearchNroExp field done when the user has searched and moved to the EditSOLICITUDCAMBIOESTADO page and then came back
            txtSearchNroExp.Text = Session("SOLICITUDCAMBIOESTADO.txtSearchNroExp")

            'Puts the previous state of the txtSearchgestor field done when the user has searched and moved to the EditSOLICITUDCAMBIOESTADO page and then came back
            txtSearchgestor.Text = Session("SOLICITUDCAMBIOESTADO.txtSearchgestor")

            'Puts the previous state of the txtSearchestadoactual field done when the user has searched and moved to the EditSOLICITUDCAMBIOESTADO page and then came back
            txtSearchestadoactual.Text = Session("SOLICITUDCAMBIOESTADO.txtSearchestadoactual")

            'Puts the previous state of the txtSearchestadosolicitado field done when the user has searched and moved to the EditSOLICITUDCAMBIOESTADO page and then came back
            txtSearchestadosolicitado.Text = Session("SOLICITUDCAMBIOESTADO.txtSearchestadosolicitado")

            'Puts the previous state of the txtSearchaccion field done when the user has searched and moved to the EditSOLICITUDCAMBIOESTADO page and then came back
            txtSearchaccion.Text = Session("SOLICITUDCAMBIOESTADO.txtSearchaccion")
            BindGrid()
            'LoadThemes()

            'End If - if IsPostBack equals false
        End If
    End Sub

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
        Command.Parameters.AddWithValue("@NroExp", "%" & txtSearchNroExp.Text & "%")

        Command.Parameters.AddWithValue("@gestor", "%" & txtSearchgestor.Text & "%")

        Command.Parameters.AddWithValue("@estadoactual", "%" & txtSearchestadoactual.Text & "%")

        Command.Parameters.AddWithValue("@estadosolicitado", "%" & txtSearchestadosolicitado.Text & "%")

        Command.Parameters.AddWithValue("@accion", "%" & txtSearchaccion.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""

        ' OJO SOLICITUDCAMBIOESTADO NO ES UNA TABLA, SINO UNA VISTA 
        sql = sql & "SELECT * FROM SOLICITUDCAMBIOESTADO "

        Dim WhereClause As String = ""

        '24/sep/2014. ------------------------------------------------------------------------------------------------------'
        'Por solicitud de german pava, las solicitudes de cambio de estado no le llegaran directamente a angie (repartidor),
        ' sino que seguiran el siguiente conducto regular (nivel_escalamiento)
        ' 1. El abogado/gestor escala la solicitud al revisor
        ' 2. El revisor escala al subdirector
        ' 3. Si el subdirector aprueba la solicitud la envia al repartidor
        'WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.nivel_escalamiento = 3"

        Dim MTG As New MetodosGlobalesCobro
        Dim NomPerfil As String = MTG.GetNomPerfil(Session("mnivelacces"))             

        If NomPerfil = "REPARTIDOR" Then
            WhereClause = WhereClause & " and nivel_escalamiento = 3"

        ElseIf NomPerfil = "REVISOR" Then
            'Se espera que sea el mismo revisor quien entre a a pagina y NO el gestor de informacion u otro perfil
            'WhereClause = WhereClause & " and nivel_escalamiento = 1 AND revisor = '" & Session("sscodigousuario") & "'"
            WhereClause = WhereClause & " and revisor = '" & Session("sscodigousuario") & "'"

        Else
            'NomPerfil = "SUPERVISOR"
            WhereClause = WhereClause & " and nivel_escalamiento = 2"

        End If
        '24/sep/2014. ------------------------------------------------------------------------------------------------------'


        If txtSearchNroExp.Text.Length > 0 Then
            WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.NroExp like @NroExp"

        End If

        If txtSearchgestor.Text.Length > 0 Then
            WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.gestor like @gestor"

        End If

        If txtSearchestadoactual.Text.Length > 0 Then
            WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.estadoactual like @estadoactual"

        End If

        If txtSearchestadosolicitado.Text.Length > 0 Then
            WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.estadosolicitado like @estadosolicitado"

        End If

        If txtSearchaccion.Text.Length > 0 Then
            WhereClause = WhereClause & " and SOLICITUDCAMBIOESTADO.accion like @accion"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause

        End If

        If Len(Session("SOLICITUDCAMBIOESTADO.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("SOLICITUDCAMBIOESTADO.SortExpression") & " " & Session("SOLICITUDCAMBIOESTADO.SortDirection")
        Else
            sql = sql & " order by fecha DESC"
        End If

        Return sql

    End Function


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()

        Session("SOLICITUDCAMBIOESTADO.txtSearchNroExp") = txtSearchNroExp.Text
        Session("SOLICITUDCAMBIOESTADO.txtSearchgestor") = txtSearchgestor.Text
        Session("SOLICITUDCAMBIOESTADO.txtSearchestadoactual") = txtSearchestadoactual.Text
        Session("SOLICITUDCAMBIOESTADO.txtSearchestadosolicitado") = txtSearchestadosolicitado.Text
        Session("SOLICITUDCAMBIOESTADO.txtSearchaccion") = txtSearchaccion.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim idunico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim expediente As String = grd.Rows(e.CommandArgument).Cells(1).Text


            Dim MTG As New MetodosGlobalesCobro
            Dim NomPerfil As String = MTG.GetNomPerfil(Session("mnivelacces"))

            If NomPerfil = "REPARTIDOR" Then
                Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & expediente)

            ElseIf NomPerfil = "REVISOR" Then
                Response.Redirect("EditSOLICITUDES_CAMBIOESTADO.aspx?ID=" & idunico & "&pExpediente=" & expediente)

            ElseIf NomPerfil = "SUPERVISOR" Then
                Response.Redirect("EditSOLICITUDES_CAMBIOESTADO.aspx?ID=" & idunico & "&pExpediente=" & expediente)

            End If


        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("SOLICITUDCAMBIOESTADO.SortDirection"))
            Case "ASC"
                Session("SOLICITUDCAMBIOESTADO.SortDirection") = "DESC"
            Case "DESC"
                Session("SOLICITUDCAMBIOESTADO.SortDirection") = "ASC"
            Case Else
                Session("SOLICITUDCAMBIOESTADO.SortDirection") = "ASC"
        End Select

        Session("SOLICITUDCAMBIOESTADO.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
        Dim MTG As New MetodosGlobalesCobro
        Dim NomPerfil As String = MTG.GetNomPerfil(Session("mnivelacces"))

        If NomPerfil = "REPARTIDOR" Then
            Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        Else
            Response.Redirect("EJEFISGLOBAL.aspx")
        End If

    End Sub


    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
        Session("Paginacion") = 10

        Response.Redirect("../../login.aspx")
    End Sub
End Class