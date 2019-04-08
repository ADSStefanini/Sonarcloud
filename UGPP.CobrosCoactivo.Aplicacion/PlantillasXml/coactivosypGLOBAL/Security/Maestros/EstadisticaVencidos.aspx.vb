Imports System.Data.SqlClient
Partial Public Class EstadisticaVencidos
    Inherits System.Web.UI.Page

    Private PageSize As Long = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BindGrid()
        End If
    End Sub

  

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        'Create a new connection to the database        
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Funciones.CadenaConexion

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "WITH tmpVencimientos AS ( " & _
            "SELECT EJEFISGLOBAL.EFINROEXP, " & _
            "TERMINO = " & _
            "	CASE WHEN ESTADOS_PROCESO.nombre <> 'PERSUASIVO' THEN 'OK: OTRA ETAPA' " & _
            "	ELSE " & _
            "		CASE WHEN FecEstiFin > GETDATE() THEN 'POR VENCER' " & _
            "		ELSE 'VENCIDO' " & _
            "		END " & _
            "	END " & _
            "FROM EJEFISGLOBAL " & _
            "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " & _
            "LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp " & _
            "WHERE EJEFISGLOBAL.EFIUSUASIG = '" & Session("sscodigousuario") & "') " & _
            "SELECT tmpVencimientos.EFINROEXP,tmpVencimientos.termino " & _
            "FROM tmpVencimientos WHERE  tmpVencimientos.termino = 'VENCIDO'"

        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

    End Sub

End Class