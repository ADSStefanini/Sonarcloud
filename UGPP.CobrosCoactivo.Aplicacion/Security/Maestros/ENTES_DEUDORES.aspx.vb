Imports coactivosyp.My.Resources
Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class ENTES_DEUDORES
    Inherits PaginaBase

    ''' <summary>
    ''' Se declaran las variables 
    ''' </summary>
    Dim tareaAsignadaObject As TareaAsignada
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL
    ''' <summary>
    ''' se inicializan
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnInit(e As EventArgs)
        tareaAsignadaBLL = New TareaAsignadaBLL()
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL()
    End Sub
    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            ' mnivelacces = 5 Es repartidor
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.REPARTIDOR) Then
                '    Si es repartidor y está adiciondo un expediente=>Impedir ingresar deudores, ya que no se pueden asociar a un expediente inexistente
                'If ModoAddEditRepartidor = "ADICIONAR" Then
                '    cmdAddNew.Visible = False
                'End If
                ''TODO Revisar a que modulo pertenecera esta pagina
                nombreModulo = "Deudor"
            ElseIf Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilitan los campos para este perfil
                txtSearchED_Codigo_Nit.Enabled = False
                txtSearchED_Nombre.Enabled = False
                cmdSearch.Enabled = False
                cmdAddNew.Enabled = False
                ''TODO Revisar a que modulo pertenecera esta pagina
                nombreModulo = "Deudor"
            Else
                'Se habilitan los campos para usuarios con perfil diferente a Verificador de Pagos
                txtSearchED_Codigo_Nit.Enabled = True
                txtSearchED_Nombre.Enabled = True
                cmdSearch.Enabled = True
                cmdAddNew.Enabled = True
                ''TODO Revisar a que modulo pertenecera esta pagina
                nombreModulo = "Deudor"
            End If

            txtSearchED_Codigo_Nit.Text = Session("ENTES_DEUDORES.txtSearchED_Codigo_Nit")
            txtSearchED_Nombre.Text = Session("ENTES_DEUDORES.txtSearchED_Nombre")
            If Session("mnivelacces") <> 11 Then ' solo area origen peude agregar deudores
                'Se oculta el botón de agregar deudor para cualquier perfil diferente a área origen
                cmdAddNew.Enabled = False
                cmdAddNew.Visible = False
                ''TODO Revisar a que modulo pertenecera esta pagina
                nombreModulo = "Deudor"
            End If
            BindGrid()

        End If

    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        lblMensaje.Text =String.Empty
        Session("ENTES_DEUDORES.RecordsFound") = 0

        If Len(Session("ENTES_DEUDORES.SortExpression")) = 0 Then
            Session("ENTES_DEUDORES.SortExpression") = "ED_Codigo_Nit"
            Session("ENTES_DEUDORES.SortDirection") = "ASC"
        End If

        If Len(Request("ID_TASK")) > 0 Then
            Dim tituloEjecutivoObj As TituloEjecutivoExt
            HdnIdTask.Value = Request("ID_TASK")
            'Si ID_TASK tiene valor se carga valida el item
            tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(Request("ID_TASK").ToString()))

            'If tareaAsignadaObject.ID_UNICO_TITULO IsNot Nothing Then
            '    HdnIdunico.Value = tareaAsignadaObject.ID_UNICO_TITULO
            '    Dim MetodoSel As ValidadorBLL = New ValidadorBLL()
            '    tituloEjecutivoObj = MetodoSel.ConsultarTituloEjecutivo(HdnIdunico.Value)
            'Else
            Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
            tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
            'End If
            Dim lstDeudoresFiltrados As List(Of Deudor) = New List(Of Deudor)

            If String.IsNullOrEmpty(txtSearchED_Nombre.Text) = False Then
                lstDeudoresFiltrados.AddRange(tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.nombreDeudor.ToUpper().Contains(txtSearchED_Nombre.Text.ToUpper()))).ToList())
            End If

            If String.IsNullOrEmpty(txtSearchED_Codigo_Nit.Text) = False Then
                lstDeudoresFiltrados.AddRange(tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.numeroIdentificacion.ToUpper().Contains(txtSearchED_Codigo_Nit.Text.ToUpper()))).ToList())
            End If
            Session("ENTES_DEUDORES.txtSearchED_Codigo_Nit") = txtSearchED_Codigo_Nit.Text
            Session("ENTES_DEUDORES.txtSearchED_Nombre") = txtSearchED_Nombre.Text

            If lstDeudoresFiltrados.Count() > 0 Then
                grd.DataSource = ResetIdentificacionDeudores(lstDeudoresFiltrados)

            Else
                If String.IsNullOrEmpty(txtSearchED_Nombre.Text) = False Or String.IsNullOrEmpty(txtSearchED_Codigo_Nit.Text) = False Then
                    lblMensaje.Text = StringsResourse.MsgMensajeDeudor
                End If

                If Not IsNothing(tituloEjecutivoObj) Then
                    grd.DataSource = ResetIdentificacionDeudores(tituloEjecutivoObj.LstDeudores)
                End If

            End If
            grd.DataBind()

        End If



    End Sub

    ''' <summary>
    ''' Obtiene los tipos de identificacion segun su código
    ''' </summary>
    ''' <param name="listDeudores"></param>
    ''' <returns>Lista de deudores</returns>
    Private Function ResetIdentificacionDeudores(ByVal listDeudores As List(Of Deudor)) As List(Of Deudor)
        Dim dataTipo As New DocumentoTituloTipoTituloBLL
        Dim listTipos As New List(Of TipoIdentificacion)
        listTipos = dataTipo.obtenerTiposIdentificaciones
        For Each deudor In listDeudores
            If Not String.IsNullOrEmpty(deudor.tipoIdentificacion) Then
                If deudor.tipoIdentificacion <> "00" Then
                    deudor.tipoIdentificacion = listTipos.Where(Function(x) x.codigo = deudor.tipoIdentificacion).FirstOrDefault().nombre
                Else
                    deudor.tipoIdentificacion = "Sin asignar"
                End If
            End If

        Next
        Return listDeudores
    End Function

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
        Response.Redirect("EditENTES_DEUDORES.aspx?ID_TASK=" & HdnIdTask.Value & "&pScr=" & Request("pScr"))
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim ED_Codigo_Nit As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Dim NomTipoDeudor As String = grd.Rows(e.CommandArgument).Cells(5).Text
                Dim Tipo As Int32 = grd.Rows(e.CommandArgument).Cells(9).Text
                '26/08/2014
                ED_Codigo_Nit = ED_Codigo_Nit.Replace("&#160;", " ").Trim
                NomTipoDeudor = NomTipoDeudor.Replace("&#160;", " ")
                '----------------------------------------------------Request("pTipo")
                Response.Redirect("EditENTES_DEUDORES.aspx?ID_TASK=" & HdnIdTask.Value & "&pTipo=" & Tipo & "&IdDeudor=" & ED_Codigo_Nit & "&pScr=" & Request("pScr") & "&pNomTipoDeudor=" & NomTipoDeudor)
            End If
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
            If grd.Rows.Count() > 0 Then
                Session("ENTES_DEUDORES.RecordsFound") = grd.DataSource("RecordSetCount")
            End If

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

End Class