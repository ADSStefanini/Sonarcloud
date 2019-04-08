Imports System.Data.SqlClient
Partial Public Class detallespredio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Dim predio As String
            predio = Request.QueryString("predio")

            Call Datos_predio(predio)
        End If
    End Sub
    Private Sub Datos_predio(ByVal Predio As String)
        Dim Sql As String = "SELECT C.CARCOD,C.PRECARVAL,D.PREDIR,B.PREPRSDOC,B.PREPRSNOM,UPPER(E.CARDES) AS CARDES FROM PREDIOS2 C INNER JOIN PREDIOS D ON C.PRENUM = D.PRENUM INNER JOIN PREDIOS3 B ON B.PRENUM = C.PRENUM INNER JOIN CARACTER E ON E.CARCOD = C.CARCOD  WHERE C.PRENUM = @PRENUM AND C.MODCOD = 1 AND C.PERCOD = (SELECT MAX(PERCOD) AS PERCOD FROM PREDIOS2 WHERE PRENUM = @PRENUM AND MODCOD = 1)"
        Using dt As New SqlDataAdapter(Sql, Funciones.CadenaConexionUnion)
            dt.SelectCommand.Parameters.Add("@PRENUM", SqlDbType.VarChar).Value = Predio
            Dim tb As New DataTable
            dt.Fill(tb)
            forma(tb)
        End Using
    End Sub
    Private Sub forma(ByVal tb As DataTable)
        Dim rows As DataRow
        Dim forma_vista As String = ""
        predio_info.InnerHtml = "<table style='width:100%;margin:0;border-collapse:collapse;'><tr><td><b>" & Session("ssCampoClave").ToString.ToUpper & " : </b></td><td>" & Request.QueryString("predio") & "</td></tr><tr><td><b>DIRECCION : </b> </td><td>" & tb.Rows(0).Item("PREDIR").trim _
        & "</td></tr></table>" _
        & "<b style='font-size:xx-small;'>PROPIETARIO " & tb.Rows(0).Item("PREPRSNOM").ToString.ToUpper & " IDENTIFICADO CON CC/NIT " & tb.Rows(0).Item("PREPRSDOC") & " </b>"

        forma_vista += "<br /><hr style='background-color:#507cd1;color:#507cd1;height:1px;border:none;' /><table style='width:100%'>"
        For Each rows In tb.Rows
            forma_vista += "<tr><td style='color:#283e68;font-weight:bold;width:135px;'>" & rows("CARDES") & " :&nbsp;</td><td>" & rows("PRECARVAL") & "</td></tr>"
        Next
        forma_vista += "</table>"
        predio_info.InnerHtml += forma_vista
    End Sub
End Class