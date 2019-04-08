Imports System.Data.SqlClient
Partial Public Class EstadisticaxExpediente1
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
        Dim sql As String
        'sql = "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos, EFIUSUASIG, " & _
        '     "MAX(USUARIOS.nombre) AS NomGestor, SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda " & _
        '            "FROM EJEFISGLOBAL " & _
        '       "LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo " & _
        '       "LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente " & _
        '       "GROUP BY EJEFISGLOBAL.EFIUSUASIG"


        sql = "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                     " & _
                "             MAX(USUARIOS.nombre) AS NomGestor, SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda           " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "               LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo                           " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       " & _
                "  WHERE EJEFISGLOBAL.EFIESTADO  not in ('04','07','08') AND MAESTRO_TITULOS.ESTADO = 1                   " & _
                "               GROUP BY EJEFISGLOBAL.EFIUSUASIG                                                          " & _
                "                                                                                                         " & _
                " union                                                                                                   " & _
                "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                     " & _
                "             'DEVUELTOS' , SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda                                " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       " & _
                "WHERE  EJEFISGLOBAL.EFIESTADO = '04'                                                                     " & _
                "                                                                                                         " & _
                "                                                                                                         " & _
                " union                                                                                                   " & _
                "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                     " & _
                "             'TERMINADO' , SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda                                " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "                                                                                                         " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       " & _
                "WHERE EJEFISGLOBAL.EFIESTADO ='07'                                                                       " & _
                " union                                                                                                   " & _
                "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                     " & _
                "             'SUSPENDIDOS' , SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda                              " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "                                                                                                         " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       " & _
                "WHERE EJEFISGLOBAL.EFIESTADO ='08'                                                                       " & _
                " UNION                                                                                                   " & _
                "SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                     " & _
                "             'INACTIVOS' AS NomGestor, SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda                    " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       " & _
                "  WHERE MAESTRO_TITULOS.ESTADO != 1                                                                      " & _
                " union                                                                                                   " & _
                " SELECT COUNT(EJEFISGLOBAL.EFINROEXP) AS NumProcesos,                                                    " & _
                "             'TOTAL' AS NomGestor, SUM(MAESTRO_TITULOS.totaldeuda) AS TotalDdeuda                        " & _
                "                    FROM EJEFISGLOBAL                                                                    " & _
                "               LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo                           " & _
                "               LEFT JOIN MAESTRO_TITULOS ON EJEFISGLOBAL.EFINROEXP = MAESTRO_TITULOS.mt_expediente       "

        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql

        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()

        'Close the Connection Object 
        Connection.Close()

    End Sub

End Class