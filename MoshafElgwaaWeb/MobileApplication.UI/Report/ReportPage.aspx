<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPage.aspx.cs" Inherits="MobileApplication.UI.Report.ReportPage" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style>
        body:nth-of-type(1) img[src*="Blank.gif"]{display:none;}
        [alt^="Export"] { transform: rotateY(-180deg); }
        table[title='Refresh'] { display: none; }
    </style>
  
    <script src="/Scripts/jquery-1.10.2.js"></script>
</head>
<body dir="rtl" style="width: 100%; height:100% ; overflow: hidden;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="text-align:center; height:95%; overflow: hidden; margin: 0 auto;">
            <rsweb:ReportViewer Height="100%" Width="95%" ID="repv" runat="server" style="margin: 0 auto; ">
            </rsweb:ReportViewer>
            &nbsp;
        </div>
    </form>
</body>
</html>


<script type="text/javascript">
    $(function () {
        $("div[id$='AsyncWait_Wait']").html('<div><img src="ajaxloader.gif" /><br />جارِ تحميل الملف ...</div>');
        var sel = $("[id$=Menu]");
        sel.find("a[title*='PDF']").text("إستخراج إلى PDF");
        sel.find("a[title*='Word']").text("إستخراج إلى Word");
        sel.find("a[title*='Excel']").text("إستخراج إلى Excel");
        sel.find("a[title*='XML']").remove();
        sel.find("a[title*='CSV']").remove();
        sel.find("a[title*='MHTML']").remove();
        sel.find("a[title*='TIFF']").remove();
        sel.find("[title='Refresh']").remove();
        //sel.css({ 'z-index': '1000', 'width': '150px' });
        $("[id$=Menu]").css('left', '140px !important');
        $('span').filter(function () { return ($(this).text() === 'of') }).text('من');
        $('a[title="Find"]').text('بحث');
        $('a[title="Find Next"]').text('التالي');

    });

    // for landscape reports
    Sys.Application.add_load(function () {
        $find("<%= repv.ClientID %>").add_propertyChanged(viewerPropertyChanged);
    });
    function viewerPropertyChanged(sender, e) {
        if (e.get_propertyName() == "isLoading") {
            if ($find("<%= repv.ClientID %>").get_isLoading()) {
                // Do something when loading starts
            }
            else {
                // Do something when loading stops
                try {
                    $('#<%= repv.ClientID %> [dir]:first').css('overflow-x', 'scroll').scrollLeft(100);
                } catch (err) { }
            }
        }
    };

    // resize report viewer
    ResizeReport();
    function ResizeReport() {
        var viewer = document.getElementById('<%= repv.ClientID %>');
        var htmlheight = document.documentElement.clientHeight;
        viewer.style.height = (htmlheight - 30) + "px";
    }
    window.onresize = function resize() { ResizeReport(); }
</script>