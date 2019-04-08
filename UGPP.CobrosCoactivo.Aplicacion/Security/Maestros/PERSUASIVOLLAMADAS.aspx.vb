Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class PERSUASIVOLLAMADAS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                txtSearchNoTelefono.Enabled = False
                txtSearchNombre.Enabled = False
                cmdSearch.Enabled = False
                cmdAddNew.Enabled = False
            Else
                'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
                txtSearchNoTelefono.Enabled = True
                txtSearchNombre.Enabled = True
                cmdSearch.Enabled = True
                cmdAddNew.Enabled = True
            End If

            txtSearchNoTelefono.Text = Session("PERSUASIVOLLAMADAS.txtSearchNoTelefono")
            txtSearchNombre.Text = Session("PERSUASIVOLLAMADAS.txtSearchNombre")
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Session("PERSUASIVOLLAMADAS.RecordsFound") = 0

        If Len(Session("PERSUASIVOLLAMADAS.CurrentPage")) = 0 Then
            Session("PERSUASIVOLLAMADAS.CurrentPage") = 1
        End If

        If Len(Session("PERSUASIVOLLAMADAS.SortExpression")) = 0 Then
            Session("PERSUASIVOLLAMADAS.SortExpression") = "IdUnico"
            Session("PERSUASIVOLLAMADAS.SortDirection") = "ASC"
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@NoTelefono", "%" & txtSearchNoTelefono.Text & "%")
        Command.Parameters.AddWithValue("@Nombre", "%" & txtSearchNombre.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PERSUASIVOLLAMADAS.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If

        If Session("PERSUASIVOLLAMADAS.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("PERSUASIVOLLAMADAS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "PERSUASIVOLLAMADAS.*, USUARIOSgestor.nombre as USUARIOSgestornombre, PERSUASIVOTIPIFICACION.nombre AS NomTipific"
        Dim Table As String = "PERSUASIVOLLAMADAS " & _
                                "LEFT JOIN USUARIOS USUARIOSgestor ON PERSUASIVOLLAMADAS.gestor       = USUARIOSgestor.codigo " & _
                                "LEFT JOIN PERSUASIVOTIPIFICACION  ON PERSUASIVOLLAMADAS.tipificacion = PERSUASIVOTIPIFICACION.codigo "

        Dim WhereClause As String = ""

        WhereClause = WhereClause & " and PERSUASIVOLLAMADAS.NroExp = @NroExp"

        If txtSearchNoTelefono.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOLLAMADAS.NoTelefono LIKE @NoTelefono"
        End If

        If txtSearchNombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and PERSUASIVOLLAMADAS.Nombre LIKE @Nombre"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If

        Dim SortOrder As String = Session("PERSUASIVOLLAMADAS.SortExpression") & " " & Session("PERSUASIVOLLAMADAS.SortDirection")
        Dim sql As String = "WITH PERSUASIVOLLAMADASRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " WHERE " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " PERSUASIVOLLAMADASRecordCount AS ( SELECT * FROM PERSUASIVOLLAMADASRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PERSUASIVOLLAMADASRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PERSUASIVOLLAMADASRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql
    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPERSUASIVOLLAMADAS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("PERSUASIVOLLAMADAS.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("PERSUASIVOLLAMADAS.txtSearchNoTelefono") = txtSearchNoTelefono.Text
        Session("PERSUASIVOLLAMADAS.txtSearchNombre") = txtSearchNombre.Text
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim IdUnico As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditPERSUASIVOLLAMADAS.aspx?ID=" & IdUnico & "&pExpediente=" & Request("pExpediente"))
            End If
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PERSUASIVOLLAMADAS.SortDirection"))
            Case "ASC"
                Session("PERSUASIVOLLAMADAS.SortDirection") = "DESC"
            Case "DESC"
                Session("PERSUASIVOLLAMADAS.SortDirection") = "ASC"
            Case Else
                Session("PERSUASIVOLLAMADAS.SortDirection") = "ASC"
        End Select

        Session("PERSUASIVOLLAMADAS.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PERSUASIVOLLAMADAS.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PERSUASIVOLLAMADAS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PERSUASIVOLLAMADAS.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If
        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PERSUASIVOLLAMADAS.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PERSUASIVOLLAMADAS.CurrentPage") = Session("PERSUASIVOLLAMADAS.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PERSUASIVOLLAMADAS.CurrentPage") > 1 Then
            Session("PERSUASIVOLLAMADAS.CurrentPage") = Session("PERSUASIVOLLAMADAS.CurrentPage") - 1
        Else
            Session("PERSUASIVOLLAMADAS.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PERSUASIVOLLAMADAS.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Registros encontrados " & Session("PERSUASIVOLLAMADAS.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PERSUASIVOLLAMADAS.CurrentPage") & " de " & GetPageCount()
    End Sub

End Class