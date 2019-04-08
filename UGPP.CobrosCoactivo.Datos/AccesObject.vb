Imports System.Data.SqlClient

Public Class AccesObject(Of T As Class)
    Public Property ConnectionString As String

    Public Sub ExecuteCommand(ByVal NameCommand As String, ParamArray Parameters As SqlParameter())
        Dim ListObject As List(Of T) = New List(Of T)()
        Try
            Using _SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                _SqlConnection.Open()
                Dim sqlcmd As SqlCommand = New SqlCommand(NameCommand, _SqlConnection)
                sqlcmd.CommandType = CommandType.StoredProcedure

                For Each item As SqlParameter In Parameters
                    sqlcmd.Parameters.Add(item)
                Next

                sqlcmd.ExecuteReader()
            End Using

        Catch __unusedException1__ As Exception
            Throw
        End Try
    End Sub


    Public Function ExecuteCommand(ByVal NameCommand As String, ByVal OutPut As String, ParamArray Parameters As SqlParameter()) As String
        Dim ListObject As List(Of T) = New List(Of T)()
        Dim vaou As String = "0"
        Try
            Using _SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                _SqlConnection.Open()
                Dim sqlcmd As SqlCommand = New SqlCommand(NameCommand, _SqlConnection)
                sqlcmd.CommandType = CommandType.StoredProcedure

                For Each item As SqlParameter In Parameters
                    sqlcmd.Parameters.Add(item)
                Next

                sqlcmd.ExecuteReader()
                vaou = sqlcmd.Parameters(OutPut).Value
            End Using

        Catch __unusedException1__ As Exception
            Throw
        End Try

        Return vaou
    End Function



    Public Function ExecuteList(ByVal NameCommand As String, ParamArray Parameters As SqlParameter()) As List(Of T)
        Dim ListObject As List(Of T) = New List(Of T)()

        Try

            Using _SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                _SqlConnection.Open()
                Dim sqlcmd As SqlCommand = New SqlCommand(NameCommand, _SqlConnection)
                sqlcmd.CommandType = CommandType.StoredProcedure

                For Each item As SqlParameter In Parameters
                    sqlcmd.Parameters.Add(item)
                Next

                ListObject = FillListWithDataReader(ListObject.[GetType](), sqlcmd.ExecuteReader())
            End Using

        Catch __unusedException1__ As Exception
            Throw
        End Try

        Return ListObject
    End Function

    Public Function ExcuteDataTable(ByRef DtTable As DataTable, ByVal NameCommand As String, ParamArray Parameters As SqlParameter()) As DataTable
        Try

            Using _SqlConnection As SqlConnection = New SqlConnection(ConnectionString)
                _SqlConnection.Open()
                Dim sqlcmd As SqlCommand = New SqlCommand(NameCommand, _SqlConnection)
                sqlcmd.CommandType = CommandType.StoredProcedure

                For Each item As SqlParameter In Parameters
                    sqlcmd.Parameters.Add(item)
                Next

                DtTable.Load(sqlcmd.ExecuteReader())
            End Using

        Catch __unusedException1__ As Exception
            Throw
        End Try

        Return DtTable
    End Function

    Private Function FillListWithDataReader(ByVal TypeObject As Type, ByVal DtReader As SqlDataReader) As List(Of T)
        Dim instance As Object
        Dim lstObj As List(Of Object) = New List(Of Object)()

        While DtReader.Read()
            instance = Activator.CreateInstance(TypeObject.GetGenericArguments()(0))

            For Each drow As DataRow In DtReader.GetSchemaTable().Rows
                FillObjectProperty(instance, drow.ItemArray(0).ToString(), DtReader(drow.ItemArray(0).ToString()))
            Next

            lstObj.Add(instance)
        End While

        Dim lstResult As List(Of T) = New List(Of T)()

        For Each item As Object In lstObj
            lstResult.Add(CType(Convert.ChangeType(item, GetType(T)), T))
        Next

        Return lstResult
    End Function

    Private Sub FillObjectProperty(ByRef Item As Object, ByVal PropertyName As String, ByVal PropertyValue As Object)
        If Not (PropertyValue.[GetType]() = GetType(System.DBNull)) Then
            Dim TObjet As Type = Item.[GetType]()
            If TObjet.GetProperty(PropertyName) <> Nothing Then
                TObjet.GetProperty(PropertyName).SetValue(Item, PropertyValue)
            End If
        End If
    End Sub
End Class