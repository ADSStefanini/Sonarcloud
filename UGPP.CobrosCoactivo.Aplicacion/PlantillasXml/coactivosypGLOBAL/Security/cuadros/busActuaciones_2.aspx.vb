Public Partial Class busActuaciones_2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim vari As String = ""
        vari = Request.QueryString("tiparch")

        Dim cnn As New System.Data.SqlClient.SqlConnection
        Dim mytb As New DataTable

        cnn.ConnectionString = "Password=123456;Persist Security Info=True;User ID=sa;Initial Catalog=bktecnoexpbd;Data Source=PC-LED\SQLSERVER2008"

        Dim myadapter As New System.Data.SqlClient.SqlDataAdapter("SELECT actuaciones.codigo as codigo ,actuaciones.nombre as nombre,idetapa,etapas.nombre as etnom FROM actuaciones,etapas where actuaciones.idetapa = etapas.codigo and actuaciones.nombre LIKE '%' + @varia + '%'", cnn)
        myadapter.SelectCommand.Parameters.Add("@varia", SqlDbType.VarChar)
        myadapter.SelectCommand.Parameters("@varia").Value = vari
        myadapter.Fill(mytb)

        GridView1.DataSource = mytb
        GridView1.DataBind()

        Me.ViewState("etapas") = mytb
        Control.Value = Request.QueryString("textbox")
    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged
        Dim mytb As DataTable = CType(Me.ViewState("etapas"), DataTable)
        Dim indexp As String = GridView1.SelectedIndex

        'Dim dato As String = ""
        'Dim termino As String
        'Dim tipotermino As String
        ''txtBuscar
        'dato = .DEP_CONMOV
        'termino = .DEP_TERMINO
        'tipotermino = .DEP_TIPOTERMINO
        'Dim strScript As String = "<script>window.opener.document.forms(0)." + Control.Value + ".value = '"
        'strScript += dato.Trim
        'strScript += "';window.opener.document.forms(0).txtBuscar.value = '"
        'strScript += Request.QueryString("etapa")
        'strScript += "';self.close()"
        'strScript += "';window.opener.document.location.href = '../configuracionActos.aspx?texto=" & dato.Trim & "&etapa=" & Request.QueryString("etapa") & "&termino=" & termino & "&tipotermino" & tipotermino & "';"
        'strScript += "</" + "script>"

        ''Dim strScript As String = "<script>window.opener.document.forms(0)." + control.Value + ".value = '"
        ''strScript += dato.Trim
        ' ''strScript += "';self.close()"
        ''strScript += "';document.location.href = '../configuracionActos.aspx';"
        ''strScript += "</" + "script>"

        'Ejecutarjavascript(strScript)
    End Sub
End Class