<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignUpForm.ascx.cs" Inherits="Connect.Controls.SignUpForm" %>

<%-- Header Info - Email Address and Password Fields --%>
<asp:UpdatePanel ID="headerFieldsPnl" runat="server" EnableViewState="true" ChildrenAsTriggers="true" UpdateMode="Conditional" ViewStateMode="Enabled">
<Triggers>
</Triggers>
<ContentTemplate>
<div class="form" id="headerinfo">

<%-- Email  Address Field --%>
<div class="row">
<span class="label">Email Address</span>
<span class="field">
<asp:TextBox ID="emailTxt" TextMode="Email" AutoCompleteType="Email" runat="server"></asp:TextBox>
<asp:RegularExpressionValidator ID="signInNameValid" runat="server" ValidationGroup="registration" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="emailTxt" ErrorMessage="Please enter a valid email address" Display="Dynamic" CssClass="validationMsg" />
<asp:RequiredFieldValidator ID="rvEmail" runat="server" ControlToValidate="emailTxt" ErrorMessage="Please provide us with your email address" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>

<%-- Password field --%>
<div class="row">
<span class="label">Password</span>
<span class="field">
<asp:TextBox ID="passTxt" TextMode="Password" CssClass="pass" runat="server"></asp:TextBox>
<asp:RegularExpressionValidator ID="passValid" runat="server" ValidationGroup="registration" ControlToValidate="passTxt" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ErrorMessage="Password must be 8 characters or longer and include at least one capital letter, one number, and any of these valid special characters (!,@,$,%,&,*)" Display="Dynamic" CssClass="validationMsg" />
<asp:RequiredFieldValidator ID="rvPassword" runat="server" ControlToValidate="passTxt" ErrorMessage="Please provide us with a password" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>

<%-- Verify Password field --%>
<div class="row">
<span class="label">Verify Password</span>
<span class="field">
<asp:TextBox ID="passVerifyTxt" CssClass="passVerify" TextMode="Password" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="verifypassValid" runat="server" ControlToValidate="passVerifyTxt" ErrorMessage="Please reenter your password" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="passTxt" ControlToValidate="passVerifyTxt" ErrorMessage="Passwords do not match" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration"></asp:CompareValidator>
<div id="passwordMatch" class="validationMsg"></div>
</span>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<%-- Sign Up Form Panel - Details specific for different user types --%>
<asp:UpdatePanel ID="signupFormPnl" runat="server" EnableViewState="true" ChildrenAsTriggers="true" UpdateMode="Conditional" ViewStateMode="Enabled">
<Triggers>
<asp:AsyncPostBackTrigger ControlID="countryDDL" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="stateDDL" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="areaDDL" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="employerDDL" EventName="SelectedIndexChanged" />
</Triggers>
<ContentTemplate>
<div class="form" id="signupinfo">

<%-- First Name --%>
<div class="row">
<span class="label">First Name</span>
<span class="field">
<asp:TextBox ID="firstNameTxt" AutoCompleteType="FirstName" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvFirstName" runat="server" ControlToValidate="firstNameTxt" ErrorMessage="Please provide us with your first name" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>

<%-- Last Name --%>
<div class="row">
<span class="label">Last Name</span>
<span class="field">
<asp:TextBox ID="lastNameTxt" AutoCompleteType="LastName" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvLastName" runat="server" ControlToValidate="lastNameTxt" ErrorMessage="Please provide us with your last name" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>

<%-- Birth Date --%>
<div class="row">
<span class="label">Birthdate</span>
<span class="field">
<asp:TextBox ID="birthdayTxt" TextMode="Date" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvBirthday" runat="server" ControlToValidate="birthdayTxt" ErrorMessage="Please provide us with your birthday" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
<asp:CustomValidator ID="ageValid" runat="server" ValidationGroup="registration" ControlToValidate="birthdayTxt" OnServerValidate="ageValid_ServerValidate" ErrorMessage="You must be 21 or older to use Connect" Display="Dynamic" CssClass="validationMsg" />
</span>
</div>

<%-- Country --%>
<div class="row">
<span class="label">Country</span>
<span class="field">
<asp:DropDownList ID="countryDDL" AutoPostBack="true" EnableViewState="true" ViewStateMode="Enabled" runat="server"></asp:DropDownList>
<asp:RequiredFieldValidator ID="rvCountry" runat="server" ControlToValidate="countryDDL" InitialValue="" ErrorMessage="Please provide us with your country" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>

<%-- State - Conditionally Visible --%>
<div class="row">
<asp:Panel ID="statePnl" Visible="false" runat="server">
<span class="label">State</span>
<span class="field">
<asp:DropDownList ID="stateDDL" AutoPostBack="true" EnableViewState="true" ViewStateMode="Enabled" runat="server"></asp:DropDownList>
<asp:RequiredFieldValidator ID="rvRequiredState" runat="server" ControlToValidate="stateDDL" InitialValue="" ErrorMessage="Please provide us with your state" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</asp:Panel>
</div>

<%-- Employer Panel -  Conditionally Visible --%>
<asp:Panel ID="empddl" runat="server" Visible="false">
<div class="row">
<span class="label">Employer</span>
<span class="field">
<asp:DropDownList ID="employerDDL" AutoPostBack="true" EnableViewState="true" ViewStateMode="Enabled" runat="server"></asp:DropDownList>
<asp:RequiredFieldValidator ID="rvEmployer" runat="server" ControlToValidate="employerDDL" InitialValue="" ErrorMessage="Please provide us with your employer" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</asp:Panel>
</div>

<%-- Creative Agency Panel - Conditionally Visible --%>
<asp:Panel ID="creative_agencypnl" runat="server" Visible="false">

<div class="row">
<span class="label">Employer</span>
<span class="field">
<asp:TextBox ID="employerTxt" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvEmployerTxt" ControlToValidate="employerTxt" runat="server" InitialValue="" ErrorMessage="Please provide us with your employer" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>
<div class="row">

<span class="label">Brand</span>
<span class="field">
<asp:TextBox ID="brand" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvBrand" ControlToValidate="brand" runat="server" InitialValue="" ErrorMessage="Please provide us with the brand that you work for" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>
</asp:Panel>

<%-- Distributor Panel - To be visible only for distributors and contractors --%>
<asp:Panel ID="distributorPnl" runat="server" Visible="false">

<%-- Area - Conditionally Visible --%>
<div class="row">
<asp:Panel runat="server" ID="areaPnl" Visible="false">
<span class="label">Area</span>
<span class="field">
<asp:DropDownList ID="areaDDL" AutoPostBack="true" EnableViewState="true" ViewStateMode="Enabled" runat="server"></asp:DropDownList>
<asp:RequiredFieldValidator ID="rvArea" runat="server" ControlToValidate="areaDDL" InitialValue="" ErrorMessage="Please provide us with your area" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</asp:Panel>
</div>

<%-- On/Off Premise --%>
<div class="row">
<span class="label">On Off Premise</span>
<span class="field">
<asp:DropDownList ID="onOffPremiseDDL" EnableViewState="true" ViewStateMode="Enabled" runat="server">
<asp:ListItem></asp:ListItem>
<asp:ListItem>On</asp:ListItem>
<asp:ListItem>Off</asp:ListItem>
<asp:ListItem>Both</asp:ListItem>
</asp:DropDownList>
<asp:RequiredFieldValidator ID="rvOnOffPremise" runat="server" ControlToValidate="onOffPremiseDDL" InitialValue="" ErrorMessage="Please let us know if you are on-premise, off-premise, or both" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
</span>
</div>
</asp:Panel>
<%-- End of Distributor Panel --%>

<%-- Beam Suntory Sponsor Email Panel --%>
<asp:Panel ID="sponsorPnl" runat="server" Visible="false">
<div class="row">
<span class="label">Beam Suntory Sponsor Email</span>
<span class="field">
<asp:TextBox ID="beamSuntorySponsorTxt" AutoCompleteType="Email" TextMode="Email" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rvbeamSuntorySponsor" runat="server" ControlToValidate="beamSuntorySponsorTxt" ErrorMessage="Please provide us with your Beam Suntory sponsor" Display="Dynamic" CssClass="validationMsg" ValidationGroup="registration" />
<asp:CustomValidator ID="sponserValid" runat="server" ValidationGroup="registration" ControlToValidate="beamSuntorySponsorTxt" Enabled="false" OnServerValidate="sponserValid_ServerValidate" ErrorMessage="Please enter a Beam Suntory email address for your sponsor." Display="Dynamic" CssClass="validationMsg" />
</span>
</div>
</asp:Panel>
    </div>
      <%-- End of form --%>

</ContentTemplate>
</asp:UpdatePanel>
        

<%-- Sign Up Button --%>
<div class="row">
<asp:Button ID="signUpBtn" CssClass="btn" ValidationGroup="registration" UseSubmitBehavior="true" runat="server" Text="Sign Up" OnClick="signUpBtn_Click" />
</div>
    

<asp:Panel ID="successMsg" CssClass="successMsg" runat="server"></asp:Panel>
        
<%-- CSS Styles --%>
<style>
    .inlinepanel {
        display: inline-block;
    }

    .row .btn {
        float: right;
        width: auto;
    }

    .row .check > input {
        text-align: left;
        width: auto;
        margin-top: 1.5em;
    }

    .successMsg {
        top: -4.3em;
        background: #fff;
        position: relative;
    }

        .successMsg .btn {
            color: #333;
        }

    .validationMsg {
        color: #CC0A0A;
    }
</style>

<script type="text/javascript">
    // Password Comparison
    jQuery(document).ready(function () {
        //PasswordComparison();
    });
    function TradeAutocomplete() {
        if (typeof (tradeEmployers) != 'undefined') {
            var html = "<ul>";
            jQuery.each(tradeEmployers, function (i, n) {
                html += "<li class='employer'>" + n + "</li>";
            });
            html == "</ul>";
            jQuery('.tradeEmployerAuto').append(html);
            jQuery('.tradeEmployer').click(function () {
                jQuery('.tradeEmployerAuto').css('display', 'block');
                jQuery(document).click(function () {
                    if (jQuery(this).attr('class') != "employer")
                        jQuery('.tradeEmployerAuto').css('display', 'none');
                });
            });
            jQuery('.tradeEmployerAuto li').click(function () {
                jQuery('.tradeEmployer').val(jQuery(this).text());
                jQuery('.tradeEmployerAuto').css('display', 'none');
            });
        }
    }

    function PasswordComparison() {
        jQuery('.passVerify').keyup(function () {
            if (jQuery(this).val() != jQuery('.pass').val())
                jQuery('#passwordMatch').text("Passwords do not match")
            else
                jQuery('#passwordMatch').text("");
        });
    }
</script>
