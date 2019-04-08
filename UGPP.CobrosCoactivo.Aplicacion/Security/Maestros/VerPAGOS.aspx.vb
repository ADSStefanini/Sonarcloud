Imports System.Data.SqlClient
Partial Public Class VerPAGOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            Loadcboestado()
            LoadcboUserVerif()
            Loadcbopagestadoprocfrp()

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from [dbo].[PAGOS] where [NroConsignacion] = @NroConsignacion"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @NroConsignacion parameter in the Command select query
                Command.Parameters.AddWithValue("@NroConsignacion", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroConsignacion.Text = Reader("NroConsignacion").ToString()
                    txtNroRadicadoSalida.Text = Reader("pagNroRadicadoSalida").ToString()
                    txtNroExp.Text = Reader("NroExp").ToString()
                    txtFecSolverif.Text = Reader("FecSolverif").ToString()
                    txtFecVerificado.Text = Left(Reader("FecVerificado").ToString().Trim, 10)
                    cboestado.SelectedValue = Reader("estado").ToString()
                    cboUserVerif.SelectedValue = Reader("UserVerif").ToString()
                    txtpagFecha.Text = Left(Reader("pagFecha").ToString().Trim, 10)
                    txtpagFechaDeudor.Text = Reader("pagFechaDeudor").ToString()
                    txtpagNroTitJudicial.Text = Reader("pagNroTitJudicial").ToString()
                    If IsDBNull(Reader("pagCapital")) Then
                        txtpagCapital.Text = ""
                    ElseIf Reader("pagCapital").ToString().Length = 1 Then
                        txtpagCapital.Text = Reader("pagCapital").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagCapital.Text = Convert.ToDouble(Reader("pagCapital")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagCapital.Text = Convert.ToDouble(Reader("pagCapital")).ToString("N0")
                    End If
                    If IsDBNull(Reader("pagAjusteDec1406")) Then
                        txtpagAjusteDec1406.Text = ""
                    ElseIf Reader("pagAjusteDec1406").ToString().Length = 1 Then
                        txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagAjusteDec1406.Text = Convert.ToDouble(Reader("pagAjusteDec1406")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagAjusteDec1406.Text = Convert.ToDouble(Reader("pagAjusteDec1406")).ToString("N0")
                    End If
                    If IsDBNull(Reader("pagInteres")) Then
                        txtpagInteres.Text = ""
                    ElseIf Reader("pagInteres").ToString().Length = 1 Then
                        txtpagInteres.Text = Reader("pagInteres").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagInteres.Text = Convert.ToDouble(Reader("pagInteres")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagInteres.Text = Convert.ToDouble(Reader("pagInteres")).ToString("N0")
                    End If
                    If IsDBNull(Reader("pagGastosProc")) Then
                        txtpagGastosProc.Text = ""
                    ElseIf Reader("pagGastosProc").ToString().Length = 1 Then
                        txtpagGastosProc.Text = Reader("pagGastosProc").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagGastosProc.Text = Convert.ToDouble(Reader("pagGastosProc")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagGastosProc.Text = Convert.ToDouble(Reader("pagGastosProc")).ToString("N0")
                    End If
                    If IsDBNull(Reader("pagExceso")) Then
                        txtpagExceso.Text = ""
                    ElseIf Reader("pagExceso").ToString().Length = 1 Then
                        txtpagExceso.Text = Reader("pagExceso").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagExceso.Text = Convert.ToDouble(Reader("pagExceso")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagExceso.Text = Convert.ToDouble(Reader("pagExceso")).ToString("N0")
                    End If
                    If IsDBNull(Reader("pagTotal")) Then
                        txtpagTotal.Text = ""
                    ElseIf Reader("pagTotal").ToString().Length = 1 Then
                        txtpagTotal.Text = Reader("pagTotal").ToString()
                    Else
                        'Miles separados por coma
                        'txtpagTotal.Text = Convert.ToDouble(Reader("pagTotal")).ToString("0,0", New CultureInfo("en-US", False).NumberFormat)
                        'Miles separados por punto
                        txtpagTotal.Text = Convert.ToDouble(Reader("pagTotal")).ToString("N0")
                    End If
                    cbopagestadoprocfrp.SelectedValue = Reader("pagestadoprocfrp").ToString()

                End If
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()
                ' 
                'The length of ID equals zero.
                'This is an insert so don't preload any data.
            Else
                ' 
                'Since this is an insert then you can't delete it yet because it's not in the database.
                ' 
                'end of if Reader.Read 

            End If

        End If
    End Sub

    Protected Sub Loadcboestado()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [ESTADOS_PAGO] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboestado.DataTextField = "nombre"
        cboestado.DataValueField = "codigo"
        cboestado.DataSource = Command.ExecuteReader()
        cboestado.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboUserVerif()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [USUARIOS] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboUserVerif.DataTextField = "nombre"
        cboUserVerif.DataValueField = "codigo"
        cboUserVerif.DataSource = Command.ExecuteReader()
        cboUserVerif.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub Loadcbopagestadoprocfrp()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [ESTADOS_PROCESO] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cbopagestadoprocfrp.DataTextField = "nombre"
        cbopagestadoprocfrp.DataValueField = "codigo"
        cbopagestadoprocfrp.DataSource = Command.ExecuteReader()
        cbopagestadoprocfrp.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PAGOSEXPEDIENTE.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

End Class