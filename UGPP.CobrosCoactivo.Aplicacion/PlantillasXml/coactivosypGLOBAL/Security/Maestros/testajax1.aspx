<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testajax1.aspx.vb" Inherits="coactivosyp.testajax1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
    <title>Ejemplo sencillo de AJAX</title>
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript">
        function realizaProceso(valorCaja1, valorCaja2) {
            var parametros = {
                "valorCaja1": valorCaja1,
                "valorCaja2": valorCaja2
            };
            $.ajax({
                data: parametros,
                url: 'test_ajax_proceso.aspx',
                type: 'post',                
                success: function(response) {
                    $("#resultado").html(response);
                }
            });
        }
    </script>
    </head>

    <body>
        Introduce valor 1
        <input type="text" name="caja_texto" id="valor1" value="0"/> 
    
        Introduce valor 2
        <input type="text" name="caja_texto" id="valor2" value="0"/>

        Realiza suma
        <input type="button" onclick="realizaProceso($('#valor1').val(), $('#valor2').val());return false;" value="Calcula"/>

        <br/>
        Resultado: <span id="resultado">0</span>
    </body>
</html>

