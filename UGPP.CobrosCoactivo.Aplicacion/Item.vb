Public Class ItemParams

    Public Parameter_Name As String
    Public Parameter_Value As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal Name As String, ByVal Value As String)
        Parameter_Name = Name
        Parameter_Value = Value
    End Sub

End Class
