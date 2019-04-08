Imports System.Data.SqlClient
Partial Public Class iyc_infogral
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            LoadInfo(Placa)
        End If
    End Sub

    Private Sub LoadInfo(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Grid de Propietarios
        cmd = "SELECT MaeNum AS placa,MaeProCod AS nit_cedula,MaePreNum AS NroPredial,MaeProNom AS nombre_contribuyente," & _
                "maeestact AS estado,MaeDir AS direccion_Establecimiento,MaeFecIni AS Fec_Ini_Act,MaeBarCod AS Barrio," & _
                "MaeZonCod AS zona, MaeFecRen AS 'Fec-Renovacion', MaeFecResC AS 'Fec-Can.', MaeNroResC AS Resol_Can," & _
                "MaeFecCam AS Fec_Camara_Com, MaeFecUltP AS FecUlt_Pago, MaeRecUltP AS 'rec-ult-pag'," & _
                "MaeValUltP AS Valor_Ult_Pago, MaePerHas AS 'Ano-Hasta', MaeSubHas AS 'Periodo-Hasta'," & _
                "MaeTel AS Telefono, MaeHor AS Horario, MaeAcuPag AS Acuerdo_de_Pago, MaeEstFis AS Emplaz_por_no_declarar," & _
                "MaeEstDian AS Emplaz_para_corregir, MaeActBaz AS 'Act-Baz', MaeDes AS 'Descrip. Actividad' " & _
                "FROM MAEIC WHERE MaeNum = '" & pPlaca & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstablecimiento As New DataTable
        Adaptador.Fill(dtEstablecimiento)

        'Colocar los datos de los campos en los textbox
        txtIdPlaca.Text = CType(dtEstablecimiento.Rows(0).Item(0), Integer).ToString("000000")
        txtCedula.Text = dtEstablecimiento.Rows(0).Item(1).ToString
        txtNumPredial.Text = dtEstablecimiento.Rows(0).Item(2).ToString
        txtNombre.Text = dtEstablecimiento.Rows(0).Item(3).ToString
        '-------
        txtDireccion.Text = dtEstablecimiento.Rows(0).Item(5).ToString
        'txtFecIniAct.Text = dtEstablecimiento.Rows(0).Item(6).ToString("dd/MM/yyyy") 'en una linea esto NO funciona
        Dim FecIniAct As Date = dtEstablecimiento.Rows(0).Item(6)
        txtFecIniAct.Text = FecIniAct.ToString("dd/MM/yyyy")

        txtBarrio.Text = dtEstablecimiento.Rows(0).Item(7).ToString
        txtZona.Text = dtEstablecimiento.Rows(0).Item(8).ToString

        'txtFecRenovacion.Text = dtEstablecimiento.Rows(0).Item(9).ToString        
        Dim FecRenovacion As Date = dtEstablecimiento.Rows(0).Item(9)
        txtFecRenovacion.Text = FecRenovacion.ToString("dd/MM/yyyy")

        txtFecCanc.Text = dtEstablecimiento.Rows(0).Item(10).ToString
        If Mid(txtFecCanc.Text, 7, 4) = "1753" Then
            txtFecCanc.Text = ""
        Else
            Dim FecCanc As Date = dtEstablecimiento.Rows(0).Item(10)
            txtFecCanc.Text = FecCanc.ToString("dd/MM/yyyy")
        End If
        txtResolCan.Text = dtEstablecimiento.Rows(0).Item(11).ToString

        'txtFecCamCom.Text = dtEstablecimiento.Rows(0).Item(12).ToString
        Dim FecCamCom As Date = dtEstablecimiento.Rows(0).Item(12)
        txtFecCamCom.Text = FecCamCom.ToString("dd/MM/yyyy")

        txtFecUltPag.Text = dtEstablecimiento.Rows(0).Item(13).ToString
        If Mid(txtFecUltPag.Text, 7, 4) = "1753" Then
            txtFecUltPag.Text = ""
        Else
            Dim FecUltPag As Date = dtEstablecimiento.Rows(0).Item(13)
            txtFecUltPag.Text = FecUltPag.ToString("dd/MM/yyyy")
        End If
        txtRecUltPag.Text = dtEstablecimiento.Rows(0).Item(14).ToString
        txtValUltPag.Text = dtEstablecimiento.Rows(0).Item(15).ToString
        txtAnoHas.Text = dtEstablecimiento.Rows(0).Item(16).ToString
        txtPerHas.Text = dtEstablecimiento.Rows(0).Item(17).ToString
        txtTel.Text = dtEstablecimiento.Rows(0).Item(18).ToString
        txtHorario.Text = dtEstablecimiento.Rows(0).Item(19).ToString

        'Acuerdos de pago
        'txtAcuPag.Text = dtEstablecimiento.Rows(0).Item(20).ToString
        Dim AcuPag As Integer = CType(dtEstablecimiento.Rows(0).Item(20), Integer)
        Select Case AcuPag
            Case 0
                txtAcuPag.Text = "Sin Acuerdo"
            Case 1
                txtAcuPag.Text = "Con Acuerdo"
            Case 8
                txtAcuPag.Text = "Sin Acuerdo"
        End Select

        txtEmpNoDec.Text = dtEstablecimiento.Rows(0).Item(21).ToString

        txtEmpCor.Text = dtEstablecimiento.Rows(0).Item(22).ToString
        txtActBas.Text = dtEstablecimiento.Rows(0).Item(23).ToString
        txtDescrip.Text = dtEstablecimiento.Rows(0).Item(24).ToString
        '----------------------------------
        Dim estado As Integer = CType(dtEstablecimiento.Rows(0).Item(4), Integer)
        Select Case estado
            Case 0
                txtEstado.Text = "Activa" 'NO DEFINIDO (Se muestran como activos) (15 registros)
            Case 1
                txtEstado.Text = "Activa"
            Case 2
                txtEstado.Text = "Con Novedad"
            Case 3
                txtEstado.Text = "Cancelada"
            Case 4
                txtEstado.Text = "Temporal"
            Case 9
                txtEstado.Text = "Activa" 'NO DEFINIDO (7 registros)
        End Select


    End Sub
End Class