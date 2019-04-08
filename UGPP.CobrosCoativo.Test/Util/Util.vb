Imports System.IO

Public Class Util
    Public Shared Function ReadFileAsBase64(ByVal fileName As String) As String
        Dim ruta As String = Path.Combine(Environment.CurrentDirectory, "..", "..", "Files", fileName)

        Using ms As MemoryStream = New MemoryStream()

            Using fileStream As FileStream = New FileStream(ruta, FileMode.Open)
                ms.SetLength(fileStream.Length)
                fileStream.Read(ms.GetBuffer(), 0, CInt(fileStream.Length))
            End Using

            Return Convert.ToBase64String(ms.ToArray())
        End Using
    End Function

    Public Shared Function ReadFileAsArray(ByVal fileName As String) As Byte()
        Dim ruta As String = Path.Combine(Environment.CurrentDirectory, "..", "..", "Files", fileName)

        Using ms As MemoryStream = New MemoryStream()

            Using fileStream As FileStream = New FileStream(ruta, FileMode.Open)
                ms.SetLength(fileStream.Length)
                fileStream.Read(ms.GetBuffer(), 0, CInt(fileStream.Length))
            End Using

            Return ms.ToArray()
        End Using
    End Function

End Class
