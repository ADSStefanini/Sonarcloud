Imports System.Data.SqlClient
Imports CrystalDecisions.Shared

Partial Public Class estado_cuenta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Predio As String
        Predio = Request.QueryString("idpred")
        If Not Page.IsPostBack Then
            If Predio <> "" Then
                Me.txtEnte.Text = Predio
            End If            
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim cmd, cmd2, refecata, UltResol, chequesDev, NroExp As String
        Dim cuadro3 As String()
        Dim Report As New repEstadoCuenta
        refecata = Me.txtEnte.Text.Trim

        If refecata = "" Then
            Validator.ErrorMessage = "Digite un número de predial por favor"
            Validator.IsValid = False
            Exit Sub
        End If

        UltResol = Me.GetUltimaResol(refecata)
        chequesDev = Me.GetChequesDev(refecata)
        NroExp = Me.GetNroExp(refecata)
        cuadro3 = Me.GetCuadro3(refecata)

        cmd = "SELECT PREDIOS.PreNum, PREDIOS.PreMatInm, PREDIOS.PreCod, PREDIOS.PreDir, PREDIOS.PreDirCob, PREDIOS.PreEstMun2, " & _
                "CASE WHEN PREDIOS.PreEstVal IS NULL  THEN 0 WHEN PREDIOS.PreEstVal IS NOT NULL  THEN PREDIOS.PreEstVal END AS PreEstVal, " & _
                "CASE WHEN PREDIOS.PreMult IS NULL  THEN 0 WHEN PREDIOS.PreMult IS NOT NULL  THEN PREDIOS.PreMult END AS PreMult,  " & _
                "PREDIOS.PreLegMun, PREDIOS.PreRecUltP, " & _
                "PREDIOS.PreFecUltP, PREDIOS.PreValUltP, PREDIOS.PrePerDes, PREDIOS.PreSubDes, PREDIOS.PrePerCan, PREDIOS.PreSubCan, PREDIOS3.PrePrsDoc, PREDIOS3.PrePrsNom, '" & _
                UltResol & "' AS Ultresol, '" & chequesDev & "' AS chequesDev, '" & NroExp & "' AS NroExp, '" & _
                cuadro3(0) & "' AS Ultunidad, '" & cuadro3(1) & "' AS Ultexpediente, '" & cuadro3(2) & "' AS Ultresolucion, '" & _
                cuadro3(3) & "' AS remanente, '" & _
                cuadro3(4) & "' AS periodo, '" & _
                cuadro3(5) & "' AS avaluo, '" & _
                cuadro3(6) & "' AS dest1, '" & _
                cuadro3(7) & "' AS dest2, '" & _
                cuadro3(8) & "' AS idEstrato, '" & _
                cuadro3(9) & "' AS NomEstrato, '" & _
                cuadro3(10) & "' AS idUbic, '" & _
                cuadro3(11) & "' AS NomUbicacion, '" & _
                cuadro3(12) & "' AS idTipo, '" & _
                cuadro3(13) & "' AS NomTipo, '" & _
                cuadro3(14) & "' AS Aterreno, '" & _
                cuadro3(15) & "' AS AConstruida " & _
                "FROM PREDIOS INNER JOIN PREDIOS3 ON RTRIM(PREDIOS.PreNum) = RTRIM(PREDIOS3.PreNum) " & _
                "WHERE (PREDIOS3.PreEstPer = 1) AND (RTRIM(PREDIOS.PreNum) = '" & refecata & "')"

        Dim dsEC As New DataSet

        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            'Dim tb As New ds.actuacionesDataTable
            'Dim tb As New dsEstadoCuenta.EstadoCuentaMixDataTable
            adapter.Fill(dsEC, "EstadoCuentaMix")
        End Using

        'Llenar el otro datatable
        cmd2 = "SELECT ejefis.PreNum,ejefis.EfiNroExp,ejefis.EfiUni,ejefis.EfiGenNuev,ejefis.EfiCon," & _
                    "ejefis2.EfiResTip,ejefis2.EfiResFec,ejefis2.EfiResNum,ejefis2.EfiResTes,ejefis2.EfiResCar," & _
                    "ejefis2.EfiResAbo, ejefis2.EfiResOb1, pasos.PasDes, cargofu1.FunCarNom " & _
                    "FROM ejefis, pasos, ejefis2 LEFT OUTER JOIN cargofu1 " & _
                    "ON ejefis2.EfiResCar = cargofu1.FunCarCod AND ejefis2.EfiResAbo = cargofu1.FunCon " & _
                    "WHERE ejefis.EfiCon = ejefis2.EfiCon AND " & _
                    "ejefis2.EfiResTip = pasos.PasCod AND " & _
                    "RTRIM(ejefis.PreNum) = '" & refecata & "' AND " & _
                    "ejefis.EfiEst = 0 AND " & _
                    "RTRIM(ejefis2.EfiResTip) > '201' " & _
                    "ORDER BY ejefis2.PefCod, ejefis2.EfiCon"

        Using Adapter2 As New System.Data.SqlClient.SqlDataAdapter(cmd2, conexion)
            Adapter2.Fill(dsEC, "DetalleProceso")

            'Si la tabla de detalle no devuelve filas => Machetear
            If dsEC.Tables("DetalleProceso").Rows.Count = 0 Then
                cmd2 = "SELECT TOP 1 '" & refecata & "' AS PreNum, 0 AS EfiNroExp, 0 AS EfiUni, '' AS EfiGenNuev, " & _
                    "0 AS EfiCon, '' AS EfiResTip, NULL AS EfiResFec, 0  AS EfiResNum, 0 AS EfiResTes,   " & _
                    "0 AS EfiResCar, 0 AS EfiResAbo, '' AS EfiResOb1, '' AS PasDes, '' AS FunCarNom   " & _
                    "FROM ejefis"
                Adapter2.SelectCommand.CommandText = cmd2
                Adapter2.Fill(dsEC, "DetalleProceso")
            End If
        End Using

        Imprimir_reporte2(Me.Page, Report, dsEC, "prueba.pdf", "")
        Response.Write("<a href='prueba.pdf' target='blank'>prueba.pdf</a>")

    End Sub

    Private Function GetCuadro3(ByVal RefeCata As String) As String()
        Dim cmd, EfiUni, EfiNroExp, EfiNroRes, Remanente, periodo, avaluo, idDestEcom, dest1, dest2, idEstrato As String
        Dim NomEstrato, idUbic, NomUbicacion, idTipo, NomTipo, Aterreno, AConstruida As String
        'Dim cuadro3 As String() = {"", "", "", ""} hasta el remanente
        '16 posiciones del 0 al 15
        Dim cuadro3 As String() = {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}
        EfiUni = ""
        EfiNroExp = ""
        EfiNroRes = ""
        cmd = "SELECT PreNum,EfiUni,EfiNroExp,EfiNroRes,EfiResImp FROM ejefis WHERE RTRIM(PreNum) = '" & RefeCata & "' AND EfiResImp = (SELECT MAX(EfiResImp) FROM ejefis WHERE RTRIM(PreNum) = '" & RefeCata & "')"
        Dim Dataset1 As DataSet = New DataSet
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "ejefis")
            If Dataset1.Tables("ejefis").Rows.Count > 0 Then
                EfiUni = Trim(Dataset1.Tables("ejefis").Rows(0).Item("EfiUni"))
                EfiNroExp = Trim(Dataset1.Tables("ejefis").Rows(0).Item("EfiNroExp"))
                EfiNroRes = Trim(Dataset1.Tables("ejefis").Rows(0).Item("EfiNroRes"))
            Else
                EfiUni = ""
                EfiNroExp = ""
                EfiNroRes = ""
            End If
        End Using
        cuadro3(0) = EfiUni
        cuadro3(1) = EfiNroExp
        cuadro3(2) = EfiNroRes

        '----------------------------------------------------------------------
        'Extraer dato del remanente
        cmd = "SELECT RemNroRad,RemNomJuz FROM REMANEN WHERE RTRIM(RemPreNum) = '" & RefeCata & "'"
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "remanen")
            If Dataset1.Tables("remanen").Rows.Count > 0 Then
                Remanente = Trim(Dataset1.Tables("remanen").Rows(0).Item("RemNroRad")) & " " & _
                     Trim(Dataset1.Tables("remanen").Rows(0).Item("RemNomJuz"))
            Else
                Remanente = ""
            End If            
        End Using
        cuadro3(3) = Remanente

        'Datos de la vigencia actual
        cmd = "SELECT * FROM predios2 WHERE RTRIM(PreNum) = '" & RefeCata & "' AND PerCod = " & _
                    "(SELECT MAX(percod) FROM predios2 WHERE RTRIM(PreNum) = '" & RefeCata & "') " & _
                 "ORDER BY CarCod"

        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "predios2")
            If Dataset1.Tables("predios2").Rows.Count > 0 Then
                periodo = Trim(Dataset1.Tables("predios2").Rows(2).Item("PerCod").ToString)
                avaluo = Trim(Dataset1.Tables("predios2").Rows(2).Item("PreCarVal").ToString)

                'Area del terreno y area cosntruida
                Aterreno = Trim(Dataset1.Tables("predios2").Rows(3).Item("PreCarVal").ToString)
                AConstruida = Trim(Dataset1.Tables("predios2").Rows(4).Item("PreCarVal").ToString)

                'Codigo del destino economico
                idDestEcom = Trim(Dataset1.Tables("predios2").Rows(0).Item("PreCarVal").ToString)

                'Codigo del estrato
                idEstrato = Trim(Dataset1.Tables("predios2").Rows(1).Item("PreCarVal").ToString)

                'Codigo de la ubicacion
                idUbic = Trim(Dataset1.Tables("predios2").Rows(5).Item("PreCarVal").ToString)

                'Codigo del tipo
                idTipo = Trim(Dataset1.Tables("predios2").Rows(6).Item("PreCarVal").ToString)
            Else
                periodo = ""
                avaluo = ""
                Aterreno = ""
                AConstruida = ""
                idDestEcom = ""
                idEstrato = ""
                idUbic = ""
                idTipo = ""
            End If
        End Using

        'Buscar el destino economico
        cmd = "SELECT * FROM caracte1 WHERE CarCod = 1 AND  CarDom = " & idDestEcom
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "caracte1")
            If Dataset1.Tables("caracte1").Rows.Count > 0 Then
                dest1 = Trim(Dataset1.Tables("caracte1").Rows(0).Item("carIga").ToString)
                dest2 = Trim(Dataset1.Tables("caracte1").Rows(0).Item("CarDesDom").ToString)
            Else
                dest1 = ""
                dest2 = ""
            End If
        End Using

        'Buscar el estrato
        cmd = "SELECT * FROM caracte1 WHERE CarCod = 2 AND  CarDom = " & idEstrato
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "qEstrato")
            If Dataset1.Tables("qEstrato").Rows.Count > 0 Then
                NomEstrato = Trim(Dataset1.Tables("qEstrato").Rows(0).Item("CarDesDom").ToString)
            Else
                NomEstrato = ""
            End If
        End Using

        'Buscar la ubicacion
        cmd = "SELECT * FROM caracte1 WHERE CarCod = 6 AND  CarDom = " & idUbic
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "qUbicacion")
            If Dataset1.Tables("qUbicacion").Rows.Count > 0 Then
                NomUbicacion = Trim(Dataset1.Tables("qUbicacion").Rows(0).Item("CarDesDom").ToString)
            Else
                NomUbicacion = ""
            End If
        End Using

        'Buscar el tipo 
        cmd = "SELECT * FROM caracte1 WHERE CarCod = 7 AND  CarDom = " & idTipo
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "qTipo")
            If Dataset1.Tables("qTipo").Rows.Count > 0 Then
                NomTipo = Trim(Dataset1.Tables("qTipo").Rows(0).Item("CarDesDom").ToString)
            Else
                NomTipo = ""
            End If
        End Using

        'Asignar valores al vector
        cuadro3(4) = periodo
        cuadro3(5) = avaluo
        cuadro3(6) = dest1
        cuadro3(7) = dest2
        cuadro3(8) = idEstrato
        cuadro3(9) = NomEstrato
        cuadro3(10) = idUbic
        cuadro3(11) = NomUbicacion
        cuadro3(12) = idTipo
        cuadro3(13) = NomTipo
        cuadro3(14) = Aterreno
        cuadro3(15) = AConstruida

        Return cuadro3
    End Function
    Private Function GetNroExp(ByVal RefeCata As String) As String
        Dim cmd, NroExp As String
        NroExp = ""
        cmd = "SELECT COUNT(*) AS NumExp FROM ejefis WHERE RTRIM(prenum)='" & RefeCata & "'"
        Dim Dataset1 As DataSet = New DataSet
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "ejefis")
            NroExp = Trim(Dataset1.Tables("ejefis").Rows(0).Item("NumExp").ToString)
        End Using
        Return NroExp
    End Function
    Private Function GetChequesDev(ByVal RefeCata As String) As String
        Dim cmd, ChequesDev As String
        ChequesDev = ""
        cmd = "SELECT COUNT(*) AS devueltos FROM CHEQDEV1 WHERE RTRIM(ChePreNum) = '" & RefeCata & "'"
        Dim Dataset1 As DataSet = New DataSet
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "cheqdev1")
            ChequesDev = Trim(Dataset1.Tables("cheqdev1").Rows(0).Item("devueltos").ToString)
        End Using
        Return ChequesDev
    End Function
    Private Function GetUltimaResol(ByVal RefeCata As String) As String
        Dim cmd, UltimaResol As String
        UltimaResol = ""
        cmd = "SELECT MAX(ResTip) AS ResTip,MAX(ResVig) AS ResVig,MAX(ResNum) AS ResNum,MAX(PreResFecA) AS PreResFecA FROM PREDIOS4 WHERE RTRIM(PreNum) = '" & RefeCata & "'"
        Dim Dataset1 As DataSet = New DataSet
        Using adapter As New System.Data.SqlClient.SqlDataAdapter(cmd, conexion)
            adapter.Fill(Dataset1, "predios4")

            UltimaResol = Trim(Dataset1.Tables("predios4").Rows(0).Item("ResTip")) & " " & _
                            Dataset1.Tables("predios4").Rows(0).Item("ResVig").ToString & " " & _
                            Dataset1.Tables("predios4").Rows(0).Item("ResNum") & " " & _
                            Format(Dataset1.Tables("predios4").Rows(0).Item("PreResFecA"), "dd/MM/yyyy").ToString
        End Using

        Return UltimaResol
    End Function

    Public Function Imprimir_reporte(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
              ByVal Datos As Object, ByVal NameFile As String, ByVal Path As String) As String

        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile

        DiskOpts.DiskFileName = Server.MapPath("~") & "\Security\prueba.pdf"

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Return DiskOpts.DiskFileName
    End Function

    Public Function Imprimir_reporte2(ByVal Page As Page, ByVal Report As CrystalDecisions.CrystalReports.Engine.ReportClass, _
              ByVal Datos As DataSet, ByVal NameFile As String, ByVal Path As String) As String

        Report.SetDataSource(Datos)
        Dim ExportOpts = New ExportOptions
        Dim DiskOpts = New DiskFileDestinationOptions
        Dim PdfFormatOpts = New PdfRtfWordFormatOptions

        ExportOpts = Report.ExportOptions
        ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile

        DiskOpts.DiskFileName = Server.MapPath("~") & "\Security\prueba.pdf"

        ExportOpts.DestinationOptions = DiskOpts
        Report.Export()
        Datos.dispose()
        Report.Close()
        Report.Dispose()
        Report = Nothing

        Return DiskOpts.DiskFileName
    End Function

    Private Function conexion() As String
        Dim strConnString As String = ConfigurationManager.ConnectionStrings("impuestosConnectionString2").ConnectionString
        Return strConnString
    End Function
End Class