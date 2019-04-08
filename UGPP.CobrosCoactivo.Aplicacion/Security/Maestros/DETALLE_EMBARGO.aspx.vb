Imports System.Data.SqlClient

Partial Public Class DETALLE_EMBARGO
    Inherits System.Web.UI.Page
    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            txtSearchIdentifBien.Text = Session("DETALLE_EMBARGO.txtSearchIdentifBien")
            BindGrid()

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
        Session("DETALLE_EMBARGO.RecordsFound") = 0
        If Len(Session("DETALLE_EMBARGO.CurrentPage")) = 0 Then
            Session("DETALLE_EMBARGO.CurrentPage") = 1

        End If
        If Len(Session("DETALLE_EMBARGO.SortExpression")) = 0 Then
            Session("DETALLE_EMBARGO.SortExpression") = "idunico"
            Session("DETALLE_EMBARGO.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@IdentifBien", "%" & txtSearchIdentifBien.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("DETALLE_EMBARGO.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("DETALLE_EMBARGO.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        'Parametro pResolEm
        Dim pResolEm As String
        pResolEm = Request("pResolEm")

        Dim StartRecord As Long = PageSize * Session("DETALLE_EMBARGO.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[DETALLE_EMBARGO].*, TIPOS_BIENESTipoBien.nombre as TIPOS_BIENESTipoBiennombre, MAESTRO_BANCOSBanco.BAN_NOMBRE as MAESTRO_BANCOSBancoBAN_NOMBRE"
        Dim Table As String = "([DETALLE_EMBARGO] left join [TIPOS_BIENES] TIPOS_BIENESTipoBien on [dbo].[DETALLE_EMBARGO].TipoBien = TIPOS_BIENESTipoBien.codigo )  left join [MAESTRO_BANCOS] MAESTRO_BANCOSBanco on [dbo].[DETALLE_EMBARGO].Banco = MAESTRO_BANCOSBanco.BAN_CODIGO "
        Dim WhereClause As String = ""

        WhereClause = WhereClause & " and DETALLE_EMBARGO.NroResolEm = '" & pResolEm & "' "

        If txtSearchIdentifBien.Text.Length > 0 Then
            WhereClause = WhereClause & " and [DETALLE_EMBARGO].[IdentifBien] like @IdentifBien"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If
        Dim SortOrder As String = Session("DETALLE_EMBARGO.SortExpression") & " " & Session("DETALLE_EMBARGO.SortDirection")
        Dim sql As String = "WITH DETALLE_EMBARGORecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " DETALLE_EMBARGORecordCount AS ( SELECT * FROM DETALLE_EMBARGORecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM DETALLE_EMBARGORecordSet) AS RC ) "
        sql = sql & "SELECT * FROM DETALLE_EMBARGORecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim pResolEm As String
        pResolEm = Request("pResolEm")
        Response.Redirect("EditDETALLE_EMBARGO.aspx?pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("DETALLE_EMBARGO.CurrentPage") = 1
        BindGrid()
        UpdateLabels()
        Session("DETALLE_EMBARGO.txtSearchIdentifBien") = txtSearchIdentifBien.Text
    End Sub

    Private Function ExisteTDJ(ByVal pIdEmbargo As String, ByRef pNroDeposito As String) As Boolean
        Dim respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT NroDeposito FROM tdj WHERE idEmbargo = " & pIdEmbargo
        Dim Command As New SqlCommand(sql, Connection)

        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            respuesta = True
            pNroDeposito = Reader("NroDeposito").ToString().Trim.Trim
        Else
            pNroDeposito = ""
        End If

        Connection.Close()

        Return respuesta
    End Function

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        Dim idunico As String = grd.Rows(e.CommandArgument).Cells(0).Text.Trim
        Dim pResolEm As String
        pResolEm = Request("pResolEm")
        Dim pNroDeposito As String = ""
        '
        Dim NomTipoBien As String = grd.Rows(e.CommandArgument).Cells(2).Text.Trim
        Dim TipoBien As String = grd.Rows(e.CommandArgument).Cells(9).Text.Trim
        Dim IdBien As String = grd.Rows(e.CommandArgument).Cells(4).Text.Trim

        If e.CommandName = "Edt" Then
            Response.Redirect("EditDETALLE_EMBARGO.aspx?ID=" & idunico.Trim & "&pResolEm=" & pResolEm & "&pExpediente=" & Request("pExpediente").Trim)

        Else
            'e.CommandName = "EdtTDJ"
            'EditTDJ.aspx?IdEmbargo=592&ID=5454654&pResolEm=546545&pExpediente=80459            
            'Response.Redirect("EditTDJ.aspx?IdEmbargo=" & idunico.Trim & "&ID= 545 4654 &pResolEm=" & pResolEm & "&pExpediente=" & Request("pExpediente").Trim)
            
            'If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "BIEN INMUEBLE" Then
            If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "VEH&#205;CULO" Or _
                 (Left(NomTipoBien, 3) = "VEH" And Right(NomTipoBien, 4) = "CULO") Or _
                 NomTipoBien = "BIEN INMUEBLE" Or NomTipoBien = "MUEBLE" Or NomTipoBien = "OTRO" Then

                Response.Redirect("EditSECUESTROAVAREM.aspx?pIdUnico=" & idunico.Trim & "&pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim & "&pTipoBien=" & TipoBien & "&pIdBien=" & IdBien)

            Else
                If ExisteTDJ(idunico.Trim, pNroDeposito) Then
                    Response.Redirect("EditTDJ.aspx?IdEmbargo=" & idunico.Trim & "&ID=" & pNroDeposito.Trim & "&pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
                Else
                    Response.Redirect("EditTDJ.aspx?IdEmbargo=" & idunico.Trim & "&pResolEm=" & pResolEm.Trim & "&pExpediente=" & Request("pExpediente").Trim)
                End If

            End If

        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("DETALLE_EMBARGO.SortDirection"))
            Case "ASC"
                Session("DETALLE_EMBARGO.SortDirection") = "DESC"
            Case "DESC"
                Session("DETALLE_EMBARGO.SortDirection") = "ASC"
            Case Else
                Session("DETALLE_EMBARGO.SortDirection") = "ASC"
        End Select

        Session("DETALLE_EMBARGO.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("DETALLE_EMBARGO.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If

        '
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Objeto button dentro del gridView
            Dim btn As Button = CType(e.Row.Cells(8).Controls(0), Button)

            'Detectar tipo de bien
            Dim NomTipoBien As String = e.Row.Cells(2).Text
            If NomTipoBien = "VEHÍCULO" Or NomTipoBien = "VEH&#205;CULO" Or _
                 (Left(NomTipoBien, 3) = "VEH" And Right(NomTipoBien, 4) = "CULO") Or _
                 NomTipoBien = "BIEN INMUEBLE" Or NomTipoBien = "MUEBLE" Or NomTipoBien = "OTRO" Then

                btn.Text = "Gestionar Secuestro, Avalúo y Remate"
            Else
                btn.Text = "Gestionar Título de Depósito Judicial (TDJ)"
            End If

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("DETALLE_EMBARGO.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("DETALLE_EMBARGO.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("DETALLE_EMBARGO.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("DETALLE_EMBARGO.CurrentPage") = Session("DETALLE_EMBARGO.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("DETALLE_EMBARGO.CurrentPage") > 1 Then
            Session("DETALLE_EMBARGO.CurrentPage") = Session("DETALLE_EMBARGO.CurrentPage") - 1
        Else
            Session("DETALLE_EMBARGO.CurrentPage") = 1
        End If

        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("DETALLE_EMBARGO.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("DETALLE_EMBARGO.RecordsFound")
        lblPageNumber.Text = "Página " & Session("DETALLE_EMBARGO.CurrentPage") & " de " & GetPageCount()
    End Sub

    Protected Sub cmdRegresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRegresar.Click
        Response.Redirect("EditEMBARGOS.aspx?ID=" & Request("pResolEm").Trim & "&pExpediente=" & Request("pExpediente").Trim)
    End Sub



End Class