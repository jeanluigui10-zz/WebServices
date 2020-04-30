<%@ Page Language="C#" AutoEventWireup="true"  Async="true" CodeBehind="CallService.aspx.cs" Inherits="xWebServices.CallService" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtUrl" value="Url" runat="server" Width="800"></asp:TextBox><br />
            <asp:Button ID="btnRequest" runat="server" Text="Send Request" OnClick="btnRequest_Click" /><br />
            <asp:TextBox ID="txtRequest" TextMode="MultiLine" Text="Request" Width="800" Height="500" runat="server"></asp:TextBox><br />         
            <asp:TextBox ID="txtResponse" TextMode="MultiLine" Text="" Width="800" Height="300" runat="server"></asp:TextBox>
        </div>
    </form>
 
</body>
</html>
