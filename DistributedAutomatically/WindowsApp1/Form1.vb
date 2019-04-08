Imports DistributedAutomatically.Service1


Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ejecucion As DistributedAutomatically.Service1 = New DistributedAutomatically.Service1

        ejecucion.Reparto_Automatico()
        MessageBox.Show("Termino Reparto Automatico")

    End Sub

    Private Shared Function GetEjecucion1(ejecucion As DistributedAutomatically.Service1) As DistributedAutomatically.Service1
        Return ejecucion
    End Function
End Class
