<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upload_test.aspx.cs" Inherits="upload" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
	<title>e-Payment</title>
    <!--[if lt IE 9]>
        <script src="scripts/html5shiv.min.js"></script>        
   <![endif]-->
	<%--<link rel="stylesheet" href="styles/epayment.css">--%>
    <link rel="stylesheet/less" href="styles/epayment.less" type="text/css" />
    <script src="scripts/less.min.js"></script>
    <link href="http://hayageek.github.io/jQuery-Upload-File/uploadfile.min.css" rel="stylesheet">
</head>
<body> 
    <header class="page" id="pageHeader">	</header>
	<div id="loginInfo">
        <asp:Label ID="lblLoggedInAs" runat="server"></asp:Label> 
	</div>
    <nav id="pageNav"></nav>
    <header class="sectionHeader">Upload File Test</header>
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
            <p><label for="FileUpload1">Select Bill File</label></p>
		    <p><asp:FileUpload ID="FileUpload1" runat="server"/></p>
        </div>
        <div class="tableRow">
            <p></p>
            <p><asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" 
                        Text="Upload Bill" required/>
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
            <p>
                <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
                        Text="Error Details" Visible="False" />
                <asp:HiddenField ID="hidSID" runat="server" />
            </p>
        </div>
        <div class="tableRow">
            <p></p>
            <p>
                <div id="fileuploader">Upload</div>
            </p>
        </div>
    </form>
    <footer id="pageFooter" class="pageFooter">	</footer>
    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script src="http://hayageek.github.io/jQuery-Upload-File/jquery.uploadfile.min.js"></script>
    <script>
        $(function () {
            $("#pageHeader").load("resources/snippets.html #snipPageHeader")
            $("#pageNav").load("resources/snippets.html #snipPageNav", function () {
                $("#pageNav li").removeClass("selected");
                $("#pageNav #nvUploadTest").addClass("selected");
                $("#pageNav").hover(
                    function () { $("#pageNav li").removeClass("selected"); },
                    function () { $("#pageNav #nvUploadTest").addClass("selected"); }
                );
            });
            $("#pageFooter").load("resources/snippets.html #snipPageFooter");
            $("#FileUpload1").click(function () {
                $("#lblMessage").text("");
            });
            $("#fileuploader").uploadFile({
                url: "download.aspx/GetSomething",
                fileName: "myfile"
            });
        });
    </script>
</body>
</html>