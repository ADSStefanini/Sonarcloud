Imports System.Data.SqlClient
Partial Public Class MENSAJES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))

            LoadcboTipoMensaje()
            BindGrid()

            If Len(Request("modo")) > 0 Then
                ABackRep.Visible = False
                A3.Visible = False
                spancerrarsesion.Visible = False
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            'Dim MTG As New MetodosGlobalesCobro
            'Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            'If idGestorResp <> Session("sscodigousuario") Then
            '    cmdAddNew.Visible = False
            '    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
            '    CustomValidator1.IsValid = False
            'End If

            '17/sep2014. Si se va a trabajar con un expediente en especifico, NO visualizar la opcion de enviados/recibidos 
            If Len(Request("pExpediente")) > 0 Then
                cboTipoMensaje.Visible = False
            End If
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("MENSAJES.RecordsFound") = 0
        If Len(Session("MENSAJES.CurrentPage")) = 0 Then
            Session("MENSAJES.CurrentPage") = 1
        End If
        If Len(Session("MENSAJES.SortExpression")) = 0 Then
            Session("MENSAJES.SortExpression") = "idunico"
            Session("MENSAJES.SortDirection") = "DESC"
        End If

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

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("MENSAJES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If
        If Session("MENSAJES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Sub LoadcboTipoMensaje()
        Dim dt As DataTable = New DataTable("TablaTiposMensajes")

        dt.Columns.Add("Codigo")
        dt.Columns.Add("Descripcion")

        Dim dr As DataRow

        dr = dt.NewRow()
        dr("Codigo") = "1"
        dr("Descripcion") = "Recibidos"
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Codigo") = "2"
        dr("Descripcion") = "Enviados"
        dt.Rows.Add(dr)

        cboTipoMensaje.DataSource = dt
        cboTipoMensaje.DataValueField = "Codigo"
        cboTipoMensaje.DataTextField = "Descripcion"
        cboTipoMensaje.DataBind()

    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("MENSAJES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "MENSAJES.idunico, MENSAJES.NroExp, MENSAJES.UsuOrigen, MENSAJES.UsuDestino, MENSAJES.Mensaje, " & _
                                "MENSAJES.FecEnvio, MENSAJES.FecRecibo, CASE leido WHEN 1 THEN 'SI' ELSE 'NO' END AS leido, " & _
                                "USUARIOSUsuOrigen.nombre as USUARIOSUsuOrigennombre, USUARIOSUsuDestino.nombre as USUARIOSUsuDestinonombre"

        Dim Table As String = "([MENSAJES] left join [USUARIOS] USUARIOSUsuOrigen on [dbo].[MENSAJES].UsuOrigen = USUARIOSUsuOrigen.codigo )  " & _
                            "left join [USUARIOS] USUARIOSUsuDestino on [dbo].[MENSAJES].UsuDestino = USUARIOSUsuDestino.codigo "

        Dim WhereClause As String = ""
        'La condicion depende del parametro
        If Len(Request("pExpediente")) > 0 Then
            WhereClause = " NroExp = '" & Request("pExpediente").Trim & "' "
        Else
            'WhereClause = " (UsuOrigen = '" & Session("sscodigousuario") & "' OR UsuDestino = '" & Session("sscodigousuario") & "') "
            '13/MZO/2014. "1" = recibidos, "2" = enviados
            If cboTipoMensaje.SelectedValue = "1" Then
                WhereClause = " (UsuDestino = '" & Session("sscodigousuario") & "') "
            Else
                WhereClause = " (UsuOrigen = '" & Session("sscodigousuario") & "') "
            End If
        End If

        ' 11/FEB/2014 Ordenar los mensajes por el campo fecha de envio en orden descendente xxx
        If True Then

        End If

        Dim SortOrder As String = Session("MENSAJES.SortExpression") & " " & Session("MENSAJES.SortDirection")
        Dim sql As String = "WITH MENSAJESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " MENSAJESRecordCount AS ( SELECT * FROM MENSAJESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM MENSAJESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM MENSAJESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditMENSAJES.aspx
        Response.Redirect("EditMENSAJES.aspx?pExpediente=" & Request("pExpediente"))
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim idunico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditMENSAJES.aspx?ID=" & idunico & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("MENSAJES.SortDirection"))
            Case "ASC"
                Session("MENSAJES.SortDirection") = "DESC"
            Case "DESC"
                Session("MENSAJES.SortDirection") = "ASC"
            Case Else
                Session("MENSAJES.SortDirection") = "ASC"
        End Select

        Session("MENSAJES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("MENSAJES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("MENSAJES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("MENSAJES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("MENSAJES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("MENSAJES.CurrentPage") = Session("MENSAJES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("MENSAJES.CurrentPage") > 1 Then
            Session("MENSAJES.CurrentPage") = Session("MENSAJES.CurrentPage") - 1
        Else
            Session("MENSAJES.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("MENSAJES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Mensajes encontrados " & Session("MENSAJES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("MENSAJES.CurrentPage") & " de " & GetPageCount()
    End Sub


    Protected Sub cboTipoMensaje_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTipoMensaje.SelectedIndexChanged
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
        'Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        Dim Perfil As String = GetNomPerfil(Session("sscodigousuario"))

        If Perfil = "REPARTIDOR" Then
            Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")

        ElseIf Perfil = "VERIFICADOR DE PAGOS" Then
            Response.Redirect("PAGOS.aspx")

        ElseIf Perfil = "GESTOR - ABOGADO" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "REVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "SUPERVISOR" Then
            Response.Redirect("EJEFISGLOBAL.aspx")

        Else
            ' SUPER ADMINISTRADOR
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