Imports System.Data.SqlClient
Partial Public Class EMBARGOS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Puts the previous state of the txtSearchNroResolEm field done when the user has searched and moved to the EditEMBARGOS page and then came back
            txtSearchNroResolEm.Text = Session("EMBARGOS.txtSearchNroResolEm")

            If NomEstadoProceso = "DEVUELTO" Or NomEstadoProceso = "TERMINADO" Then
                cmdAddNew.Visible = False
                CustomValidator1.Text = "No se pueden adicionar atos en estado " & NomEstadoProceso
                CustomValidator1.IsValid = False
            Else
                cmdAddNew.Visible = True
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim MTG As New MetodosGlobalesCobro
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    cmdAddNew.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If
                
            End If

            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Session("EMBARGOS.RecordsFound") = 0
        If Len(Session("EMBARGOS.CurrentPage")) = 0 Then
            Session("EMBARGOS.CurrentPage") = 1

        End If
        If Len(Session("EMBARGOS.SortExpression")) = 0 Then
            Session("EMBARGOS.SortExpression") = "NroResolEm"
            Session("EMBARGOS.SortDirection") = "ASC"

        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@NroResolEm", "%" & txtSearchNroResolEm.Text)

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("EMBARGOS.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False

        End If
        If Session("EMBARGOS.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False

        End If
    End Sub

    Private Function GetSQL() As String
        'Parametro pExpediente
        Dim pExpediente As String
        pExpediente = Request("pExpediente")

        Dim StartRecord As Long = PageSize * Session("EMBARGOS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[dbo].[EMBARGOS].*, ESTADOS_EMBARGOEstadoEm.nombre as ESTADOS_EMBARGOEstadoEmnombre"
        Dim Table As String = "[dbo].[EMBARGOS] left join [ESTADOS_EMBARGO] ESTADOS_EMBARGOEstadoEm on [dbo].[EMBARGOS].EstadoEm = ESTADOS_EMBARGOEstadoEm.codigo "
        Dim WhereClause As String = ""

        WhereClause = WhereClause & " and EMBARGOS.NroExp = '" & pExpediente & "' "

        If txtSearchNroResolEm.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[EMBARGOS].[NroResolEm] like @NroResolEm"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If
        Dim SortOrder As String = Session("EMBARGOS.SortExpression") & " " & Session("EMBARGOS.SortDirection")
        Dim sql As String = "WITH EMBARGOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table

        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " EMBARGOSRecordCount AS ( SELECT * FROM EMBARGOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM EMBARGOSRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM EMBARGOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim pExpediente As String
        pExpediente = Request("pExpediente")
        Response.Redirect("EditEMBARGOS.aspx?pExpediente=" & pExpediente.Trim)
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("EMBARGOS.CurrentPage") = 1
        BindGrid()
        UpdateLabels()
        Session("EMBARGOS.txtSearchNroResolEm") = txtSearchNroResolEm.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        Dim NroResolEm As String
        Dim pExpediente As String
        '
        NroResolEm = grd.Rows(e.CommandArgument).Cells(0).Text
        pExpediente = Request("pExpediente")

        If e.CommandName = "Editar" Then            
            Response.Redirect("EditEMBARGOS.aspx?ID=" & NroResolEm.Trim & "&pExpediente=" & pExpediente)
        Else
            'e.CommandName = "EdtDetalleEmb"
            Response.Redirect("DETALLE_EMBARGO.aspx?pResolEm=" & NroResolEm.Trim & "&pExpediente=" & pExpediente)
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting
        Select Case CStr(Session("EMBARGOS.SortDirection"))
            Case "ASC"
                Session("EMBARGOS.SortDirection") = "DESC"
            Case "DESC"
                Session("EMBARGOS.SortDirection") = "ASC"
            Case Else
                Session("EMBARGOS.SortDirection") = "ASC"
        End Select
        Session("EMBARGOS.SortExpression") = e.SortExpression
        BindGrid()
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("EMBARGOS.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("EMBARGOS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("EMBARGOS.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("EMBARGOS.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("EMBARGOS.CurrentPage") = Session("EMBARGOS.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("EMBARGOS.CurrentPage") > 1 Then
            Session("EMBARGOS.CurrentPage") = Session("EMBARGOS.CurrentPage") - 1
        Else
            Session("EMBARGOS.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("EMBARGOS.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Embargos encontrados " & Session("EMBARGOS.RecordsFound")
        lblPageNumber.Text = "Página " & Session("EMBARGOS.CurrentPage") & " de " & GetPageCount()
    End Sub
End Class