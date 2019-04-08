Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class PERSUASIVOOBSERVACIONES
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilita el botón Adicionar para este perfil
                cmdAddNew.Enabled = False
            Else
                'Se habilita el botón Adicionar para usuarios con perfil diferente a Verificador de Pagos
                cmdAddNew.Enabled = True
            End If

            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Session("PERSUASIVOOBSERVACIONES.RecordsFound") = 0

        If Len(Session("PERSUASIVOOBSERVACIONES.CurrentPage")) = 0 Then
            Session("PERSUASIVOOBSERVACIONES.CurrentPage") = 1
        End If

        If Len(Session("PERSUASIVOOBSERVACIONES.SortExpression")) = 0 Then
            Session("PERSUASIVOOBSERVACIONES.SortExpression") = "IdUnico"
            Session("PERSUASIVOOBSERVACIONES.SortDirection") = "ASC"
        End If

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("PERSUASIVOOBSERVACIONES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If

        If Session("PERSUASIVOOBSERVACIONES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("PERSUASIVOOBSERVACIONES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "PERSUASIVOOBSERVACIONES.*, USUARIOSgestor.nombre as USUARIOSgestornombre"
        Dim Table As String = "PERSUASIVOOBSERVACIONES left join [USUARIOS] USUARIOSgestor on PERSUASIVOOBSERVACIONES.gestor = USUARIOSgestor.codigo "

        Dim WhereClause As String = ""
        WhereClause = WhereClause & " and PERSUASIVOOBSERVACIONES.NroExp = @NroExp"

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If

        Dim SortOrder As String = Session("PERSUASIVOOBSERVACIONES.SortExpression") & " " & Session("PERSUASIVOOBSERVACIONES.SortDirection")
        Dim sql As String = "WITH PERSUASIVOOBSERVACIONESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " PERSUASIVOOBSERVACIONESRecordCount AS ( SELECT * FROM PERSUASIVOOBSERVACIONESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM PERSUASIVOOBSERVACIONESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM PERSUASIVOOBSERVACIONESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql
    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPERSUASIVOOBSERVACIONES.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim IdUnico As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditPERSUASIVOOBSERVACIONES.aspx?ID=" & IdUnico & "&pExpediente=" & Request("pExpediente"))
            End If
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("PERSUASIVOOBSERVACIONES.SortDirection"))
            Case "ASC"
                Session("PERSUASIVOOBSERVACIONES.SortDirection") = "DESC"
            Case "DESC"
                Session("PERSUASIVOOBSERVACIONES.SortDirection") = "ASC"
            Case Else
                Session("PERSUASIVOOBSERVACIONES.SortDirection") = "ASC"
        End Select

        Session("PERSUASIVOOBSERVACIONES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("PERSUASIVOOBSERVACIONES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()
        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("PERSUASIVOOBSERVACIONES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("PERSUASIVOOBSERVACIONES.RecordsFound") Mod PageSize

        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("PERSUASIVOOBSERVACIONES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("PERSUASIVOOBSERVACIONES.CurrentPage") = Session("PERSUASIVOOBSERVACIONES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("PERSUASIVOOBSERVACIONES.CurrentPage") > 1 Then
            Session("PERSUASIVOOBSERVACIONES.CurrentPage") = Session("PERSUASIVOOBSERVACIONES.CurrentPage") - 1
        Else
            Session("PERSUASIVOOBSERVACIONES.CurrentPage") = 1
        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("PERSUASIVOOBSERVACIONES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Observaciones encontradas " & Session("PERSUASIVOOBSERVACIONES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("PERSUASIVOOBSERVACIONES.CurrentPage") & " de " & GetPageCount()
    End Sub

End Class