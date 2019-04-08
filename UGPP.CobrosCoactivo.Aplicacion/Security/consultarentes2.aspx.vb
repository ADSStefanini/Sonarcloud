Imports System.Data.SqlClient
Partial Public Class consultarentes2
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmd, CodigoEtapa, NombreEtapa As String
        Dim X As Integer
        'TipoConexion = ConfigurationManager.AppSettings("tipoconexion") 'local o web
        Dim SqlConnection1 As New SqlConnection(Funciones.CadenaConexion)
        'Creacion de objeto SQLCommand
        Dim oscmd As SqlClient.SqlCommand

        cmd = "SELECT codigo, nombre FROM etapas ORDER BY codigo"
        oscmd = New SqlClient.SqlCommand(cmd, SqlConnection1)

        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, SqlConnection1)
        oDtAdapterSql1.SelectCommand = oscmd

        'Creacion del DataSet
        Dim Dataset1 As DataSet = New DataSet
        oDtAdapterSql1.Fill(Dataset1, "etapas")
        Me.contenidogrids.InnerHtml = ""

        For X = 0 To Dataset1.Tables("etapas").Rows.Count - 1
            CodigoEtapa = Trim(Dataset1.Tables("etapas").Rows(X).Item("codigo"))
            NombreEtapa = CodigoEtapa & ". " & Dataset1.Tables("etapas").Rows(X).Item("nombre")
            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "<div class='contenedortitulos'>" + NombreEtapa + "<br /></div>"

            Dim Logic As Ejecucion_Actos = New Ejecucion_Actos(New SqlClient.SqlConnection(Funciones.CadenaConexion), CodigoEtapa, 1)
            Dim Table As DataTable = Logic.Proyeccion("200039570")

            Dim row As DataRow
            Dim col As DataColumn
            Dim Msg As String = ""

            If Table.Rows.Count > 0 Then
                Msg &= "<table>"
                For Each row In Table.Rows
                    Msg &= "<tr>"
                    For Each col In Table.Columns
                        If (col.ColumnName <> "DIAS") And (col.ColumnName <> "ENTIDAD") And (col.ColumnName <> "NOMBRE") Then
                            If col.ColumnName = "DESCRIPCION" Then
                                Msg &= "<td style=""font-size:11px;padding: .3em;border: 1px solid #4CAAF2;color: Black;"">" & ArmarHREF(row) & "</td>"
                            Else
                                Msg &= "<td style=""font-size:11px;padding: .3em;border: 1px solid #4CAAF2;color: Black;"">" & valorNull(row(col), "&nbsp") & "</td>"
                            End If
                        End If
                    Next
                    Msg &= "<tr>"
                Next
                Msg &= "</table>"
            Else
                Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + "No hay actos..."
            End If
            Me.contenidogrids.InnerHtml = Me.contenidogrids.InnerHtml + Msg
        Next
    End Sub
    Private Function ArmarHREF(ByVal row As DataRow) As String
        Dim deudor As String = row("entidad")
        Dim reto As String
        reto = "<a href=""javascript:;""  onclick=""window.open('TiffViewer.aspx?nomente=" & row("nombre") & "&idente=" & row("entidad") & "&F=" & row("NOMARCHIVO") & "&totimg=" & row("PAGINAS") & "&acto=" & row("DESCRIPCION") & "&idacto=" & row("ACTO") & "&folder=&Enabled=false', '', 'fullscreen=yes, scrollbars=auto')"">" & row("DESCRIPCION") & "<a>"
        Return Reto
    End Function
End Class