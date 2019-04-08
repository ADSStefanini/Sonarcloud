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
                    txtNroExp.Text = Reader("NroExp").ToString()
                    txtFecSolverif.Text = Reader("FecSolverif").ToString()
                    txtFecVerificado.Text = Reader("FecVerificado").ToString()
                    cboestado.SelectedValue = Reader("estado").ToString()
                    cboUserVerif.SelectedValue = Reader("UserVerif").ToString()
                    txtpagFecha.Text = Reader("pagFecha").ToString()
                    txtpagFechaDeudor.Text = Reader("pagFechaDeudor").ToString()
                    txtpagNroTitJudicial.Text = Reader("pagNroTitJudicial").ToString()
                    txtpagCapital.Text = Reader("pagCapital").ToString()
                    txtpagAjusteDec1406.Text = Reader("pagAjusteDec1406").ToString()
                    txtpagInteres.Text = Reader("pagInteres").ToString()
                    txtpagGastosProc.Text = Reader("pagGastosProc").ToString()
                    txtpagExceso.Text = Reader("pagExceso").ToString()
                    txtpagTotal.Text = Reader("pagTotal").ToString()
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