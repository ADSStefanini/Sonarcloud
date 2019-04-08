<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="detallespredio.aspx.vb" Inherits="coactivosyp.detallespredio" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        div#ejprediofondo {background:transparent url('../images/1320950948_home.png') 220px 85px no-repeat;margin:0px;}
    </style>
    <!--[if IE7]> <style type="text/css"> div#ejprediofondo {background:transparent url('../images/1320950948_home.png') 220px 85px no-repeat;} </style><![endif]-->
</head>
<body>
    <form id="form1" runat="server">
      <div id ="ejprediofondo" style="">
           <h2 style="font-size:18px;text-transform:uppercase;margin:0;color:#FFF;padding:10px 10px 10px 45px;-moz-border-radius-topleft:10px;-moz-border-radius-topright:10px;-webkit-border-top-left-radius:10px;-webkit-border-top-right-radius:10px;background:#507cd1 url(images/icons/icoproperty.png) 7px 5px no-repeat">
                Informacion detallada del predio
           </h2>
           <div style="margin:10px 10px 25px;padding-bottom:5px;border-bottom:1px solid #507cd1;color:#000;" id="predio_info" runat="server">
              
           </div>
       </div>
    </form>
</body>
</html>
