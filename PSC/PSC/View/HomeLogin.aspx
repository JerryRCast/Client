<%@ Page Title="Login" Language="C#" MasterPageFile="~/View/HomePage.Master" AutoEventWireup="true" CodeBehind="HomeLogin.aspx.cs" Inherits="PSC.View.HomeLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <div class="divLoginHome">
        <br />
        <br />
        <div class="labelHeaderLog">Iniciar de sesión</div>
        <br />
        <div><asp:TextBox ID="userNameT" runat="server" CssClass="textBoxLogin" placeholder="Ingrese usuario..."></asp:TextBox></div>
        <br />
        <div><asp:TextBox ID="rfcT" runat="server" CssClass="textBoxLogin" placeholder="Ingrese RFC de la empresa..."></asp:TextBox></div>
        <div><asp:label ID="errorLengthRFC" runat="server" Text=" " CssClass="labelLogin"/></div>
        <br />
        <div><asp:TextBox ID="passwordT" type="password" runat="server" CssClass="textBoxLogin" placeholder="Ingrese contraseña..."></asp:TextBox></div>
        <br />
        <div><asp:Button ID="loginB" runat="server" Text="Entrar" class="btnLogin" OnClick="loginB_Click"/></div>
        <div><asp:label ID="errorSesion" runat="server" Text=" " CssClass="labelLogin" /></div>
        <div><asp:label ID="ClientErrors" runat="server" Text=" " CssClass="labelLogin" /></div>
        <br />
        <br />
        <br />
    </div>

</asp:Content>
