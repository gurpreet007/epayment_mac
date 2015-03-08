<%@ Page Language="C#" AutoEventWireup="true" CodeFile="download.aspx.cs" Inherits="download" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
	<title>e-Payment</title>
    <!--[if lt IE 9]>
        <script src="scripts/html5shiv.min.js"></script>        
   <![endif]-->
    <link rel="stylesheet/less" href="styles/epayment.less" type="text/css" />
    <script src="scripts/less.min.js"></script>
</head>
<body> 
    <header class="page" id="pageHeader">	</header>
	<div id="loginInfo">
        <asp:Label ID="lblLoggedInAs" runat="server"></asp:Label> 
	</div>
    <nav id="pageNav"></nav>
    <header class="sectionHeader">Download Billing Data</header>
    <form runat="server" class="tableWrapper">
        <div class="tableRow">
            <p> <label for="drpBillType">Bill Type</label></p>
            <p>
                <asp:DropDownList ID="drpBillType" runat="server">
                    <asp:ListItem Selected="True" Value="BT">Bill Type</asp:ListItem>
                    <asp:ListItem Value="DSBELOW10KW">DSBELOW10KW (Spot Billing)</asp:ListItem>
                    <asp:ListItem>LS</asp:ListItem>
                    <asp:ListItem>SP</asp:ListItem>
                    <asp:ListItem>MS</asp:ListItem>
                    <asp:ListItem>DSABOVE10KW</asp:ListItem>
                    </asp:DropDownList>
            </p>
        </div>
        <div class="tableRow">
            <p></p>
            <p><input type="button" ID="btnDownload"  Value="Show Count" />
            </p>
        </div>
        <div class="tableRow">
            <p></p>
            <p>
                <span class="msg"><asp:Label ID="lblMessage" runat="server" class="msg"></asp:Label></span>
            </p>
        </div>
        <div class="tableRow">
            <p></p>
            <p><input type="button" ID="btnEmpty" runat="server" Value="Empty All Tables" />
            </p>
        </div>
    </form>
    <footer id="pageFooter" class="pageFooter">	</footer>
    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script>
        $(function () {
            $("#pageHeader").load("resources/snippets.html #snipPageHeader")
            $("#pageNav").load("resources/snippets.html #snipPageNav", function () {
                $("#pageNav li").removeClass("selected");
                $("#pageNav #nvDownload").addClass("selected");
                $("#pageNav").hover(
                    function () { $("#pageNav li").removeClass("selected"); },
                    function () { $("#pageNav #nvDownload").addClass("selected"); }
                );
            });
            $("#pageFooter").load("resources/snippets.html #snipPageFooter");
            
            $("#btnDownload").click(function () {
                var billType = $('#drpBillType option:selected').val();
                if (billType == "BT") {
                    $("#lblMessage").html("Select a Bill Type");
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "download.aspx/GetSomething",
                    data: JSON.stringify({ 'drpVal': billType }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $("#lblMessage").html(response.d);
                    },
                    error: function (error) {
                        $("#lblMessage").html(error.statustext);
                    }
                });
            });
            $("#btnEmpty").click(function () {
                $.ajax({
                    type: "POST",
                    url: "download.aspx/EmptyTables",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $("#lblMessage").html(response.d);
                    },
                    error: function (error) {
                        $("#lblMessage").html(error.statusText);
                    }
                });
            });
        });
    </script>
</body>
</html>