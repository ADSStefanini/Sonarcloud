<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ErrorPage.aspx.vb" Inherits="coactivosyp.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pagina Error</title>
    <link rel="shortcut icon" type="image/x-icon" href="web_page.ico" />
    <script type="text/javascript" src="Security/Maestros/js/jquery.min.js"></script>
    <script type="text/javascript" src="Security/Maestros/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Security/Maestros/js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Security/Maestros/js/jquery.min.js"></script>
    <link href="assets/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script type="text/javascript" src="Security/Maestros/js/bts-jquery.min.js"></script>
    <script type="text/javascript" src="assets/bootstrap/js/bootstrap.js"></script>
    <style type="text/css">
        div#container {
            position: relative;
            width: 1000px;
            margin-top: 0px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 0px;
            text-align: left;
        }

        body {
            text-align: center;
            margin: 0;
            font-family: Verdana, Geneva, sans-serif;
            background-color: #F9F9F9;
        }

        img {
            border: none;
        }
    </style>
</head>
<body oncontextmenu="return false" onselectstart="return false" ondragstart="return false" oncopy="return false">
    <div id="container">
        <form id="Form1" runat="server">
            <div class="ui-widget-content ui-widget" style="background-color: #ffffff; height: 410px;">
                <div style="margin-top: 0px;">
                    <div style="z-index: 5; top: 0px; left: 0px; background-repeat: no-repeat; background-image: url(Imagenes/chicaplantillanet_2_01.png); height: 86px; width: 1000px; background-color: #ffffff;"></div>
                    <img src="images/imgTop.png" alt="img" />

                </div>
                <div class="row">
                    <div class="col-md-7">
                        <img src="images/ugpp.png" alt="Imagen" />
                    </div>
                    <div class="col-md-5" style="text-align: center; margin-top: 20px">
                        <div>
                            <asp:Label ID="lblError" Width="300px" Height="80px" runat="server" CssClass="ui-widget-header" Font-Bold="True" Font-Size="Large"></asp:Label>
                        </div>
                        <div>
                            <img src="images/Error.png" alt="ImagenError Autor: flaticon" />
                        </div>
                    </div>
                </div>

            </div>

        </form>
    </div>
</body>
</html>
