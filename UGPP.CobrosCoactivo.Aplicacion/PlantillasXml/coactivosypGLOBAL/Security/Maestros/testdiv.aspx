<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="testdiv.aspx.vb" Inherits="coactivosyp.testdiv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Creación y asignación de expedientes</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                $("#alnkFiltros").click(function(event) {
                    event.preventDefault();
                    $("#divFiltros").slideToggle();
                });

                $("#divFiltros a").click(function(event) {
                    event.preventDefault();
                    $("#divFiltros").slideUp();
                });
            });
        </script>
        
        <style type="text/css">		    
		    #divFiltros { display: none; }
	    </style>
    </head>
    
    <body>
        <form id="form1" runat="server">
                <a href="#" id="alnkFiltros">TRIGGER</a>
                <div id="divFiltros">                    
                    <p>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren</p>
                </div>
        </form>
    </body>
</html>
