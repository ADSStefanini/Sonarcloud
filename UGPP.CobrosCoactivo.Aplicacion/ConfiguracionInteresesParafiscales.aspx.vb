Public Partial Class ConfiguracionInteresesParafiscales
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            'Puts the previous state of the txtSearchdesde field done when the user has searched and moved to the EditCALCULO_INTERESES_PARAFISCALES page and then came back
            txtSearchdesde.Text = Session("CALCULO_INTERESES_PARAFISCALES.txtSearchdesde")
            BindGrid()
            LoadThemes()

            'End If - if IsPostBack equals false
        End If
    End Sub

    Private Sub LoadThemes()
        Dim ThemeName As String = "base"
        If IsNothing(Request.Cookies("PCGSettings")) = False Then
            If IsNothing(Request.Cookies("PCGSettings")("Themes")) = False Then
                ThemeName = Request.Cookies("PCGSettings")("Themes")
                cboTheme.SelectedValue = ThemeName

            End If

        End If

        ThemesCSS.Attributes.Add("href", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/themes/" & ThemeName & "/ui.all.css")
    End Sub

    Private Sub SaveThemesCookie(ByVal ThemeName As String)
        Dim Cookie As New HttpCookie("PCGSettings")
        Cookie("Themes") = ThemeName
        Cookie.Expires = Now.AddYears(1)
        Response.Cookies.Add(Cookie)
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        Command.Parameters.AddWithValue("@desde", txtSearchdesde.Text)

        Dim DataAdapter As New SqlDataAdapter(Command)

        Dim DataSet As New DataSet

        DataAdapter.Fill(DataSet)
        grd.DataSource = DataSet
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim sql As String = ""
        sql = sql & "select [dbo].[CALCULO_INTERESES_PARAFISCALES].* from [dbo].[CALCULO_INTERESES_PARAFISCALES]"
        Dim WhereClause As String = ""
        If txtSearchdesde.Text.Length > 0 Then
            WhereClause = WhereClause & " and [dbo].[CALCULO_INTERESES_PARAFISCALES].[desde] = @desde"

        End If

        If WhereClause.Length > 0 Then
            WhereClause = Replace(WhereClause, " and ", "", , 1)
            sql = sql & "where " & WhereClause

        End If

        If Len(Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortExpression")) > 0 Then
            sql = sql & " order by " & Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortExpression") & " " & Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection")

        End If
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditCALCULO_INTERESES_PARAFISCALES.aspx
        Response.Redirect("EditCALCULOUnderScoreINTERESESUnderScorePARAFISCALES.aspx")
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()

        Session("CALCULO_INTERESES_PARAFISCALES.txtSearchdesde") = txtSearchdesde.Text
    End Sub


    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim consec As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditCALCULOUnderScoreINTERESESUnderScorePARAFISCALES.aspx?ID=" & consec)

        End If
    End Sub

    Protected Sub grd_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grd.PageIndexChanging
        grd.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Protected Sub grd_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grd.Sorting

        Select Case CStr(Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection"))
            Case "ASC"
                Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection") = "DESC"
            Case "DESC"
                Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection") = "ASC"
            Case Else
                Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortDirection") = "ASC"
        End Select

        Session("CALCULOUnderScoreINTERESESUnderScorePARAFISCALES.SortExpression") = e.SortExpression

        BindGrid()

    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub

    Protected Sub cboTheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTheme.SelectedIndexChanged
        ThemesCSS.Attributes.Add("href", "http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/themes/" & cboTheme.SelectedValue & "/ui.all.css")
        SaveThemesCookie(cboTheme.SelectedValue)
        BindGrid()
    End Sub
End Class
