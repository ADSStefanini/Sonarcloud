Public Partial Class pruebas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub probar_cookie_Click(ByVal sender As Object, ByVal e As EventArgs) Handles probar_cookie.Click
        Dim myCookie As HttpCookie = New HttpCookie("UserSettings")
        myCookie("Font") = "Arial"
        myCookie("Color") = "Blue"
        myCookie.Expires = Now.AddDays(1)
        Response.Cookies.Add(myCookie)


        Response.Write("La cookie fue escrita correctamente... <br />")
        If (Response.Cookies("UserSettings") IsNot Nothing) Then
            Dim userSettings As String
            userSettings = Request.Cookies("UserSettings")("Font")
            Response.Write(userSettings)
        End If

        If (Response.Cookies("nombre") IsNot Nothing) Then
            Dim userSettings As String
            Response.Write("<br />")
            userSettings = Request.Cookies("nombre").Value
            Response.Write(userSettings)
        Else
            Response.Write("<br />")
            Response.Write("No esta ....")
        End If

    End Sub
End Class