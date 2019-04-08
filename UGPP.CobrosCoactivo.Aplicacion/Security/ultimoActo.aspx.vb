Imports System.Data.SqlClient
Partial Public Class ultimoActo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("mcobrador") = "01"
    End Sub

    Protected Sub btnUltimoActo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUltimoActo.Click
        Using con As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim DatosConsultasTablas As New DatosConsultasTablas
            Dim xdatatable As New DataTable
            xdatatable = DatosConsultasTablas.Preparar_expedientes(Session("mcobrador"), con)
            Dim x As Integer
            Dim msn As String = ""

            For x = 0 To xdatatable.Rows.Count - 1
                Dim expedientet As String = xdatatable.Rows(x).Item("docexpediente")
                Dim Logic As Ejecucion_Actos = New Ejecucion_Actos(con, "02", 1)
                Dim Table As DataTable = Logic.Proyeccion(expedientet)

                Dim row As DataRow
                'Dim col As DataColumn
                Dim ultimopaso As String = ""
                Dim despcricio As String = ""
                Dim deudor As String = ""
                If Table.Rows.Count > 0 Then
                    For Each row In Table.Rows
                        If ChekaNull(row("ACTO")) = False Then
                            ultimopaso = row("ACTO")
                            despcricio = row("DESCRIPCION")
                            deudor = row("ENTIDAD").Equals(Table)
                        End If
                    Next
                    Call DatosConsultasTablas.InsertaUltimo_Acto(expedientet, ultimopaso, despcricio, Date.Now, deudor)
                Else
                    ' msn = msn + xdatatable.Rows(x).Item("docexpediente") + "<br />"
                End If
            Next
            con.Close()
        End Using
    End Sub
End Class