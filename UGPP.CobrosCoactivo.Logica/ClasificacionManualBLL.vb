Imports UGPP.CobrosCoactivo.Entidades

Public Class ClasificacionManualBLL

    ''' <summary>
    ''' ID único del título que se esta actualizando
    ''' </summary>
    ''' <returns></returns>
    Property idTitulo As Int32
    ''' <summary>
    ''' ID de la tarea asiganada reacionada con el título
    ''' </summary>
    ''' <returns></returns>
    Property idTareaAsiganada As Int32
    Private Property _AuditEntity As LogAuditoria

    ''' <summary>
    ''' Realiza la clasificación de un expediente si cumple con los parametros exigidos en la clasificacion por cuantia
    ''' </summary>
    ''' <param name="prmStrNumExpediente">id del expediente</param>
    ''' <returns>valReturn retorna true si se realiza la clasificación por cuantia</returns>
    Public Function ClasificacionPorCuantia(ByVal prmStrNumExpediente As String, Optional ByVal prmEstablecerEstado As Boolean = False) As Boolean
        'Dim idExpediente = "80007"
        Dim idExpediente = prmStrNumExpediente
        'Inicializacion de clases
        Dim uvt As New UvtBLL


        'Por qué años?? => Representa la cantidad de UVTs que se deben tener en cuenta para la clasificación
        Dim domDetalle As New DominioDetalleBLL
        Dim dominioid = My.Resources.ValIdDomAniosClasCuant
        Dim prmCantidadUVTs = domDetalle.consultarDominioPorIdDominio(Convert.ToInt32(dominioid))
        Dim cantidadAnios = prmCantidadUVTs.FirstOrDefault(Function(x) (x.ID_DOMINIO) = dominioid)
        Dim cantidadUVTs As Int32 = Convert.ToInt32(cantidadAnios.VAL_VALOR)

        Dim pagos As PagosBLL = New PagosBLL
        Dim maestroTitulo As MaestroTitulosBLL = New MaestroTitulosBLL
        Dim anioActual As Int32 = Now.Year
        Dim listValUvtAnioActual = uvt.ObtenerValAnioActual()
        Dim objListaUvtAnioActual = listValUvtAnioActual.FirstOrDefault(Function(x) (x.Anio) = anioActual)
        Dim valUvtAnioActual = objListaUvtAnioActual.Valor * cantidadUVTs

        Dim ListaValTipoTitulo = maestroTitulo.consultarTipoTitulo(idExpediente)
        Dim valTipoTitulo = ListaValTipoTitulo.FirstOrDefault(Function(x) (x.MT_tipo_titulo))
        Dim valorDificilCobro = My.Resources.ValDificilCobro
        Dim tipoTitulo = valTipoTitulo.MT_tipo_titulo
        Dim listaPagos = pagos.consultarPagos(idExpediente)
        Dim capitalInicial = ListaValTipoTitulo.Sum(Function(x) (x.MT_total_obligacion + x.MT_total_partida_global))
        Dim valTotalDeuda = ListaValTipoTitulo.Sum(Function(x) (x.totaldeuda))
        Dim valRevocatoria = ListaValTipoTitulo.Sum(Function(x) (x.valorRevoca))
        Dim pagosACapital = listaPagos.Sum(Function(x) (x.pagCapital + x.pagAjusteDec1406))

        Dim saldoActual As Double
        If tipoTitulo = My.Resources.ValCodLiqOficialSancion Then
            saldoActual = Convert.ToDouble(valTotalDeuda) - Convert.ToDouble(valRevocatoria) - Convert.ToDouble(pagosACapital)
        Else
            saldoActual = Convert.ToDouble(valTotalDeuda) - Convert.ToDouble(valRevocatoria) - Convert.ToDouble(pagosACapital)
        End If

        If saldoActual <= Convert.ToDouble(valUvtAnioActual) Then
            Return True
        End If
        Return False
        'Dim saldoActualObligaciones = Convert.ToDouble(valTotalDeuda) - Convert.ToDouble(valRevocatoria) - Convert.ToDouble(pagosACapital) - Convert.ToDouble(capitalInicial)
        'Dim saldoActualObligacionesLiqOficial = Convert.ToDouble(valTotalDeuda) - Convert.ToDouble(valRevocatoria) - Convert.ToDouble(pagosACapital) - Convert.ToDouble(capitalInicial)

        'If saldoActualObligaciones <= Convert.ToDouble(valUvtAnioActual) And tipoTitulo <> My.Resources.ValCodLiqOficialSancion Then
        '    EstablecerEstadoProcesal(idExpediente, valorDificilCobro)
        '    valReturn = True
        'ElseIf saldoActualObligacionesLiqOficial <= Convert.ToDouble(valUvtAnioActual) And tipoTitulo <> My.Resources.ValCodLiqOficialSancion Then
        '    EstablecerEstadoProcesal(idExpediente, valorDificilCobro)
        '    valReturn = True
        'End If
        'Return valReturn
    End Function

    ''' <summary>
    ''' permite actualizar el estado procesal del expediente
    ''' </summary>
    ''' <param name="prmStrNumExpediente">id del expediente</param>
    ''' <param name="ValEstadoProcesal">valor del estado procesal a actualizar</param>
    Public Function EstablecerEstadoProcesal(ByVal prmStrNumExpediente As String, ValEstadoProcesal As String) As Boolean
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        If expedientes.ActualizarExpediente(prmStrNumExpediente, ValEstadoProcesal) Then
            If Not IsNothing(Me.idTareaAsiganada) Then
                Dim _expedienteBLL As New ExpedienteBLL()
                _expedienteBLL.asignarExpedientePorRepartir(prmStrNumExpediente)
                Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(idTareaAsiganada, 6)

                'Asignar expediente a reparto
                Dim objTareaAsigana As New Entidades.TareaAsignada
                objTareaAsigana.VAL_USUARIO_NOMBRE = String.Empty 'Se deja vacio para que no presente error
                objTareaAsigana.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Expediente
                objTareaAsigana.EFINROEXP_EXPEDIENTE = prmStrNumExpediente
                objTareaAsigana.COD_ESTADO_OPERATIVO = 10 'Por repartir

                Try
                    _tareaAsignadaBLL.registrarTarea(objTareaAsigana)
                Catch ex As Exception
                    'TODO: Llamar LOG errores
                End Try
            End If

            Return True
        End If
        Return False
    End Function


    ''' <summary>
    ''' Metodo apra asignar estado y etapa procesal
    ''' </summary>
    ''' <param name="prmStrNumExpediente"></param>
    ''' <param name="ValEstadoProcesal"></param>
    ''' <param name="ValEtapaProcesal"></param>
    ''' <returns></returns>
    Public Function EstablecerEstadoProcesal(ByVal prmStrNumExpediente As String, ValEstadoProcesal As String, ValEtapaProcesal As String) As Boolean

        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        If String.IsNullOrEmpty(ValEtapaProcesal) Then
            If expedientes.ActualizarExpediente(prmStrNumExpediente, ValEstadoProcesal) Then
                If Not IsNothing(Me.idTareaAsiganada) Then
                    Dim _expedienteBLL As New ExpedienteBLL()
                    _expedienteBLL.asignarExpedientePorRepartir(prmStrNumExpediente)
                    Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                    _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(idTareaAsiganada, 6)

                    'Asignar expediente a reparto
                    Dim objTareaAsigana As New Entidades.TareaAsignada
                    objTareaAsigana.VAL_USUARIO_NOMBRE = String.Empty 'Se deja vacio para que no presente error
                    objTareaAsigana.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Expediente
                    objTareaAsigana.EFINROEXP_EXPEDIENTE = prmStrNumExpediente
                    objTareaAsigana.COD_ESTADO_OPERATIVO = 10 'Por repartir

                    Try
                        _tareaAsignadaBLL.registrarTarea(objTareaAsigana)
                    Catch ex As Exception
                        'TODO: Llamar LOG errores
                    End Try
                End If

                Return True
            End If
        End If

        If String.IsNullOrEmpty(ValEtapaProcesal) = False Then
            If expedientes.ActualizarExpedienteEtapaProcesal(prmStrNumExpediente, ValEstadoProcesal, ValEtapaProcesal) Then
                If Not IsNothing(Me.idTareaAsiganada) Then
                    Dim _expedienteBLL As New ExpedienteBLL()
                    _expedienteBLL.asignarExpedientePorRepartir(prmStrNumExpediente)
                    Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                    _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(idTareaAsiganada, 6)

                    'Asignar expediente a reparto
                    Dim objTareaAsigana As New Entidades.TareaAsignada
                    objTareaAsigana.VAL_USUARIO_NOMBRE = String.Empty 'Se deja vacio para que no presente error
                    objTareaAsigana.COD_TIPO_OBJ = Entidades.Enumeraciones.DominioDetalle.Expediente
                    objTareaAsigana.EFINROEXP_EXPEDIENTE = prmStrNumExpediente
                    objTareaAsigana.COD_ESTADO_OPERATIVO = 10 'Por repartir

                    Try
                        _tareaAsignadaBLL.registrarTarea(objTareaAsigana)
                    Catch ex As Exception
                        'TODO: Llamar LOG errores
                    End Try
                End If

                Return True
            End If
        End If

        Return False
    End Function



    ''' <summary>
    ''' cambia el estado procesal del expediente a cartera incobrable
    ''' </summary>
    Public Function EstadoProcesalCarteraIncobrable(ByVal idExpediente As String, ByVal valDdlMatriculaMercantil As String) As Boolean
        Dim valueDdlMatriculaMercantil = valDdlMatriculaMercantil.ToString()
        Dim valorCarteraIncobrable = My.Resources.ValCarteraIncobrable
        Dim valRet As Boolean = False
        If valueDdlMatriculaMercantil = My.Resources.ValNegativoddl Then
            EstablecerEstadoProcesal(idExpediente, valorCarteraIncobrable)
            valRet = True
        End If
        Return valRet
    End Function



    Public Function EstadoProcesalDificilCobro(ByVal idExpediente As String, ByVal valDdlPersonaViva As String) As Boolean
        Dim valueDdlPersonaViva As String = valDdlPersonaViva
        Dim valorDificilCobro As String = My.Resources.ValDificilCobro
        Dim valRet As Boolean = False
        If valueDdlPersonaViva = My.Resources.ValNegativoddl Then
            EstablecerEstadoProcesal(idExpediente, valorDificilCobro)
            valRet = True
        End If
        Return valRet
    End Function

    ''' <summary>
    ''' permite realizar la clasificacion manual dependiendo del tipo de proceso juridico
    ''' </summary>
    Public Sub ClasificacionPorTipoProcesoJuridico(ByVal idExpediente As String, ByVal valDdlTipoProcesoJuridica As String, ByVal valDdlBeneficioTributario As String)
        Dim valueddlTipoProcesoJuridica = valDdlTipoProcesoJuridica
        Dim valueddlBeneficioTributario = valDdlBeneficioTributario
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valEstadoProcPersuasivo = My.Resources.ValEstadoProcesoPersuasivo
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        Dim valEstadoProcesoCoactivo = My.Resources.ValEstadoProcesoCoactivo
        Dim valEtapaBeneficioTributario = Convert.ToInt32(My.Resources.ValEtapaBenTributario)
        If valueddlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria And valueddlBeneficioTributario = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcPersuasivo, valEtapaBeneficioTributario)
        Else
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcConcursal, valEtapaBeneficioTributario)
        End If
        If valueddlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcesoCoactivo)
        ElseIf valueddlTipoProcesoJuridica <> My.Resources.ValTiProcLiqVoluntaria And valueddlTipoProcesoJuridica <> My.Resources.DefaultValueDdlTipoProceso Then
            expedientes.ActualizarExpediente(idExpediente, valEstadoProcConcursal)
        End If
    End Sub

    ''' <summary>
    ''' Permite realizar la clasificación por beneficio tributario
    ''' </summary>
    Public Sub clasificacionBeneficioTributario(ByVal idExpediente As String, ByVal valDdlBeneficioTributario As String, ByVal valDdlTipoProcesoJuridica As String)
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valueddlBeneficioTributario = valDdlBeneficioTributario
        Dim valEstadoProcPersuasivo = My.Resources.ValEstadoProcesoPersuasivo
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        Dim valEtapaBeneficioTributario = Convert.ToInt32(My.Resources.ValEtapaBenTributario)
        If valDdlTipoProcesoJuridica = My.Resources.ValTiProcLiqVoluntaria And valueddlBeneficioTributario = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcConcursal, valEtapaBeneficioTributario)
            EstablecerEstadoProcesal(idExpediente, Me.idTareaAsiganada)
        ElseIf valueddlBeneficioTributario = My.Resources.ValNegativoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcPersuasivo, valEtapaBeneficioTributario)
            EstablecerEstadoProcesal(idExpediente, Me.idTareaAsiganada)
        End If

    End Sub

    ''' <summary>
    ''' Permite clasificación si el deudor realizo pagos
    ''' </summary>
    Public Sub clasificacionDeudorRealizaPagos(ByVal idExpediente As String, ByVal valDdlPagosDeudor As String)
        Dim expedientes As EjefisglobalBLL = New EjefisglobalBLL
        Dim valueddlPagosDeudor = valDdlPagosDeudor
        Dim valEstadoProcVerifPagos = My.Resources.ValEstadoProcesoVerifPagos
        Dim valEtapaNormalizacion = My.Resources.ValEtapaNormalizacion
        If valueddlPagosDeudor = My.Resources.ValPositivoddl Then
            expedientes.ActualizarExpedienteEtapaProcesal(idExpediente, valEstadoProcVerifPagos, valEtapaNormalizacion)
        End If
    End Sub

    ''' <summary>
    ''' Permite la clasificación por tipo proceso natural
    ''' </summary>
    Public Sub ClasificacionPorTipoProcesoNatural(ByVal idExpediente As String, ByVal valDdlTipoProcesoNatural As String)
        Dim valueddlTipoProcesoNatural = valDdlTipoProcesoNatural
        Dim valEstadoProcConcursal = My.Resources.ValEstadoProcesoConcursal
        If valueddlTipoProcesoNatural <> My.Resources.DefaultValueDdlTipoProceso Then
            EstablecerEstadoProcesal(idExpediente, valEstadoProcConcursal)
        End If
    End Sub
End Class
