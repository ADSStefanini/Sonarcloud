<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="busactuaciones.aspx.vb" Inherits="coactivosyp.busactuaciones" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
      $(document).ready(function() {
            //agregar una nueva columna con todo el texto
            //contenido en las columnas de la grilla
           // contains de Jquery es CaseSentive, por eso a minúscula

          $(".filtrar tr:has(td)").each(function() {
          var t = $(this).text().toLowerCase(); 
                    $("<td class='indexColumn'></td>").hide().text(t).appendTo(this);
                });

            //Agregar el comportamiento al texto (se selecciona por el ID)
            $("#texto").keyup(function() {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function() {
                     $(".filtrar tr:visible .indexColumn:not(:contains('"
                     + this + "'))").parent().hide();
                }); 
            }); 
        });
        
        
          $(document).ready(function() {
            //agregar una nueva columna con todo el texto
            //contenido en las columnas de la grilla
           // contains de Jquery es CaseSentive, por eso a minúscula

            
                //Agregar el comportamiento al texto (se selecciona por el ID)
                $("#Text2").keyup(function() {
                    try {  
                          //
                          var textoBuscar = document.getElementById('Text2').value;
                          var Longitud = textoBuscar.length;
                          if (Longitud == 1){
                              $("#show").load('busActuaciones_2.aspx?tiparch=' + textoBuscar); 
                          }
                          else{
                             var s = $(this).val().toLowerCase().split(" ");
                             $(".busqueda tr:hidden").show();
                             $.each(s, function() {
                             $(".busqueda tr:visible .indexColumn:not(:contains('"
                             + this + "'))").parent().hide();
                             //alert('Escribio: ' + s);
                            });
                          }//if
                    }
                    catch (err) {
                             txt = "Existe un error en esta página.\n\n";
                             txt += "Descripcion del Error : " + err.description + "\n\n";
                             txt += "Presione ok o continue.\n\n";
                             alert(txt);
                    } // try
                }); 
                        
        });
    </script> 
</head>
<body>
    <form id="form1" runat="server">
     <asp:TextBox ID="Text2" runat="server" 
            style="position:absolute;top:11px; left:8px; width: 519px;"></asp:TextBox>
            
     <div id="show" 
                style="border: 1px double #507CD1; Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 39px; width: 523px;height: 269px; overflow:auto;"></div>
    </form>
</body>
</html>
