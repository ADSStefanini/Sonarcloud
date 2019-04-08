Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades

Public Class BandejaAreaOrigenBLL

    Private Property _BandejaTitulosAreaOrigenDAL As BandejaAreaOrigenDAL
    Private Property _Audit As LogAuditoria

    Public Sub New()
        _BandejaTitulosAreaOrigenDAL = New BandejaAreaOrigenDAL()
    End Sub

    Public Sub New(ByVal auditData As LogAuditoria)
        _Audit = auditData
        _BandejaTitulosAreaOrigenDAL = New BandejaAreaOrigenDAL(_Audit)
    End Sub

    ''' <summary>
    ''' Funcion para consultar los datos de la grilla de bandeja de titulos area origen
    ''' </summary>
    ''' <param name="USULOG"></param>
    ''' <param name="NROTITULO"></param>
    ''' <param name="ESTADOPROCESAL"></param>
    ''' <param name="ESTADOSOPERATIVO"></param>
    ''' <param name="FCHENVIOCOBRANZADESDE"></param>
    ''' <param name="FCHENVIOCOBRANZAHASTA"></param>
    ''' <param name="NROIDENTIFICACIONDEUDOR"></param>
    ''' <param name="NOMBREDEUDOR"></param>
    Public Function ConsultarDatosGrillaBandejaAreaOrigen(ByVal USULOG As String, ByVal NROTITULO As String, ByVal ESTADOPROCESAL As Int32, ByVal ESTADOSOPERATIVO As Int32, ByVal FCHENVIOCOBRANZADESDE As String, ByVal FCHENVIOCOBRANZAHASTA As String, ByVal NROIDENTIFICACIONDEUDOR As String, ByVal NOMBREDEUDOR As String) As List(Of BandejaTitulosAreaOrigen)
        Dim lsvResultProcedimiento As List(Of BandejaTitulosAreaOrigen) = _BandejaTitulosAreaOrigenDAL.ConsultaGrillaBandejaAreaOrigen(USULOG, NROTITULO, ESTADOPROCESAL, ESTADOSOPERATIVO, FCHENVIOCOBRANZADESDE, FCHENVIOCOBRANZAHASTA, NROIDENTIFICACIONDEUDOR, NOMBREDEUDOR)
        lsvResultProcedimiento.RemoveAll(Function(x) (x.ID_ESTADO_OPERATIVOS = 1 And x.IDUNICO = 0))
        Dim lsvResultEntity As List(Of BandejaTitulosAreaOrigen) = RetornarLecturaTitulosAreaOrigen(_BandejaTitulosAreaOrigenDAL.ConsultaBandejaAreaOrigenEnCreacion(USULOG))
        If lsvResultEntity.Count() > 0 Then
            If String.IsNullOrEmpty(NROTITULO) = True And ESTADOPROCESAL = 0 And ESTADOSOPERATIVO = 0 And String.IsNullOrEmpty(FCHENVIOCOBRANZADESDE) = True And String.IsNullOrEmpty(FCHENVIOCOBRANZAHASTA) = True And String.IsNullOrEmpty(NOMBREDEUDOR) = True And String.IsNullOrEmpty(NROIDENTIFICACIONDEUDOR) = True Then
                lsvResultProcedimiento.AddRange(lsvResultEntity.ToList())
            Else
                If (String.IsNullOrEmpty(NROTITULO) = False Or String.IsNullOrEmpty(NOMBREDEUDOR) = False Or String.IsNullOrEmpty(NROIDENTIFICACIONDEUDOR) = False) Then
                    If String.IsNullOrEmpty(NROTITULO) = False Then
                        'lsvResultProcedimiento.AddRange(lsvResultEntity.Where(Function(x) (x.NROTITULO = NROTITULO)).ToList())
                        lsvResultProcedimiento.AddRange(lsvResultEntity.FindAll(Function(x) x.NROTITULO.IndexOf(NROTITULO, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                    End If
                    If String.IsNullOrEmpty(NOMBREDEUDOR) = False Then
                        lsvResultProcedimiento.AddRange(lsvResultEntity.FindAll(Function(y) y.NOMBREDEUDOR.IndexOf(NOMBREDEUDOR, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                    End If
                    If String.IsNullOrEmpty(NROIDENTIFICACIONDEUDOR) = False Then
                        lsvResultProcedimiento.AddRange(lsvResultEntity.FindAll(Function(x) x.NRONITCEDULA.IndexOf(NROIDENTIFICACIONDEUDOR, StringComparison.OrdinalIgnoreCase) >= 0).ToList())
                        'lsvResultProcedimiento.AddRange(lsvResultEntity.Where(Function(x) (x.NRONITCEDULA = NROIDENTIFICACIONDEUDOR)).ToList())
                    End If
                End If
            End If
        End If
        Return lsvResultProcedimiento
    End Function

    Public Function RetornarLecturaTitulosAreaOrigen(ByVal ListBandeja As List(Of ALMACENAMIENTO_TEMPORAL)) As List(Of BandejaTitulosAreaOrigen)
        Dim NewListBandeja As List(Of BandejaTitulosAreaOrigen)
        Dim dataTiposTitulo As New DocumentoTituloTipoTituloDAL
        NewListBandeja = New List(Of BandejaTitulosAreaOrigen)()
        For Each item As ALMACENAMIENTO_TEMPORAL In ListBandeja
            Dim ObjectNew As BandejaTitulosAreaOrigen
            ObjectNew = New BandejaTitulosAreaOrigen()
            ObjectNew.ID_TAREA_ASIGNADA = item.TAREA_ASIGNADA.ID_TAREA_ASIGNADA
            ObjectNew.ID_ESTADO_OPERATIVOS = item.TAREA_ASIGNADA.COD_ESTADO_OPERATIVO
            If String.IsNullOrEmpty(item.JSON_OBJ) = False Then
                Dim tituloEjecutivoObj As TituloEjecutivoExt
                tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(item.JSON_OBJ)
                If tituloEjecutivoObj IsNot Nothing Then
                    ObjectNew.TIPOOBLIGACION = tituloEjecutivoObj.TituloEjecutivo.tipoTitulo
                    If tituloEjecutivoObj.TituloEjecutivo.totalObligacion IsNot Nothing Then
                        ObjectNew.TOTALOBLIGACION = (tituloEjecutivoObj.TituloEjecutivo.valorTitulo + tituloEjecutivoObj.TituloEjecutivo.partidaGlobal + tituloEjecutivoObj.TituloEjecutivo.sancionOmision + tituloEjecutivoObj.TituloEjecutivo.sancionInexactitud + tituloEjecutivoObj.TituloEjecutivo.sancionMora)
                    End If
                    ObjectNew.NROTITULO = tituloEjecutivoObj.TituloEjecutivo.numeroTitulo
                End If
                Dim DeudorPrincipal = tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.TipoEnte) = 1).FirstOrDefault()
                If DeudorPrincipal IsNot Nothing Then
                    ObjectNew.NOMBREDEUDOR = DeudorPrincipal.nombreDeudor
                    ObjectNew.NRONITCEDULA = DeudorPrincipal.numeroIdentificacion
                End If
                End If
            If (String.IsNullOrEmpty(ObjectNew.NOMBREDEUDOR)) Then
                ObjectNew.NOMBREDEUDOR = String.Empty
            End If
            If (ObjectNew.IDUNICO = 0) Then
                If (ObjectNew.TIPOOBLIGACION IsNot Nothing) Then
                    ObjectNew.TIPOOBLIGACION = dataTiposTitulo.obtenerTiposTitulo().Where(Function(x) x.codigo = ObjectNew.TIPOOBLIGACION).FirstOrDefault().nombre
                End If
            End If
            NewListBandeja.Add(ObjectNew)
        Next
        Return NewListBandeja
    End Function
End Class




