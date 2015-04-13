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
    <header class="sectionHeader">Activity Log</header>
    <asp:Panel class="reportPanel" ID="panActivity" runat="server" Visible="True">
        <form id="form1" runat="server" class="tableWrapper">
            <div class="tableRow">
                <p><label for="panActivity_drpUsers">User</label></p>
                <p><asp:DropDownList ID="panActivity_drpUsers" runat="server" autofocus></asp:DropDownList></p>
            </div>
            <div class="tableRow">
                <p><label for="panActivity_drpBillType">Bill Type</label></p>
                <p><asp:DropDownList ID="panActivity_drpBillType" runat="server">
                    <asp:ListItem Value="ALL">All Bill Types</asp:ListItem>
                    <asp:ListItem Value="DSBELOW10KW">DSBELOW10KW (Spot Billing)</asp:ListItem>
                    <asp:ListItem>LS</asp:ListItem>
                    <asp:ListItem>SP</asp:ListItem>
                    <asp:ListItem>MS</asp:ListItem>
                    <asp:ListItem>DSABOVE10KW</asp:ListItem>
                    </asp:DropDownList></p>
            </div>
            <div class="tableRow">
                <p><label for="panActivity_drpDuration">Duration</label></p>
                <p><asp:DropDownList ID="panActivity_drpDuration" runat="server">
                    <asp:ListItem Value="day1">Today</asp:ListItem>
                    <asp:ListItem Value="day2">2 Days</asp:ListItem>
                    <asp:ListItem Value="day3">3 Days</asp:ListItem>
                    <asp:ListItem Value="week">Week</asp:ListItem>
                    <asp:ListItem Value="month">Month</asp:ListItem>
                    <asp:ListItem Value="year">Year</asp:ListItem>
                    <asp:ListItem Value="dates">Enter Dates</asp:ListItem>
                    </asp:DropDownList></p>
            </div> 
            <div class="tableRow exactDate">
                <p><label for="sDate">Start Date</label></p>
                <p><input type="text" runat="server" id="sDate" placeholder="DD/MM/YYYY"/></p>
            </div> 
            <div class="tableRow exactDate">
                <p><label for="sDate">End Date</label></p>
                <p><input type="text" runat="server" id="eDate" placeholder="DD/MM/YYYY"/></p>
            </div>
            <div class="tableRow">
                <p></p>
                <p><asp:Button ID="panActivity_btnAddUser" Text="Activity Log" runat="server" onclick="btnUserActivity_Click" /></p>
            </div>
            <div class="tableRow">
                <p></p>
                <p><asp:Label ID="panActivity_lblMsg" class="msg" runat="server"></asp:Label></p>
            </div>
        </form>
    </asp:Panel>
    <footer id="pageFooter" class="pageFooter"></footer>
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

            //date handling
            var isChrome = /chrom/.test(navigator.userAgent.toLowerCase());
            var d = new Date();
            var day = d.getDate();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            day = (day < 10) ? "0" + day : day;
            month = (month < 10) ? "0" + month : month;
            if ($("#sDate").val() == "") {
                if (isChrome) {
                    $("#sDate").val(new Date().toJSON().substring(0, 10));
                }
                else {
                    $("#sDate").val(day + '/' + month + '/' + year);
                }
            }
            if ($("#eDate").val() == "") {
                if (isChrome) {
                    $("#eDate").val(new Date().toJSON().substring(0, 10));
                }
                else {
                    $("#eDate").val(day + '/' + month + '/' + year);
                }
            }
            

            $(".exactDate").removeClass("tableRow");
            $(".exactDate").addClass("hide");
            function exactDates()
            {
                var drpDur = $("#panActivity_drpDuration option:selected").val();
                if (drpDur == "dates") {
                    $(".exactDate").removeClass("hide");
                    $(".exactDate").addClass("tableRow");
                }
                else {
                    $(".exactDate").removeClass("tableRow");
                    $(".exactDate").addClass("hide");
                }
            }
            exactDates();//needed incase of FF refresh while Exact Date is selected in drpdown
            $("#panActivity_drpDuration").change(exactDates);
            $("#panActivity_btnAddUser").click(function () {
                $("#panActivity_lblMsg").html("");
            })
        });
    </script>
</body>
</html>
