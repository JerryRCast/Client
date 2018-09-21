<%@ Page Language="C#" Title="Gestión de datos" MasterPageFile="~/View/LoggedMaster.Master" AutoEventWireup="true" CodeBehind="LoggedSession.aspx.cs" Inherits="PSC.View.LoggedSession" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            margin: 5px 10px 0px 10px;
            border-style: none;
            border-color: inherit;
            border-width: medium;
            background-color: #80a7be;
            font-size: 16px;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            -webkit-transition-duration: 0.4s;
            transition-duration: 0.4s;
        }
        .auto-style2 {
            margin: 5px 10px 0px 0px;
            border-style: double;
            border-color: inherit;
            border-width: medium;
            background-color: #fff;
            font-size: 16px;
            color: black;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            -webkit-transition-duration: 0.4s;
            transition-duration: 0.4s;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ MasterType virtualpath="~/View/LoggedMaster.Master" %>

    <div style="text-align:center">
        <ul class="ulcl">
            <li class="ulcl">
                <p style="font-family:Arial">
                    &nbsp;<asp:DropDownList runat="server" ID="areaFilter" CssClass="auto-style2" Height="20px" Width="142px">
                        <asp:ListItem Text="Área" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="formatFilter" runat="server" CssClass="auto-style2" Height="20px" Width="142px">
                        <asp:ListItem Text ="Formato" Value=""></asp:ListItem>
                        <asp:ListItem>pdf</asp:ListItem>
                        <asp:ListItem>txt</asp:ListItem>
                        <asp:ListItem>docx</asp:ListItem>
                    </asp:DropDownList>
                    Fecha:
                    <asp:TextBox ID="DateTxt" runat="server" TextMode="Date" Width="142px" CssClass="list"></asp:TextBox>
                    <asp:Button ID="ApplyFilter" runat="server" Text ="Aplicar Filtros" CssClass="auto-style1" Visible="true" OnClick="ApplyFilter_Click"/>   
            </li>
        </ul>
        <br />
    </div>

    <div style="align-content:center; text-align:center/*; background-color:#80a7be*/">
        
        <asp:Button ID="LoadFilesBtn" runat="server" Text="Cargar archivo(s)" CssClass="btnPag1" Visible="true"/> 
        <asp:Button ID="FinderBtn" runat="server" Text="Buscar" CssClass="btnPag1" Visible="true" OnClick="FinderBtn_Click"/> 
        <asp:TextBox ID="WordFilterTxt" runat="server" Width="200px" TextMode="Search"></asp:TextBox>
        <asp:PlaceHolder runat="server" ID="bMenu" ></asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="dMenu"></asp:PlaceHolder>
    </div>
    <div>
        <asp:Label id="error" runat="server" Text=""></asp:Label>
    </div>
    <br /> <br />
    <div style="text-align: center" class="divLabelFiles">
        <br />
        <asp:label ID="contentFiles" runat="server" Text=" " CssClass="labelFiles"/>
    </div>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server"></asp:ToolkitScriptManager>
    <asp:Panel ID="p1" runat="server" CssClass="panelDisplayed">
        <div style="text-align:center" class="textPanel">Seleccione los archivos a subir</div>
        <br />
        <div style="text-align:center"><asp:FileUpload Multiple ="Multiple" runat="server" ID="FileUpload1" Visible="true" /></div>
        <br />
        <div style="text-align:center">
            <asp:DropDownList runat="server" ID="areaLPanel" CssClass="listArea">
                        <asp:ListItem Text="Seleccione área" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div style="text-align:center">
            <asp:Button ID="panelAccept" runat="server" Text="Aceptar" CssClass="btnPag1" OnClick="panelAccept_Click"/>
            <asp:Button ID="panelCancel" runat="server" Text="Cancelar" CssClass="btnPag1" />
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="panelCancel" PopupControlID="p1" TargetControlID="LoadFilesBtn" BackgroundCssClass="modBackg"></asp:ModalPopupExtender>

    <asp:UpdatePanel ID="UpdtPanel" runat ="server">
        <ContentTemplate>
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" GridLines="None" style="width: 100%" AllowPaging="true" EmptyDataText="No hay archivos agregados">
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <asp:checkbox  ID="checkC" runat="server"></asp:checkbox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="No." ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label  ID="numberT" runat="server" Text='<%# Eval("id") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Nombre archivo"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="name" runat="server" Text='<%# Eval("fileName") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Formato"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="typeT" runat="server" Text='<%# Eval("fileFormat") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Tamaño"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="sizeT" runat="server" Text='<%# Eval("fileSize") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Fecha carga"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="dateLoad" runat="server" Text='<%# Eval("fileLoadDate") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="OID"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="oidT" runat="server" Text='<%# Eval("OID") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Hash"  ItemStyle-HorizontalAlign="Justify">
                        <ItemTemplate>
                            <asp:label ID="strnHash" runat="server" Text='<%# Eval("strnHash") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Estatus"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:label ID="sealFlag" runat="server" Text='<%# Eval("sealFlag") %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                <FooterStyle BackColor="#9BA096" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#9AB976" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#639D5D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#858481" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Applyfilter" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="FinderBtn" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
