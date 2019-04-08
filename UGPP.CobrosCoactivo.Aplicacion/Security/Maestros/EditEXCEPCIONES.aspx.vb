Imports System.Data.SqlClient

Partial Public Class EditEXCEPCIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from EXCEPCIONES where NroRad = @NroRad"
                Dim Command As New SqlCommand(sql, Connection)

                Command.Parameters.AddWithValue("@NroRad", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    txtNroRad.Text = Reader("NroRad").ToString()
                    txtFecRad.Text = Left(Reader("FecRad").ToString().Trim, 10)
                    'txtNroExp.Text = Reader("NroExp").ToString()
                    txtNroResResuelve.Text = Reader("NroResResuelve").ToString()
                    txtFecResResuelve.Text = Left(Reader("FecResResuelve").ToString().Trim, 10)
                    txtNroOfiNotCor.Text = Reader("NroOfiNotCor").ToString().Trim
                    txtFecOfiNotCor.Text = Left(Reader("FecOfiNotCor").ToString().Trim, 10)
                    txtFecNotif.Text = Left(Reader("FecNotif").ToString().Trim, 10)

                    'Datos del recurso de reposicion
                    txtFecRadRecurso.Text = Left(Reader("FecRadRecurso").ToString().Trim, 10)
                    txtNroResolRes.Text = Reader("NroResolRes").ToString().Trim
                    txtFecResolRes.Text = Left(Reader("FecResolRes").ToString().Trim, 10)
                    txtNroOfiCitaPers.Text = Reader("NroOfiCitaPers").ToString().Trim
                    txtFecOfiCitaPers.Text = Left(Reader("FecOfiCitaPers").ToString().Trim, 10)
                    txtFecPubEdic.Text = Left(Reader("FecPubEdic").ToString().Trim, 10)

                End If
                ' 
                'Close the Data Reader we are done with it.
                Reader.Close()

                'Close the Connection Object 
                Connection.Close()
            Else
                'Since this is an insert then you can't delete it yet because it's not in the database.
                'cmdDelete.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            Dim msg As String = ""
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                msg = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                'Desactivar controles
                DesactivarControles(msg)
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    msg = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    DesactivarControles(msg)
                End If
                
            End If

        End If
    End Sub

    Private Sub DesactivarControles(ByVal pMsg As String)
        cmdSave.Visible = False
        txtNroRad.Enabled = False
        txtFecRad.Enabled = False
        txtNroResResuelve.Enabled = False
        txtFecResResuelve.Enabled = False

        txtNroOfiNotCor.Enabled = False
        txtFecOfiNotCor.Enabled = False
        txtFecNotif.Enabled = False

        CustomValidator1.Text = pMsg
        CustomValidator1.IsValid = False
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If txtNroRad.Text.Trim = "" Or txtFecRad.Text.Trim = "" Then
            CustomValidator1.Text = "El número de radicación de la excepción y su respectiva fecha son obligatorios!"
            CustomValidator1.IsValid = False
        Else
            CustomValidator1.Text = ""
            CustomValidator1.IsValid = True
            '-------------------------------
            Dim ID As String = Request("ID")

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            Dim Command As SqlCommand

            Dim InsertSQL As String = "Insert into EXCEPCIONES (NroRad, FecRad, NroExp, NroOfiNotCor, FecOfiNotCor, FecNotif, FecRadRecurso, NroOfiCitaPers, FecOfiCitaPers, FecPubEdic ) VALUES ( @NroRad, @FecRad, @NroExp, @NroOfiNotCor, @FecOfiNotCor, @FecNotif, @FecRadRecurso, @NroOfiCitaPers, @FecOfiCitaPers, @FecPubEdic ) "
            Dim UpdateSQL As String = "Update EXCEPCIONES set FecRad = @FecRad, NroExp = @NroExp, NroOfiNotCor = @NroOfiNotCor, FecOfiNotCor = @FecOfiNotCor, FecNotif = @FecNotif, FecRadRecurso = @FecRadRecurso, NroOfiCitaPers = @NroOfiCitaPers, FecOfiCitaPers = @FecOfiCitaPers, FecPubEdic = @FecOfiCitaPers where NroRad = @NroRad "

            If String.IsNullOrEmpty(ID) Then
                'insert 
                ID = txtNroRad.Text.Trim
                Command = New SqlCommand(InsertSQL, Connection)
                Command.Parameters.AddWithValue("@NroRad", ID)
            Else
                'update             
                Command = New SqlCommand(UpdateSQL, Connection)
                Command.Parameters.AddWithValue("@NroRad", ID)
            End If

            If IsDate(Left(txtFecRad.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecRad", Left(txtFecRad.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecRad", DBNull.Value)
            End If

            'Command.Parameters.AddWithValue("@NroExp", txtNroExp.Text)
            Dim pExpediente As String
            pExpediente = Request("pExpediente")
            Command.Parameters.AddWithValue("@NroExp", pExpediente)

            ' 18/jun/2014. Daniel ya genera estos datos con los actos 359 y 370
            'Command.Parameters.AddWithValue("@NroResResuelve", txtNroResResuelve.Text)
            'If IsDate(Left(txtFecResResuelve.Text.Trim, 10)) Then
            '    Command.Parameters.AddWithValue("@FecResResuelve", Left(txtFecResResuelve.Text.Trim, 10))
            'Else
            '    Command.Parameters.AddWithValue("@FecResResuelve", DBNull.Value)
            'End If

            Command.Parameters.AddWithValue("@NroOfiNotCor", txtNroOfiNotCor.Text.Trim)

            If IsDate(Left(txtFecOfiNotCor.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecOfiNotCor", Left(txtFecOfiNotCor.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecOfiNotCor", DBNull.Value)
            End If

            If IsDate(Left(txtFecNotif.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecNotif", Left(txtFecNotif.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecNotif", DBNull.Value)
            End If

            '18/jul/2014. Campos del recurso de reposicion
            'FecRadRecurso, 
            If IsDate(Left(txtFecRadRecurso.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecRadRecurso", Left(txtFecRadRecurso.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecRadRecurso", DBNull.Value)
            End If

            'NroOfiCitaPers, 
            Command.Parameters.AddWithValue("@NroOfiCitaPers", txtNroOfiCitaPers.Text.Trim)

            'FecOfiCitaPers, 
            If IsDate(Left(txtFecOfiCitaPers.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecOfiCitaPers", Left(txtFecOfiCitaPers.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecOfiCitaPers", DBNull.Value)
            End If

            'FecPubEdic
            If IsDate(Left(txtFecPubEdic.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecPubEdic", Left(txtFecPubEdic.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecPubEdic", DBNull.Value)
            End If

            Try
                Command.ExecuteNonQuery()
                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Excepciones", "No. radicación " & ID.Trim, Command)
                Connection.Close()
                Response.Redirect("EXCEPCIONES.aspx?pExpediente=" & Request("pExpediente").Trim)

            Catch ex As Exception
                Dim Msg As String = ex.Message
                CustomValidator1.Text = Msg
                CustomValidator1.IsValid = False
                Connection.Close()

            End Try

        End If        
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("EXCEPCIONES.aspx?pExpediente=" & Request("pExpediente").Trim)
    End Sub

End Class