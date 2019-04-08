Imports System.Data.SqlClient
Partial Public Class Expedientes
    Inherits System.Web.UI.Page

    Function documento(ByVal deudor As String) As DataTable
        Dim datata As New DataTable
        Dim myadapa As New SqlDataAdapter("select distinct (docexpediente) as docexpediente from documentos where entidad = @entidad", Funciones.CadenaConexion)
        myadapa.SelectCommand.Parameters.Add("@entidad", SqlDbType.VarChar).Value = deudor
        myadapa.Fill(datata)

        Me.ViewState("documento") = datata
        Return datata
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Dim control, Deudor As String
            control = Request.QueryString("textbox").Trim
            Deudor = Request.QueryString("deudor").Trim
            pp.InnerHtml = "<b>" & Deudor & "</b>"
            Deudor = Mid(Deudor.Trim(), Deudor.IndexOf(":") + 3)

            Dim tb As DataTable = documento(Deudor)
            If tb.Rows.Count = 0 Then
                Div1.InnerHtml = "Digite un deudor para continuar"
                Exit Sub
            Else
                Div1.InnerHtml = "<b>EXPEDINETES : " & tb.Rows.Count & "</b>"
            End If

            Gridexpedinete.DataSource = tb
            Gridexpedinete.DataBind()
        End If
    End Sub

    Protected Sub Gridexpedinete_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Gridexpedinete.SelectedIndexChanged
        Try
            With Me
                Dim Mytb As DataTable = CType(.ViewState("documento"), DataTable)
                'With Mytb.Item(.Gridexpedinete.SelectedIndex)

                Dim dato As String = ""
                dato = Mytb.Rows(.Gridexpedinete.SelectedIndex).Item("docexpediente")

                Dim strScript As String = "<script>window.opener.document.forms(0)." + Request.QueryString("textbox").Trim + ".value = '"
                strScript += dato.Trim
                strScript += "';self.close();"
                strScript += "</" + "script>"

                Ejecutarjavascript(strScript)
                'End With
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub Ejecutarjavascript(ByVal script As String)
        'Response.Write(vari)
        ' Define the name and type of the client scripts on the page. 
        Dim csname1 As [String] = "anything"
        Dim cstype As Type = Me.[GetType]()

        ' Get a ClientScriptManager reference from the Page class. 
        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterClientScriptBlock(cstype, csname1, cstext1.ToString())
        End If
    End Sub
End Class