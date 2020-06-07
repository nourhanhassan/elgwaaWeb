<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="QVControlPanel.error._500" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>اوقافى</title>
    <link href="/Areas/ControlPanel/Content/css/styles.css" rel="stylesheet" />
</head>
<body class="focusedform">
    <div class="verticalcenter">
        <img src="/Areas/ControlPanel/Content/img/logo-big.png" alt="Logo" class="brand" />
        <div class="panel panel-primary perspective ">
            <div class="panel-body login">
                <h5 class="text-center" style="margin-bottom: 25px;">عذراً لقد حدث خطأ ما</h5>
            </div>
            <div class="panel-footer">
            </div>
        </div>
    </div>
</body>
</html>
<!--
<%
    Exception ex = Server.GetLastError();
    if(ex != null)
    {
        Response.Write(ex.Message);
    }
%>
-->
