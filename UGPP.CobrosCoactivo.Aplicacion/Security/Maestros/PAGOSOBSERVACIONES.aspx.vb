Imports System.Data.SqlClient

Partial Public Class PAGOSOBSERVACIONES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Private TipoLectura As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Session("PAGOSOBSERVACIONES.RecordsFound") = 0
        If Len(Session("PAGOSOBSERVACIONES.CurrentPage")) = 0 Then
            Session("PAGOSOBSERVACIONES.CurrentPage") = 1
        End If
        If Len(Session("PAGOSOBSERVACIONES.SortExpression")) = 0 Then
            Session("PAGOSOBSERVACIONES.SortExpression") = "IdUnico"
            Session("PAGOSOBSERVACIONES.SortDirection") = "ASC"
        End If
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        Try
            If (Trim(Request("pExpediente").Trim) <> "") Then
                Command.Parameters.AddWithValue("@NroExp", Trim(Request("pExpediente").Trim))
            End If

        Catch ex As Exception
            Command.Parameters.AddWithValue("@NroExp", Trim(Request("pExpedientel").Trim))
            cmdAddNew.Visible = False

            grd.Columns(5).Visible = False

        End Try


        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PAGOSOBSERVACIONES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If
        If Session("PAGOSOBSERVACIONES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("PAGOSOBSERVACIONES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "PAGOSOBSERVACIONES.*, USUARIOSgestor.nombre as USUARIOSgestornombre"
        Dim Table As String = "PAGOSOBSERVACIONES left join [USUARIOS] USUARIOSgestor on PAGOSOBSERVACIONES.gestor = USUARIOSgestor.codigo "

        Dim WhereClause As String = ""
        WhereClause = WhereClause & " and PAGOSOBSERVACIONES.NroExp = @NroExp"

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If

        Dim SortOrder As String = Session("PAGOSOBSERVACIONES.SortExpression") & " " & Session("PAGOSOBSERVACIONES.SortDirection")
        Dim sql As String = "WITH PAGOSOBSERVACIONESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " PAGOSOBSERVACIONESRecordCount AS ( SELECT * FROM PAGOSOBSERVACIONESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PAGOSOBSERVACIONESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PAGOSOBSERVACIONESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPAGOSOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente"))
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim IdUnico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditPAGOSOBSERVACIONES.aspx?ID=" & IdUnico & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PAGOSOBSERVACIONES.SortDirection"))
            Case "ASC"
                Session("PAGOSOBSERVACIONES.SortDirection") = "DESC"
            Case "DESC"
                Session("PAGOSOBSERVACIONES.SortDirection") = "ASC"
            Case Else
                Session("PAGOSOBSERVACIONES.SortDirection") = "ASC"
        End Select

        Session("PAGOSOBSERVACIONES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PAGOSOBSERVACIONES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PAGOSOBSERVACIONES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PAGOSOBSERVACIONES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If
        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PAGOSOBSERVACIONES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PAGOSOBSERVACIONES.CurrentPage") = Session("PAGOSOBSERVACIONES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PAGOSOBSERVACIONES.CurrentPage") > 1 Then
            Session("PAGOSOBSERVACIONES.CurrentPage") = Session("PAGOSOBSERVACIONES.CurrentPage") - 1
        Else
            Session("PAGOSOBSERVACIONES.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PAGOSOBSERVACIONES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Observaciones encontradas " & Session("PAGOSOBSERVACIONES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PAGOSOBSERVACIONES.CurrentPage") & " de " & GetPageCount()
    End Sub

End Class