Public Partial Class expedinteexaminar
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim Control As String
            Control = Request.QueryString("expediente")
            Dim refcata As String
            refcata = Request.QueryString("refcata")

            Dim Logic As Ejecucion_Expediente_examinar = New Ejecucion_Expediente_examinar(New SqlClient.SqlConnection(Funciones.CadenaConexion), "02")
            Dim Table As DataTable = Logic.ExamenCompleto(Control)

            If Table.Rows.Count = 0 Then
                'No Trajo datos la aplicacion (expediente)
                NoExpediente(Control, refcata)
                Exit Sub
            Else

            End If

            Me.Repeater1.DataSource = Table
            Me.Repeater1.DataBind()

            If Table.Rows.Count > 0 Then
                Me.ViewState("entidad") = Table.Rows(0).Item("entidad")
                Me.ViewState("nombre") = Table.Rows(0).Item("nombre")
                Me.ViewState("docexpediente") = Table.Rows(0).Item("docexpediente")
                Me.ViewState("nroeje") = Table.Rows.Count
            End If
        End If
    End Sub

    Private Sub NoExpediente(ByVal expediente As String, ByVal refcata As String)
        Try
            datosinf.Attributes.Add("style", "display:none;")
            Dim htmlH2 As New Global.System.Web.UI.HtmlControls.HtmlGenericControl
            htmlH2.Attributes.Add("class", "dft")
            htmlH2.InnerHtml = "<font size='4'><b>Expedinte: " & Request.QueryString("expediente").ToString() & "</b></font> <hr /><br /><b> Este expediente virtual no está digitalizado. </b> <br />" _
                & "Nota : para poder visualizar o analizar este expediente deberá escanear el documento físico y después guardar los archivos con tecno expedientes."

            If refcata <> Nothing Then
                Dim Adapter As New SqlClient.SqlDataAdapter("select distinct entidad,upper(nombre) as nombre ,docexpediente from documentos,entesdbf where docpredio_refecatrastal = @refcata and (entidad = codigo_nit)", Funciones.CadenaConexion)
                Adapter.SelectCommand.Parameters.Add("@refcata", SqlDbType.VarChar)
                Adapter.SelectCommand.Parameters("@refcata").Value = refcata
                Dim mytable As New DataTable
                Adapter.Fill(mytable)
                If mytable.Rows.Count > 0 Then
                    Dim x As Integer
                    htmlH2.InnerHtml = htmlH2.InnerHtml + "<br /><br /> <font size='4'><b>Predio: " & Request.QueryString("refcata").ToString() & "</b></font>" _
                    & "<br />Se detectaron uno o varios expedientes asociados a este predio."
                    Dim ul As String = "<table width='100%' class='dteall'><tr><th>ID Deudor</th><th>Nombre</th><th>Expedinete</th></tr>"
                    For x = 0 To mytable.Rows.Count - 1
                        ul = ul + "<tr><td>" + mytable.Rows(x).Item("entidad") + "</td><td>" + mytable.Rows(x).Item("nombre") + "</td><td>" + mytable.Rows(x).Item("docexpediente") + "</td></tr>"
                    Next
                    ul = ul + "</table>"
                    htmlH2.InnerHtml = htmlH2.InnerHtml + ul
                    htmlH2.InnerHtml = htmlH2.InnerHtml + "<br />" _
                    & "Nota: el software para facilitar el trabajo y procesos intento escanear uno a varios expedientes  para una mejor auditoria."
                Else
                    htmlH2.InnerHtml = htmlH2.InnerHtml + "<br /><br /> <font size='4'><b>Predio: " & Request.QueryString("refcata").ToString() & "</b></font><hr /><br /><b> No se detectaron expedientes asociados a este predio</b> <br />" _
                    & "Nota: el software para facilitar el trabajo y procesos intento escanear uno a varios expedientes  para una mejor auditoria."
                End If
            Else

            End If
         
            form1.Controls.Add(htmlH2)
        Catch ex As Exception
            Response.Write("Error : " & ex.Message)
        End Try
    End Sub

End Class