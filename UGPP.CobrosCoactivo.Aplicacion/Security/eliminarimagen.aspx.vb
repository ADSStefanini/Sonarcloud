Imports System.Data
Imports System.Data.SqlClient
Public Class eliminarimagen
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection

    End Sub
    Protected WithEvents HyperLink4 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink5 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink6 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim NomServidor, Usuario, Clave, BaseDatos, cmd, userDE As String
        NomServidor = ConfigurationManager.AppSettings("ServerName")
        Usuario = ConfigurationManager.AppSettings("BD_User")
        Clave = ConfigurationManager.AppSettings("BD_pass")
        BaseDatos = ConfigurationManager.AppSettings("BD_name")

        Me.SqlConnection1.ConnectionString = "workstation id= " & NomServidor & ";packet size=4096;user id=" & Usuario & ";data source=" & NomServidor & _
            ";persist security info=True;initial catalog=" & BaseDatos & ";password=" & Clave


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Código para eliminar la imagen
        'Creando la sentencia SQL
        Dim mIdEnte, mIdActo, mArchivo, cmd, mTotimg As String
        Dim registros As Integer
        mIdEnte = Request.QueryString("idente")
        mIdActo = Request.QueryString("idacto")
        mArchivo = Request.QueryString("nomarchivo")
        'mTotimg = Request.QueryString("totimg") - 1

        cmd = "DELETE FROM documentos WHERE entidad = '" & mIdEnte & "' AND idacto = '" & mIdActo & "' AND nomarchivo = '" & mArchivo & "'"

        ' Se crea el SQLCommand
        Dim cmdsql As SqlCommand
        cmdsql = New SqlCommand(cmd, Me.SqlConnection1)
        cmdsql.CommandType = CommandType.Text

        ' Abrir la conexion
        Me.SqlConnection1.Open()

        ' Ejecutar la sentencia de borrado        
        registros = cmdsql.ExecuteNonQuery()

        'Consultar cuantas hojas hay
        cmd = "SELECT count(*) AS NumPaginas FROM documentos WHERE entidad = '" & mIdEnte & "' AND idacto = '" & mIdActo & "' AND cobrador = '" & Session("mcobrador") & "'"
        cmdsql.CommandText = cmd
        mTotimg = cmdsql.ExecuteScalar()
        mTotimg = mTotimg

        ' Crear y ejecutar la sentencia de actualizacion del numero de paginas
        cmd = "UPDATE documentos SET paginas = " & mTotimg & " WHERE entidad = '" & mIdEnte & "' AND idacto = '" & mIdActo & "' AND cobrador = '" & Session("mcobrador") & "'"
        cmdsql.CommandText = cmd
        registros = cmdsql.ExecuteNonQuery()

        'Cerrar la conexion
        Me.SqlConnection1.Close()


        ' Borrar la imagen del servidor 
        Dim FileToDelete As String
        FileToDelete = Server.MapPath("") & "\expedientes\" & Session("mcobrador") & "\" & mIdEnte & "\" & mArchivo

        If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.Delete(FileToDelete)
            'MsgBox("File Deleted")
        End If


        'Mostrar resultado
        Me.Label13.Text = "Imagen eliminada en forma exitosa"
        Me.Button2.Enabled = False

    End Sub
End Class
