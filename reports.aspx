<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reports.aspx.cs" Inherits="reports" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8">
	<title>e-Payment</title>
    <!--[if lt IE 9]>
        <script src="scripts/html5shiv.min.js"></script>        
   <![endif]-->
	<%--<link rel="stylesheet" href="styles/epayment.css">--%>
    <link rel="stylesheet/less" href="styles/epayment.less" type="text/css" />
    <script src="scripts/less.min.js"></script>
</head>
<body>  
    <header class="page" id="pageHeader">	</header>
	<div id="loginInfo">
        <asp:Label ID="lblLoggedInAs" runat="server"></asp:Label> 
	</div>
    <nav id="pageNav"></nav>
    <header class="sectionHeader">Reports</header>
    <form id="form1" runat="server" class="tableWrapper">
        <asp:LinkButton ID="lnkUserActivity" runat="server" 
                            onclick="lnkUserActivity_Click">User Activity Log</asp:LinkButton>
    </form>
    <footer id="pageFooter" class="pageFooter">	</footer>
    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script>
        $(document).ready(function () {
            $("#pageHeader").load("resources/snippets.html #snipPageHeader");
            $("#pageNav").load("resources/snippets.html #snipPageNav", function () {
                $("#pageNav li").removeClass("selected");
                $("#pageNav #nvReports").addClass("selected");
                $("#pageNav").hover(
                    function () { $("#pageNav li").removeClass("selected"); },
                    function () { $("#pageNav #nvReports").addClass("selected"); }
                );
            });
            $("#pageFooter").load("resources/snippets.html #snipPageFooter");
        });
    </script>
</body>
</html>

