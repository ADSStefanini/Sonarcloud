﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class Titulos
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("coactivosyp.Titulos", GetType(Titulos).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to El estado operativo actual no permite realizar calificación.
        '''</summary>
        Friend Shared ReadOnly Property lblErrorEstadoOperativoErroneo() As String
            Get
                Return ResourceManager.GetString("lblErrorEstadoOperativoErroneo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Solo pueden ser calificados los títulos.
        '''</summary>
        Friend Shared ReadOnly Property lblErrorNoEsTitulo() As String
            Get
                Return ResourceManager.GetString("lblErrorNoEsTitulo", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to El título no puede ser calificacdo.
        '''</summary>
        Friend Shared ReadOnly Property lblErrorPrioridad() As String
            Get
                Return ResourceManager.GetString("lblErrorPrioridad", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Este título ya se encuentra calificado y clasificado.
        '''</summary>
        Friend Shared ReadOnly Property lblErrorTituloClasificado() As String
            Get
                Return ResourceManager.GetString("lblErrorTituloClasificado", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Este título ya se encuentra calificado, por favor proceda a su clasificación.
        '''</summary>
        Friend Shared ReadOnly Property lblMensajeClasificacion() As String
            Get
                Return ResourceManager.GetString("lblMensajeClasificacion", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to La calificación del título esta asignado a otro gestor.
        '''</summary>
        Friend Shared ReadOnly Property lblUsuarioAsignado() As String
            Get
                Return ResourceManager.GetString("lblUsuarioAsignado", resourceCulture)
            End Get
        End Property
    End Class
End Namespace
