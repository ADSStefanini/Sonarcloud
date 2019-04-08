Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditSECUESTROAVAREM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            LoadcboTipoBienSec()

            ' Colocar el valor que viene como parametro para el tipo de bien 
            Dim TipoBien As String = Request("pTipoBien").Trim
            cboTipoBienSec.SelectedValue = TipoBien
            txtIdBienSec.Text = Request("pIdBien").Trim
            'Desactivar controles
            cboTipoBienSec.Enabled = False
            txtIdBienSec.Enabled = False

            '---------------------------------------------------------------------------------------------------------
            Dim IdUnico As String = Request("pIdUnico").Trim            
            If ExisteSecuestroAvaRem(IdUnico) Then
                Dim Connection As New SqlConnection(Funciones.CadenaConexion)
                Connection.Open()
                Dim sql As String = "SELECT * FROM SECUESTROAVALUOREM WHERE idunico = @IDUNICO"
                '
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@IDUNICO", IdUnico)
                Dim Reader As SqlDataReader = Command.ExecuteReader

                'Si se encontro el Id
                If Reader.Read Then

                    'Secuestro
                    txtNroResSec.Text = Reader("NroResSec").ToString().Trim
                    txtFecResSec.Text = Left(Reader("FecResSec").ToString(), 10)
                    txtEstadoSec.Text = Reader("EstadoSec").ToString().Trim
                    txtIdSecuestre.Text = Reader("IdSecuestre").ToString().Trim
                    txtNomSecuestre.Text = Reader("NomSecuestre").ToString().Trim

                    'Avaluo
                    txtNroResAva.Text = Reader("NroResAva").ToString().Trim
                    txtFecResAva.Text = Left(Reader("FecResAva").ToString().Trim, 10)
                    txtOfiTrasAva.Text = Reader("OfiTrasAva").ToString().Trim
                    txtFecObjAva.Text = Left(Reader("FecObjAva").ToString().Trim, 10)
                    txtNroResApruAva.Text = Reader("NroResApruAva").ToString().Trim
                    txtFecResApruAva.Text = Left(Reader("FecResApruAva").ToString().Trim, 10)
                    txtFecNotAutoAva.Text = Left(Reader("FechaNotifAva").ToString().Trim, 10)

                    'Remate
                    txtNroResRem.Text = Reader("NroResRem").ToString().Trim
                    txtFecResRem.Text = Left(Reader("FecResRem").ToString().Trim, 10)
                    txtFecLicita1.Text = Left(Reader("FecLicita1").ToString().Trim, 10)
                    txtFecLicita2.Text = Left(Reader("FecLicita2").ToString().Trim, 10)
                    txtFecLicita3.Text = Left(Reader("FecLicita3").ToString().Trim, 10)

                End If
                ' 
                'Cerrar el Data Reader
                Reader.Close()
                'Cerrar conexion
                Connection.Close()
            End If


        End If

    End Sub

    Private Sub LoadcboTipoBienSec()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM tipos_bienes ORDER BY codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboTipoBienSec.DataTextField = "nombre"
        cboTipoBienSec.DataValueField = "codigo"
        cboTipoBienSec.DataSource = Command.ExecuteReader()
        cboTipoBienSec.DataBind()
        'Close the Connection Object 
        Connection.Close()

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click

        Dim IdUnico As String = Request("pIdUnico").Trim
        '---------------------------------------------------------------------------------------------
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "INSERT INTO SECUESTROAVALUOREM (idunico, NroResolEm, IdentifBien, TipoBien, NroResSec, FecResSec, EstadoSec, IdSecuestre, NomSecuestre, NroResAva, FecResAva, OfiTrasAva, FecObjAva, NroResApruAva, FecResApruAva, FechaNotifAva, NroResRem, FecResRem, FecLicita1, FecLicita2, FecLicita3) VALUES (@idunico, @NroResolEm, @IdentifBien, @TipoBien, @NroResSec, @FecResSec, @EstadoSec, @IdSecuestre, @NomSecuestre, @NroResAva, @FecResAva, @OfiTrasAva, @FecObjAva, @NroResApruAva, @FecResApruAva, @FechaNotifAva, @NroResRem, @FecResRem, @FecLicita1, @FecLicita2, @FecLicita3) "
        Dim UpdateSQL As String = "UPDATE SECUESTROAVALUOREM SET NroResolEm = @NroResolEm, IdentifBien = @IdentifBien, TipoBien = @TipoBien, NroResSec = @NroResSec, FecResSec = @FecResSec, EstadoSec = @EstadoSec, IdSecuestre = @IdSecuestre, NomSecuestre = @NomSecuestre, NroResAva = @NroResAva, FecResAva = @FecResAva, OfiTrasAva = @OfiTrasAva, FecObjAva = @FecObjAva, NroResApruAva = @NroResApruAva, FecResApruAva = @FecResApruAva, FechaNotifAva = @FechaNotifAva, NroResRem = @NroResRem, FecResRem = @FecResRem, FecLicita1 = @FecLicita1, FecLicita2 = @FecLicita2, FecLicita3 = @FecLicita3 WHERE idunico = @idunico "

        If ExisteSecuestroAvaRem(IdUnico) Then
            'Hacer update 
            Command = New SqlCommand(UpdateSQL, Connection)            
        Else
            'Hacer insert
            Command = New SqlCommand(InsertSQL, Connection)            
        End If

        'Asignación de valores a los parametros
        Command.Parameters.AddWithValue("@idunico", IdUnico)
        Command.Parameters.AddWithValue("@NroResolEm", Request("pResolEm").Trim)
        Command.Parameters.AddWithValue("@IdentifBien", Request("pIdBien").Trim)
        Command.Parameters.AddWithValue("@TipoBien", Request("pTipoBien").Trim)

        ' Secuestro
        Command.Parameters.AddWithValue("@NroResSec", txtNroResSec.Text.Trim)
        If IsDate(Left(txtFecResSec.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResSec", Left(txtFecResSec.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResSec", DBNull.Value)
        End If
        Command.Parameters.AddWithValue("@EstadoSec", txtEstadoSec.Text.Trim)
        Command.Parameters.AddWithValue("@IdSecuestre", txtIdSecuestre.Text.Trim)
        Command.Parameters.AddWithValue("@NomSecuestre", txtNomSecuestre.Text.Trim)

        ' Avaluo
        Command.Parameters.AddWithValue("@NroResAva", txtNroResAva.Text.Trim)        
        If IsDate(Left(txtFecResAva.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResAva", Left(txtFecResAva.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResAva", DBNull.Value)
        End If
        Command.Parameters.AddWithValue("@OfiTrasAva", txtOfiTrasAva.Text)        
        If IsDate(Left(txtFecObjAva.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecObjAva", Left(txtFecObjAva.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecObjAva", DBNull.Value)
        End If
        Command.Parameters.AddWithValue("@NroResApruAva", txtNroResApruAva.Text)        
        If IsDate(Left(txtFecResApruAva.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResApruAva", Left(txtFecResApruAva.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResApruAva", DBNull.Value)
        End If
        If IsDate(Left(txtFecNotAutoAva.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FechaNotifAva", Left(txtFecNotAutoAva.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FechaNotifAva", DBNull.Value)
        End If

        ' Remate
        Command.Parameters.AddWithValue("@NroResRem", txtNroResRem.Text.Trim)
        If IsDate(Left(txtFecResRem.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResRem", Left(txtFecResRem.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResRem", DBNull.Value)
        End If
        If IsDate(Left(txtFecLicita1.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecLicita1", Left(txtFecLicita1.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecLicita1", DBNull.Value)
        End If
        If IsDate(Left(txtFecLicita2.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecLicita2", Left(txtFecLicita2.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecLicita2", DBNull.Value)
        End If
        If IsDate(Left(txtFecLicita3.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecLicita3", Left(txtFecLicita3.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecLicita3", DBNull.Value)
        End If
        'Command.Parameters.AddWithValue("@FormaNotifRem", FormaNotifRem)
        'Command.Parameters.AddWithValue("@FechaNotifRem", FechaNotifRem) '

        Try
            Command.ExecuteNonQuery()

            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de Secuestro, Avaluo y Remate", "Expediente " & Request("pExpediente").Trim, Command)
            '
            CustomValidator1.Text = "Módulo de Secuestro, Avaluo y Remate. Datos almacenados con éxito."
            CustomValidator2.Text = "Módulo de Secuestro, Avaluo y Remate. Datos almacenados con éxito."

        Catch ex As Exception            
            CustomValidator1.Text = ex.Message
            CustomValidator2.Text = ex.Message

        End Try
        CustomValidator1.IsValid = False
        CustomValidator2.IsValid = False

        Connection.Close()
    End Sub

    Public Function ExisteSecuestroAvaRem(ByVal pIdUnico As String) As Boolean
        Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "  SELECT IdUnico FROM SECUESTROAVALUOREM WHERE IdUnico = " & pIdUnico
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            sw = True
        End If
        Reader.Close()
        Connection.Close()
        '
        Return sw
    End Function

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("DETALLE_EMBARGO.aspx?pResolEm=" & Request("pResolEm").Trim & "&pExpediente=" & Request("pExpediente").Trim)
    End Sub

End Class