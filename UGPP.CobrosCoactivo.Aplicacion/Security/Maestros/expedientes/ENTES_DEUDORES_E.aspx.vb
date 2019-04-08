Imports System.Data.SqlClient

Partial Public Class ENTES_DEUDORES_E
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            ' mnivelacces = 5 Es repartidor
            If Session("mnivelacces") = 5 Then
                ' Si es repartidor y está adiciondo un expediente=>Impedir ingresar deudores, ya que no se pueden asociar a un expediente inexistente
                'If ModoAddEditRepartidor = "ADICIONAR" Then
                '    cmdAddNew.Visible = False
                'End If
            End If

            txtSearchED_Codigo_Nit.Text = Session("ENTES_DEUDORES.txtSearchED_Codigo_Nit")
            txtSearchED_Nombre.Text = Session("ENTES_DEUDORES.txtSearchED_Nombre")
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

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            If Session("mnivelacces") <> 5 And Session("mnivelacces") <> 8 Then
                Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
                If idGestorResp <> Session("sscodigousuario") Then
                    cmdAddNew.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If
            End If

            If Session("mnivelacces") <> 11 Then ' solo area origen peude agregar deudores
                cmdAddNew.Visible = False
            End If
        End If

    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Session("ENTES_DEUDORES.RecordsFound") = 0
        If Len(Session("ENTES_DEUDORES.CurrentPage")) = 0 Then
            Session("ENTES_DEUDORES.CurrentPage") = 1
        End If
        If Len(Session("ENTES_DEUDORES.SortExpression")) = 0 Then
            Session("ENTES_DEUDORES.SortExpression") = "ED_Codigo_Nit"
            Session("ENTES_DEUDORES.SortDirection") = "ASC"
        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@ED_Codigo_Nit", "%" & txtSearchED_Codigo_Nit.Text)
        Command.Parameters.AddWithValue("@ED_Nombre", "%" & txtSearchED_Nombre.Text & "%")

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

        cmdFirst.Enabled = True
        cmdPrevious.Enabled = True
        cmdNext.Enabled = True
        cmdLast.Enabled = True

        If Session("ENTES_DEUDORES.CurrentPage") = "1" Then
            cmdFirst.Enabled = False
            cmdPrevious.Enabled = False
        End If
        If Session("ENTES_DEUDORES.CurrentPage") = GetPageCount() Then
            cmdNext.Enabled = False
            cmdLast.Enabled = False
        End If
    End Sub

    Private Function GetSQL() As String
        'Parametro del numero del expediente
        Dim pExpediente As String = ""
        If Len(Request("pExpediente")) > 0 Then
            pExpediente = Request("pExpediente").Trim
        End If

        Dim pTipo As String = ""
        pTipo = Request("pTipo")

        Dim StartRecord As Long = PageSize * Session("ENTES_DEUDORES.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim Columns As String = "[DEUDORES].*"
        Dim Table As String = "[DEUDORES]"
        Dim WhereClause As String = ""


        WhereClause = WhereClause & " and DEUDORES.EFINROEXP = '" & pExpediente & "'"

        If pTipo = "1" Or pTipo = "2" Then
            WhereClause = WhereClause & " and (DEUDORES.tipo = 1 OR DEUDORES.tipo = 2) and ED_Codigo_Nit IN (SELECT DxE.deudor FROM DEUDORES_EXPEDIENTES DxE WHERE DxE.NroExp = '" & pExpediente & "' AND (DxE.tipo = '1' OR DxE.tipo = '2'))"
        Else
            WhereClause = WhereClause & " and (DEUDORES.tipo = " & pTipo & ") and ED_Codigo_Nit IN (SELECT DxE.deudor FROM DEUDORES_EXPEDIENTES DxE WHERE DxE.NroExp = '" & pExpediente & "' AND DxE.tipo = '" & pTipo & "')"
        End If
        'End If

        If txtSearchED_Codigo_Nit.Text.Length > 0 Then
            WhereClause = WhereClause & " and [DEUDORES].[ED_Codigo_Nit] like @ED_Codigo_Nit"
        End If

        If txtSearchED_Nombre.Text.Length > 0 Then
            WhereClause = WhereClause & " and [DEUDORES].[ED_Nombre] like @ED_Nombre"
        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
        End If
        Dim SortOrder As String = Session("ENTES_DEUDORES.SortExpression") & " " & Session("ENTES_DEUDORES.SortDirection")
        Dim sql As String = "WITH ENTES_DEUDORESRecordSet AS ( SELECT ROW_NUMBER() OVER (ORDER BY " & SortOrder & ") AS RecordSetID, " & Columns & " FROM " & Table
        If Len(WhereClause) > 0 Then
            sql = sql & " where " & WhereClause
        End If
        sql = sql & " ),"
        sql = sql & " ENTES_DEUDORESRecordCount AS ( SELECT * FROM ENTES_DEUDORESRecordSet, (SELECT MAX(RecordSetID) AS RecordSetCount FROM ENTES_DEUDORESRecordSet) AS RC ) "
        sql = sql & "SELECT * FROM ENTES_DEUDORESRecordCount WHERE RecordSetID >= " & StartRecord & " AND RecordSetID < " & StopRecord
        Return sql

    End Function

    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditENTES_DEUDORES_E.aspx?pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Session("ENTES_DEUDORES.CurrentPage") = 1
        BindGrid()

        UpdateLabels()

        Session("ENTES_DEUDORES.txtSearchED_Codigo_Nit") = txtSearchED_Codigo_Nit.Text
        Session("ENTES_DEUDORES.txtSearchED_Nombre") = txtSearchED_Nombre.Text
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim ED_Codigo_Nit As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim NomTipoDeudor As String = grd.Rows(e.CommandArgument).Cells(5).Text
            '26/08/2014
            ED_Codigo_Nit = ED_Codigo_Nit.Replace("&#160;", " ").Trim
            NomTipoDeudor = NomTipoDeudor.Replace("&#160;", " ")
            '----------------------------------------------------
            Response.Redirect("EditENTES_DEUDORES_E.aspx?ID=" & ED_Codigo_Nit & "&pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr") & "&pNomTipoDeudor=" & NomTipoDeudor)
        End If
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("ENTES_DEUDORES.SortDirection"))
            Case "ASC"
                Session("ENTES_DEUDORES.SortDirection") = "DESC"
            Case "DESC"
                Session("ENTES_DEUDORES.SortDirection") = "ASC"
            Case Else
                Session("ENTES_DEUDORES.SortDirection") = "ASC"
        End Select

        Session("ENTES_DEUDORES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Session("ENTES_DEUDORES.RecordsFound") = grd.DataSource("RecordSetCount")
            UpdateLabels()

        End If
    End Sub

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("ENTES_DEUDORES.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("ENTES_DEUDORES.RecordsFound") Mod PageSize
        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If
        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        Session("ENTES_DEUDORES.CurrentPage") = 1
        BindGrid()
    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Session("ENTES_DEUDORES.CurrentPage") = Session("ENTES_DEUDORES.CurrentPage") + 1
        BindGrid()
    End Sub

    Protected Sub cmdPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrevious.Click
        If Session("ENTES_DEUDORES.CurrentPage") > 1 Then
            Session("ENTES_DEUDORES.CurrentPage") = Session("ENTES_DEUDORES.CurrentPage") - 1
        Else
            Session("ENTES_DEUDORES.CurrentPage") = 1

        End If
        BindGrid()
    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        Session("ENTES_DEUDORES.CurrentPage") = GetPageCount()
        BindGrid()
    End Sub

    Protected Sub UpdateLabels()
        lblRecordsFound.Text = "Deudores encontrados " & Session("ENTES_DEUDORES.RecordsFound")
        lblPageNumber.Text = "Página " & Session("ENTES_DEUDORES.CurrentPage") & " de " & GetPageCount()
    End Sub

    Protected Sub grd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grd.SelectedIndexChanged

    End Sub
End Class