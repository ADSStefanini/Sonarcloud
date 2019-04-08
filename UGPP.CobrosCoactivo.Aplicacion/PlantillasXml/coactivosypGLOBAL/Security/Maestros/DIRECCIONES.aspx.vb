Imports System.Data
Imports System.Data.SqlClient

Partial Public Class DIRECCIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            BindGrid()

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos                       
            If NomEstadoProceso = "DEVUELTO" Or NomEstadoProceso = "TERMINADO" Then
                cmdAddNew.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar datos"
                CustomValidator1.IsValid = False
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

        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        If Len(Request("ID")) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "select DIRECCIONES.*, DEPARTAMENTOSDepartamento.nombre as DEPARTAMENTOSDepartamentonombre, " & _
                    "MUNICIPIOSCiudad.nombre as MUNICIPIOSCiudadnombre from (DIRECCIONES " & _
                    "left join [DEPARTAMENTOS] DEPARTAMENTOSDepartamento on [dbo].[DIRECCIONES].Departamento = DEPARTAMENTOSDepartamento.codigo )  " & _
                    "left join [MUNICIPIOS] MUNICIPIOSCiudad on [dbo].[DIRECCIONES].Ciudad = MUNICIPIOSCiudad.codigo " & _
                    "WHERE direcciones.deudor = @deudor"
            Dim Command As New SqlCommand()
            Command.Connection = Connection
            Command.CommandText = sql
            Command.Parameters.AddWithValue("@deudor", Request("ID").Trim)
            grd.DataSource = Command.ExecuteReader()
            grd.DataBind()
            lblRecordsFound.Text = "Direcciones encontradas " & grd.Rows.Count
            'Close the Connection Object 
            Connection.Close()
        End If
    End Sub

    'Adicionar
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Ir a la pagina de edicion
        Response.Redirect("EditDIRECCIONES.aspx?ID=" & Request("ID") & "&pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim deudor As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim Iddireccion As String = grd.Rows(e.CommandArgument).Cells(8).Text
            Response.Redirect("EditDIRECCIONES.aspx?ID=" & deudor & "&pIdUnico=" & Iddireccion & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub
End Class