Public Class ServiceParent

    Public Sub ClearProperties(obj As Object)
        If obj Is Nothing Then
            Return
        End If
        Dim type As Type = obj.GetType()
        For Each propertyInfo As Reflection.PropertyInfo In type.GetProperties()
            If propertyInfo.PropertyType.IsArray Then
                Dim vector = DirectCast(propertyInfo.GetValue(obj, Nothing), Array)
                If vector IsNot Nothing Then
                    For index = 0 To vector.Length - 1
                        ClearProperties(vector(index))
                    Next
                End If
            ElseIf propertyInfo.CanWrite AndAlso (propertyInfo.PropertyType = GetType(System.String)) Then
                If (propertyInfo.GetValue(obj, Nothing) Is Nothing) Then
                    propertyInfo.SetValue(obj, "")
                End If
            ElseIf propertyInfo.PropertyType.IsClass Then
                ClearProperties(propertyInfo.GetValue(obj, Nothing))
            End If
        Next
    End Sub

End Class
