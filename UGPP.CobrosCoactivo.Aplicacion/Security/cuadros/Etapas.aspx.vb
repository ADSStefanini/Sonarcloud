Imports System.Data.SqlClient
Partial Public Class Etapas
    Inherits System.Web.UI.Page


    Function Etapas() As DataTable
        Dim datata As New DatasetForm.etapasDataTable
        Dim myadapa As New SqlDataAdapter("SELECT * FROM ETAPAS", CadenaConexion)
        myadapa.Fill(datata)

        Me.ViewState("etapa") = datata
        Return datata
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GridEtapas.DataSource = Etapas()
            GridEtapas.DataBind()
        End If
    End Sub

    Protected Sub GridEtapas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridEtapas.SelectedIndexChanged
        Try
            With Me
                Dim Mytb As DatasetForm.etapasDataTable = CType(.ViewState("etapa"), DatasetForm.etapasDataTable)
                With Mytb.Item(.GridEtapas.SelectedIndex)
                    Dim dato As String = ""
                    dato = .nombre.Trim.ToUpper & "::" & .codigo.Trim
                    Response.Redirect("DependenciasActos.aspx?etapa=" & dato.Trim & "&textbox=" & Request.QueryString("textbox").Trim, True)
                End With
            End With
        Catch ex As Exception

        End Try
    End Sub

    'Private Sub Ejecutarjavascript(ByVal script As String)
    '    'Response.Write(vari)
    '    ' Define the name and type of the client scripts on the page. 
    '    Dim csname1 As [String] = "anything"
    '    Dim cstype As Type = Me.[GetType]()

    '    ' Get a ClientScriptManager reference from the Page class. 
    '    Dim cs As ClientScriptManager = Page.ClientScript

    '    If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
    '        Dim cstext1 As String = script
    '        cs.RegisterClientScriptBlock(cstype, csname1, cstext1.ToString())
    '    End If
    'End Sub
End Class