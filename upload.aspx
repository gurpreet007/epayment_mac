<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="upload" %>

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
</head>
<body> 
    <header class="page" id="pageHeader">	</header>
	<div id="loginInfo">
        <asp:Label ID="lblLoggedInAs" runat="server"></asp:Label> 
	</div>
    <nav id="pageNav"></nav>
    <header class="sectionHeader">Upload Billing Data</header>
    <form runat="server" class="tableWrapper">
        <div class="tableRow">
            <p></p>
            <p>
                <input type="radio" class="billClass" name="billClass" value="sap" checked>SAP
                <input type="radio" class="billClass" name="billClass" value="nonsap">Non-SAP
            </p>
        </div>
        <div class="tableRow">
            <p> <label for="drpBillType">Bill Type</label></p>
            <p id="ddlHere">
                <select ID='drpBillType' runat='server'>
                    <option Value='BT'>Bill Type</option>
                </select>
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
                <asp:HiddenField ID="hidBillType" value = "0" runat="server" />
            </p>
        </div>
    </form>
    <footer id="pageFooter" class="pageFooter">	</footer>
    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script>
        $(function () {
            function ChangeVals() {
                var value = $("input[type='radio'][name='billClass']:checked").val();
                if (value == "sap") {
                    FillSAP();
                }
                else {
                    FillNonSAP();
                }
                SetHidBillType();
            }
            function SetHidBillType() {
                var billClass = $("input[type='radio'][name='billClass']:checked").val();
                var billType = $("#drpBillType").val()
                var billComb = billClass + "-" + billType;
                //alert("setting: " + billComb);
                $("#hidBillType").val(billComb);
            }
            function HidReplace() {
                var hidVal = $("#hidBillType").val();
                if (hidVal != "0") {
                    //alert(hidVal);
                    var billClass = hidVal.split("-")[0];
                    var billType = hidVal.split("-")[1];
                    //$("input[type='radio'][name='billClass']:checked").val(billClass);
                    //$("#drpBillType").val(billType)
                    $('input[name=billClass]').val([billClass]);
                    ChangeVals();
                    $("#drpBillType").val(billType)
                }
            }
            function FillNonSAP() {
                var el;
                $("#drpBillType").html("");
                el = document.createElement("option");
                el.textContent = "Bill Type";
                el.value = "BT"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "DSBELOW10KW (Spot Billing)";
                el.value = "DSBELOW10KW"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "LS";
                el.value = "LS"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "SP";
                el.value = "SP"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "MS";
                el.value = "MS"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "DSABOVE10KW";
                el.value = "DSABOVE10KW"
                $("#drpBillType").append(el);
            }
            function FillSAP() {
                var el;
                $("#drpBillType").html("");
                el = document.createElement("option");
                el.textContent = "Bill Type";
                el.value = "BT"
                $("#drpBillType").append(el);

                el = document.createElement("option");
                el.textContent = "DSBELOW10KW (Spot Billing)";
                el.value = "DSBELOW10KW"
                $("#drpBillType").append(el);
            }
            FillSAP();
            HidReplace();
            $("#pageHeader").load("resources/snippets.html #snipPageHeader")
            $("#pageNav").load("resources/snippets.html #snipPageNav", function () {
                $("#pageNav li").removeClass("selected");
                $("#pageNav #nvUpload").addClass("selected");
                $("#pageNav").hover(
                    function () { $("#pageNav li").removeClass("selected"); },
                    function () { $("#pageNav #nvUpload").addClass("selected"); }
                );
            });
            $("#pageFooter").load("resources/snippets.html #snipPageFooter");
            $("#FileUpload1").click(function () {
                $("#lblMessage").text("");
            });
            $(".billClass").change(ChangeVals);
            $("#drpBillType").change(SetHidBillType);
        });
    </script>
</body>
</html>
