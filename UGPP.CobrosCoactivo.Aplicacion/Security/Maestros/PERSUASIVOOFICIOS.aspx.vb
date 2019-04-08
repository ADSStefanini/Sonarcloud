Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class PERSUASIVOOFICIOS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                txtSearchNroOfi.Enabled = False
                txtSearchNoGuiaEnt.Enabled = False
                cmdSearch.Enabled = False
                cmdAddNew.Enabled = False
            Else
                'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
                txtSearchNroOfi.Enabled = True
                txtSearchNoGuiaEnt.Enabled = True
                cmdSearch.Enabled = True
                cmdAddNew.Enabled = True
            End If

            txtSearchNroOfi.Text = Session("PERSUASIVOOFICIOS.txtSearchNroOfi")
            txtSearchNoGuiaEnt.Text = Session("PERSUASIVOOFICIOS.txtSearchNoGuiaEnt")
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Session("PERSUASIVOOFICIOS.RecordsFound") = 0

        If Len(Session("PERSUASIVOOFICIOS.CurrentPage")) = 0 Then
            Session("PERSUASIVOOFICIOS.CurrentPage") = 1
        End If

        If Len(Session("PERSUASIVOOFICIOS.SortExpression")) = 0 Then
            Session("PERSUASIVOOFICIOS.SortExpression") = "NroExp"
            Session("PERSUASIVOOFICIOS.SortDirection") = "ASC"
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@NroOfi", txtSearchNroOfi.Text & "%")
        Command.Parameters.AddWithValue("@NoGuiaEnt", txtSearchNoGuiaEnt.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PERSUASIVOOFICIOS.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If

        If Session("PERSUASIVOOFICIOS.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        'Dim StartRecord As Long = PageSize * Session("PERSUASIVOOFICIOS.CurrentPage") - PageSize + 1
        'Dim StopRecord As Long = StartRecord + PageSize
        'Dim Columns As String = "PERSUASIVOOFICIOS.*"
        'Dim Table As String = "PERSUASIVOOFICIOS"
        'Dim WhereClause As String = ""

        'WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NroExp = @NroExp"

        'If txtSearchNroOfi.Text.Length > 0 Then
        '    WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NroOfi LIKE @NroOfi"
        'End If

        'If txtSearchNoGuiaEnt.Text.Length > 0 Then
        '    WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NoGuiaEnt LIKE @NoGuiaEnt"
        'End If

        'If WhereClause.Length > 0 Then
        '    WhereClause = Replace(WhereClause, " and ", "", , 1)
        'End If
        'Dim SortOrder As String = Session("PERSUASIVOOFICIOS.SortExpression") & " " & Session("PERSUASIVOOFICIOS.SortDirection")
        'Dim sql As String = "WITH PERSUASIVOOFICIOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        'If Len(WhereClause) > 0 Then
        '    sql = sql & " where " & WhereClause
        'End If
        'sql = sql & " ),"
        'sql = sql & " PERSUASIVOOFICIOSRecordCount AS ( SELECT * FROM PERSUASIVOOFICIOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PERSUASIVOOFICIOSRecordSet) AS RC ) "
        'sql = sql & "SELECT * FROM PERSUASIVOOFICIOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        'Return sql

        Dim StartRecord As Long = PageSize * Session("PERSUASIVOOFICIOS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "PERSUASIVOOFICIOS.*, PERSUASIVOTIPIFICACIONOFICIOtipificacion.nombre as PERSUASIVOTIPIFICACIONOFICIOtipificacionnombre"
        Dim Table As String = "PERSUASIVOOFICIOS left join PERSUASIVOTIPIFICACIONOFICIO PERSUASIVOTIPIFICACIONOFICIOtipificacion on PERSUASIVOOFICIOS.tipificacion = PERSUASIVOTIPIFICACIONOFICIOtipificacion.codigo "

        Dim WhereClause As String = ""
        WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NroExp = @NroExp"
        If txtSearchNroOfi.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NroOfi like @NroOfi"
        End If

        If txtSearchNoGuiaEnt.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOOFICIOS.NoGuiaEnt like @NoGuiaEnt"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If
        Dim sql As String = "WITH PERSUASIVOOFICIOSRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY numero ASC) AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " PERSUASIVOOFICIOSRecordCount AS ( SELECT * FROM PERSUASIVOOFICIOSRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PERSUASIVOOFICIOSRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PERSUASIVOOFICIOSRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql
    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPERSUASIVOOFICIOS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("PERSUASIVOOFICIOS.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("PERSUASIVOOFICIOS.txtSearchNroOfi") = txtSearchNroOfi.Text
        Session("PERSUASIVOOFICIOS.txtSearchNoGuiaEnt") = txtSearchNoGuiaEnt.Text
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim Noficio As String = grd.Rows(e.CommandArgument).Cells(1).Text
                Response.Redirect("EditPERSUASIVOOFICIOS.aspx?numero=" & Noficio & "&pExpediente=" & Request("pExpediente"))
            End If
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PERSUASIVOOFICIOS.SortDirection"))
            Case "ASC"
                Session("PERSUASIVOOFICIOS.SortDirection") = "DESC"
            Case "DESC"
                Session("PERSUASIVOOFICIOS.SortDirection") = "ASC"
            Case Else
                Session("PERSUASIVOOFICIOS.SortDirection") = "ASC"
        End Select

        Session("PERSUASIVOOFICIOS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PERSUASIVOOFICIOS.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PERSUASIVOOFICIOS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PERSUASIVOOFICIOS.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If
        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PERSUASIVOOFICIOS.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PERSUASIVOOFICIOS.CurrentPage") = Session("PERSUASIVOOFICIOS.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PERSUASIVOOFICIOS.CurrentPage") > 1 Then
            Session("PERSUASIVOOFICIOS.CurrentPage") = Session("PERSUASIVOOFICIOS.CurrentPage") - 1
        Else
            Session("PERSUASIVOOFICIOS.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PERSUASIVOOFICIOS.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Oficios encontrados " & Session("PERSUASIVOOFICIOS.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PERSUASIVOOFICIOS.CurrentPage") & " of " & GetPageCount()
    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    Session.Abandon()
    '    Session.Clear()
    '    Session.RemoveAll()
    '    Response.Redirect("../../login.aspx")
    'End Sub
End Class