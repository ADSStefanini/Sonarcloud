Imports System.Data.SqlClient
Partial Public Class capturainformeespecial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If

        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")
            txtexpediente.Focus()

        End If
    End Sub

    Protected Sub LinkCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkCancelar.Click
        Response.Redirect("cobranzas2.aspx")
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Try

            If validaExpediente(txtexpediente.Text.Trim, Session("ssimpuesto")) Then


                Dim sihaydoc As Boolean = chekhay(txtexpediente.Text.Trim, txtacto.Text.Trim)
                If sihaydoc Then

                    Dim resolucion As DataTable = Funciones.InsertReslucion(txtexpediente.Text.Trim, txtacto.Text.Trim)
                    If resolucion.Rows.Count > 0 Then
                        Validator.Text = "Documento generado satisfactoriamente. con nuemro de resolucion R" & String.Format("{0:000000}", CInt(resolucion.Rows(0).Item("DG_NRO_DOC"))) & " <br />"
                        Me.Validator.IsValid = False
                    Else
                        Validator.Text = "El acto administrativo " & txtacto.Text.Trim & " no genera numero de resolución <br />"
                        Me.Validator.IsValid = False

                    End If

                End If
            Else
                Validator.Text = "El expediente E" & String.Format("{0:000000}", CInt(txtexpediente.Text.Trim)) & " no esta asociado al impuesto " & Session("ssCodimpadm") & " o no se ha generado  <br />"
                Me.Validator.IsValid = False

            End If
        Catch ex2 As Exception
            Validator.Text = "No se guardaron los datos. <br />" & ex2.Message
            Me.Validator.IsValid = False
        End Try

    End Sub

    Private Function chekhay(ByVal expediente As String, ByVal acto As String) As Boolean
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM DOCUMENTOS_GENERADOS WHERE DG_EXPEDIENTE = @doc_expediente and DG_COD_ACTO=@acto", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@doc_expediente", SqlDbType.VarChar).Value = expediente
            myadapter.SelectCommand.Parameters.Add("@acto", SqlDbType.VarChar).Value = acto
            Using myTable As New DataTable
                myadapter.Fill(myTable)
                If myTable.Rows.Count > 0 Then
                    Validator.Text = "El acto asociado al expediente ya se genero previamente. con nuemro de resolucion R" & String.Format("{0:000000}", CInt(myTable.Rows(0).Item("DG_NRO_DOC"))) & " <br />"
                    Me.Validator.IsValid = False
                    Return False
                Else
                    Return True
                End If
            End Using
        End Using
    End Function

    Private Function validaExpediente(ByVal expediente As String, ByVal IMP As String) As Boolean
        Dim sw As Boolean = True
        Using myadapter As New SqlClient.SqlDataAdapter("SELECT *  FROM EJEFISGLOBAL WHERE EFINROEXP = @expediente and EFIMODCOD=@IMP", Funciones.CadenaConexion)
            myadapter.SelectCommand.Parameters.Add("@expediente", SqlDbType.VarChar).Value = expediente
            myadapter.SelectCommand.Parameters.Add("@IMP", SqlDbType.VarChar).Value = IMP
            Using myTable As New DataTable
                myadapter.Fill(myTable)
                If myTable.Rows.Count = 0 Then
                    sw = False
                End If
            End Using
        End Using

        Return sw
    End Function


End Class