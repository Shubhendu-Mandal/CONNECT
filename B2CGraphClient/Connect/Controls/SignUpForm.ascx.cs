#define PROD
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GraphApi;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Text.RegularExpressions;

namespace Connect.Controls
{
    public partial class SignUpForm : System.Web.UI.UserControl
    {
        List<Classes.CascadingDropDown> countrycdl = Classes.Helpers.GetCountries();
#if DEV
        private static string tenant = "BSIB2CDev.onmicrosoft.com";
        private static string id = "61eb9d21-9472-4af2-8b39-69647f309aa8";
        private static string secret = "cSU4McpAIvojzd9TOYcxB6xeqoyBXplRxiHZS3/9rIY=";
        //private static string secret = "7VNxTdHqru1Uqfyw+Qj0ZzVYQLdYgnNB6C0kt0ZXsn4=";
#endif
#if PROD
        private static string tenant = "extbeamsuntory.onmicrosoft.com";
        private static string id = "46d0355e-649f-4797-8bec-340e124fd149";
        private static string secret = "i1b5Ep0Im/MIDOv6I3qPBHwqI6r+Y7OiXpyPoaDj+7A=";
        //private static string secret = "8NhTs6hDCn5s9VMOvkGrtDi7rHZZPlhNXYGzdlGZGgg="; --old key
#endif
        internal bool TradeUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get user type from query string
            string SF_Roleid = Request.QueryString["usertype"];

            //Load fields respective to each user type
            countryDDL.SelectedIndexChanged += CountryDDL_SelectedIndexChanged;
            stateDDL.SelectedIndexChanged += StateDDL_SelectedIndexChanged;

            if (!Page.IsPostBack)
            {
                countryDDL.DataSource = countrycdl;
                countryDDL.DataValueField = "SystemValue";
                countryDDL.DataTextField = "Root";
                countryDDL.DataBind();
                //Distributor or Beam Suntory Contractor
                if ((SF_Roleid == "distributor") || (SF_Roleid == "beamsuntorycontractor"))
                {

                    empddl.Visible = true;
                    employerDDL.DataSource = Classes.Helpers.GetEmployers();
                    employerDDL.DataBind();
                    distributorPnl.Visible = true;
                    sponsorPnl.Visible = true;

                }
                else if (SF_Roleid == "creative_agency")
                {
                    creative_agencypnl.Visible = true;
                    rvEmployerTxt.Enabled = true;
                    rvBrand.Enabled = true;
                    sponsorPnl.Visible = true;


                }
                else if ((SF_Roleid == "trade") || (SF_Roleid == "promo_agency"))
                {
                    empddl.Visible = true;
                    employerDDL.DataSource = Classes.Helpers.GetTradeEmployers();
                    employerDDL.DataBind();

                }

            }
        }
        private string GetRegion()
        {
            string country = countryDDL.SelectedItem.Value;
            List<string> Americas = new List<string>
            {
                "Argentina",
                "Belize",
                "Bolivia, Plurinational State of",
                "Brazil",
                "Canada",
                "Chile",
                "Colombia",
                "Costa Rica",
                "Ecuador",
                "El Salvador",
                "Falkland Islands (Malvinas)",
                "French Guiana",
                "Guatemala",
                "Guyana",
                "Honduras",
                "Mexico",
                "Nicaragua",
                "Panama",
                "Paraguay",
                "Peru",
                "Suriname",
                "Uruguay",
                "Venezuela, Bolivarian Republic of",
                "South Georgia and the South Sandwich Islands",
                "United States"
            };
            if (Americas.Contains(country))
                return "Americas";
            return "International";
        }

        private string GetCommercialRegion()
        {
            string state = string.Empty;
            if (TradeUser)
                return null;
            if (stateDDL.SelectedIndex > 0)
            {
                state = stateDDL.SelectedItem.Value;
            }
            else
            {
                return null;
            }
            switch (state)
            {
                case "California":
                case "Hawaii":
                case "Alaska":
                case "Arizona":
                case "Nevada":
                case "New Mexico":
                case "Washington":
                    return "SGWS West Region";
                case "Arkansas":
                case "Illinois":
                case "Kansas":
                case "Louisiana":
                case "Minnesota":
                case "Texas":
                    return "SGWS Central Region";
                case "Florida":
                case "New York":
                case "Delaware":
                case "Kentucky":
                case "Ohio - Low Proof":
                case "South Carolina":
                    return "SGWS East Region";
                case "Georgia":
                case "Maryland":
                case "Mont. Cty.":
                case "Tennessee":
                case "Washington DC":
                case "Indiana":
                case "Colorado":
                case "Nebraska":
                case "North Dakota":
                case "Oklahoma":
                case "South Dakota":
                    return "RNDC Region";
                case "Connecticut":
                case "Massachusetts":
                case "Missouri":
                case "New Jersey":
                case "Rhode Island":
                case "Wisconsin":
                case "Alabama":
                case "Maine":
                case "Mississippi":
                case "New Hampshire":
                case "North Carolina":
                case "Pennsylvania":
                case "Vermont":
                case "Virginia":
                case "West Virginia":
                case "Idaho":
                case "Iowa":
                case "Michigan":
                case "Montana":
                case "Ohio - High Proof":
                case "Oregon":
                case "Utah":
                case "Wyoming":
                    return "Franchise and Control States Region";
                //case "Georgia":
                // case "Maryland":
                //case "District of Columbia":
                //case "Nebraska":
                //case "Indiana":
                //case "Colorado":
                //case "North Dakota":
                //case "Oklahoma":
                //case "South Dakota":
                //return "Central Region";
                default:
                    return null;
            }
        }

        internal string GetDivision(string State)
        {
            switch (State)
            {
                case "California":
                case "Hawaii":
                    return "California/Hawaii Division";
                case "Alaska":
                case "Arizona":
                case "Nevada":
                case "New Mexico":
                case "Washington":
                    return "Mountain Division";
                // case "Arkansas":
                //case "Illinois":
                //case "Kansas":
                //case "Louisiana":
                // case "Minnesota":
                // case "Texas":
                //return "Central Division";
                //case "Delaware":
                case "Florida":
                    return "Florida Division";
                case "New York":
                    return "New York Division";
                //case "South Carolina":
                //return "East Coast Division";
                case "Delaware":
                case "Kentucky":
                case "Ohio - Low Proof":
                case "South Carolina":
                    return "Mid-Atlantic Division";
                case "Connecticut":
                case "Massachusetts":
                case "Missouri":
                case "New Jersey":
                case "Rhode Island":
                case "Wisconsin":
                    return "Franchise Region";
                case "Alabama":
                case "Maine":
                case "Mississippi":
                case "New Hampshire":
                case "North Carolina":
                //case "Pennsylvania":
                case "Vermont":
                case "Virginia":
                case "West Virginia":
                    return "Eastern Control Division";
                case "Idaho":
                case "Iowa":
                //case "Michigan":
                case "Montana":
                //case "Ohio":
                case "Ohio-High Proof":
                case "Oregon":
                case "Utah":
                case "Wyoming":
                    return "Western Control Division";
                //case "Nebraska":
                //case "Indiana":
                //case "Colorado":
                //case "North Dakota":
                //case "Oklahoma":
                //case "South Dakota":
                // return "Central Control Division";
                default:
                    return null;
            }
        }

        internal void SendNewUserEmail(B2CUser NewUser)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();

                m.From = new MailAddress("connect@beamsuntory.com", "Beam Suntory Connect");
                m.To.Add(new MailAddress("someone@beamsuntory.com", "Someones Name"));
                m.Subject = "New User Pending Approval in Connect - " + NewUser.mail;
                m.Body = "";
                m.IsBodyHtml = true;
                string html = "";
                AlternateView av = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                m.AlternateViews.Add(av);

                sc.Host = "smtp.beamsuntory.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("from@username.com", "password");
                sc.EnableSsl = true;
                sc.Send(m);
            }
            catch (Exception) {  /* expected until we get full email login details */}
        }

        #region Events
        protected void signUpBtn_Click(object sender, EventArgs e)
        {

            //Capturing the current language of the browser

            string lang = HttpContext.Current.Request.UserLanguages[0];

            Page.Validate("registration");
            if (!Page.IsValid)
                return;
            bool TradeUser;
            bool accountStatus;
            string SF_Roleid = Request.QueryString["usertype"];

            //Trade accounts will be activated automatically whereas Distributor and agent accounts need to be activated manually in B2C
            if ((SF_Roleid == "distributor") || (SF_Roleid == "beamsuntorycontractor"))
            {
                accountStatus = false;
                SF_Roleid = "Dist_ID";
                TradeUser = false;
            }
            else if ((SF_Roleid == "trade") || (SF_Roleid == "promo_agency"))
            {
                accountStatus = true;
                SF_Roleid = "Trade_ID";
                TradeUser = true;

            }
            else
            {
                accountStatus = false;
                SF_Roleid = "Agency_ID";
                TradeUser = false;

            }

            //Create new user object
            GraphApi.B2CNewUser newUser = new B2CNewUser
            {

                accountEnabled = accountStatus,
                signInNames = new SignInName[] { new SignInName
                    {
                        type = "emailAddress",
                        value = emailTxt.Text
                    }
                },
                displayName = firstNameTxt.Text + " " + lastNameTxt.Text,
                creationType = "LocalAccount",
                givenName = firstNameTxt.Text,
                mailNickname = emailTxt.Text.Split('@')[0],
                passwordProfile = new PasswordProfile
                {
                    password = passTxt.Text,
                    forceChangePasswordNextLogin = false
                },
                passwordPolicies = "DisablePasswordExpiration",
                streetAddress = emailTxt.Text,
                surname = lastNameTxt.Text,
                usageLocation = "US",
                userType = "member",
                extension_a67c327875c34024bd4523a3d66619ba_Birthdate = birthdayTxt.Text,
                extension_a67c327875c34024bd4523a3d66619ba_Email = emailTxt.Text,
                extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsRoleID = SF_Roleid,
                extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsID = (Guid.NewGuid()).ToString().Replace("-", "").Substring(0, 20) + "_EXT",
                country = (countryDDL.SelectedIndex > 0) ? countryDDL.SelectedItem.Value : null,
                state = (stateDDL.SelectedIndex > 0) ? stateDDL.SelectedItem.Value : null,
                extension_a67c327875c34024bd4523a3d66619ba_Region = GetRegion()
            };

            if (SF_Roleid == "Dist_ID")
            {
                newUser.extension_a67c327875c34024bd4523a3d66619ba_Employer = (employerDDL.SelectedIndex > 0) ? employerDDL.SelectedItem.Value : null;
                newUser.extension_a67c327875c34024bd4523a3d66619ba_BeamSuntorySponsor = beamSuntorySponsorTxt.Text;
                newUser.extension_a67c327875c34024bd4523a3d66619ba_Area = (areaDDL.SelectedIndex > 0) ? areaDDL.SelectedItem.Value : null;
                newUser.extension_a67c327875c34024bd4523a3d66619ba_OnOffPremise = (onOffPremiseDDL.SelectedIndex > 0) ? onOffPremiseDDL.SelectedItem.Value : null;
                newUser.extension_a67c327875c34024bd4523a3d66619ba_CommercialRegion = GetCommercialRegion();
                newUser.extension_a67c327875c34024bd4523a3d66619ba_Division = (stateDDL.SelectedIndex > 0 && countryDDL.SelectedItem.Value == "United States") ? GetDivision(stateDDL.SelectedItem.Value) : null;

            }
            else
            {
                if (SF_Roleid == "Agency_ID")
                {
                    newUser.extension_a67c327875c34024bd4523a3d66619ba_Employer = employerTxt.Text;
                    newUser.extension_a67c327875c34024bd4523a3d66619ba_BeamSuntoryBrand = brand.Text;
                    newUser.extension_a67c327875c34024bd4523a3d66619ba_BeamSuntorySponsor = beamSuntorySponsorTxt.Text;

                }
                else
                {
                    newUser.extension_a67c327875c34024bd4523a3d66619ba_Employer = (employerDDL.SelectedIndex > 0) ? employerDDL.SelectedItem.Value : null;
                    TradeUser = true;
                }
            }
            GraphApi.AADUser aad = new AADUser(tenant, id, secret);
            GraphApi.B2CUser user = null;
            try
            {
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                user = aad.CreateUser(newUser);
            }
            catch (Exception ex)
            {
                if (lang.Contains("de"))
                {
                    successMsg.Controls.Add(new LiteralControl("<h1>Whoops!</h1><p>Wir konnten Ihre Registrierung zu diesem Zeitpunkt nicht abschließen. Wenn Sie weiterhin Probleme haben, wenden Sie sich bitte an <a href='mailto:connect@beamsuntory.com'>connect@beamsuntory.com</a> zur Hilfe.</p><a class='btn' href='/'>OK</a>" + ex.Message));

                }

                else
                {
                    successMsg.Controls.Add(new LiteralControl("<h1>Whoops!</h1><p>We were unable to complete your registration at this time. If you are still experiencing issues, please contact <a href='mailto:connect@beamsuntory.com'>connect@beamsuntory.com</a> for assistance.</p><a class='btn' href='/'>OK</a>" + ex.Message));
                }
                    
                successMsg.Controls.Add(new LiteralControl("<style>.form .row {display:none !important;}.footer .btn {display:none;}</style>"));
                successMsg.Controls.Add(new LiteralControl("<div class='error' style='display:none'><div>New User Details:</div><div>" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "</div></div>"));
                signupFormPnl.Update();
            }
#if DEBUG
            successMsg.Controls.Add(new LiteralControl("<div style='display:none'><div>New User Details:</div><div>" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "</div></div>"));
#endif
            if (user != null)
            {
                if (user.userType != "Error")
                {
                    //woo... also indicate to the user that they have been signed up pending approval
                    //send pending user email to admin
                    creative_agencypnl.Visible = false;
                    sponsorPnl.Visible = false;
                    if (lang.Contains("de"))
                    {
                        successMsg.Controls.Add(new LiteralControl("<h1>Fast fertig!</h1><p> Überprüfe dein inbox!</p><a class='btn' href='/'>OK</a>"));

                    }

                    else
                    {
                        successMsg.Controls.Add(new LiteralControl("<h1>Almost Done!</h1><p> Check your inbox!</p><a class='btn' href='/'>OK</a>"));
                    }

                    
                    /// commeted the below code to accomodate a new alert message post new user creation process
                    /// 4/19/2017 updated by amal requester  Matt Mason
                    //if (distributeChk.Checked)
                    //    successMsg.Controls.Add(new LiteralControl("<h1>Registration Complete!</h1><p>You have successfully registered for Beam Suntory CONNECT! Your registration is pending approval which can take 1 - 2 business days.</p><a class='btn' href='/'>OK</a>"));
                    //else
                    //    successMsg.Controls.Add(new LiteralControl("<h1>Registration Complete!</h1><p>You have successfully registered for Beam Suntory CONNECT! Please note it will take 15 minutes for content to appear on your page upon logging in for the first time. Learning courses should become available for you in approximately  1-2 business days.</p><a class='btn' href='/'>Login</a>"));
                    //bool TradeUser = (distributeChk.Checked) ? false : true;
                    Classes.Helpers.LogUserChange(TradeUser, false, GetUserDictionary(user));
                    successMsg.Controls.Add(new LiteralControl("<style>.form .row {display:none !important;}.footer .btn {display:none;}</style>"));
                    
                    signupFormPnl.Update();

#if PROD
                    string webAppURL = "https://connect.beamsuntory.com/";
#endif
#if DEV
                    string webAppURL = "http://azjbbfpspdw1.cloudapp.net/";
#endif


                    

                    SendEmailToCreator(emailTxt.Text, webAppURL, lang);
                }
                else
                {
#if PROD
                    string resetUrl = "https://passwordreset.microsoftonline.com/?ru=https%3a%2f%2flogin.microsoftonline.com%2fextbeamsuntory.onmicrosoft.com%2freprocess%3fctx%3drQIIAbVSv4vTcBQnSTt4HCgFQTiQG24S0uSbpL220KG9_rCl_YZrm4vNlqTftN80ybfXfEua4OCqooOIQic5t9vURW_wD7jFcxNHJ3F0ctNWwUXQyeF9eLz3Ph_e4_NucCALSnuWJZkSkk3ezqF9XnFEky8UkMivAzgj2bGBo8wzW1e2P78RMzuN6tPlzt3vW6_erZjraEktZPrhIqBkHmdJ4GN7TkLi0KxN_FOmP6F0FpYEwSNjHGR_d0ng4QBtZgSKhL-rCMRc0IkkbHCOwtlrhrlgmDssc49NAVGWV2waBbzWf8Yyp-w1o9uDN_UjywirpD5LLOdAV6ZRufyRvaxWNjobIHOcoE9_VL6w6Z97fmXTxws0jx9xV8kMBXhkkyBANs3iESVTFKy4-wyK24O-dohV3FY6Okg6OoxMXaS2381DSYuhD13D1-jQNTA8EEVDryedwThnDLoUNrVo6E4ltdmYGrqWtHCEj-peveUSrDYPl119KA2TCoUunKhrLqyN_DU3hrUhhX7bg66tDJMqNtzKshWIL7l_-HDG7YkyyOWkEeJFVCzwShEB3txfZw6QRUsumnkpv3_ObSPfxN7ur6M_cMxFivmWYk7Sa_Prj58_fPvgdu3Fe547eZLJnqeFTk-YjYuJE_eaqpsb3QLuMozCQQGAmNZacmV23OsaSWOAvaislMDZpf_xDT8A0&mkt=en-US&hosted=0&device_platform=Windows+10&nca=1&domain_hint=extbeamsuntory.onmicrosoft.com";
#endif
#if DEV
                    string resetUrl = "https://passwordreset.microsoftonline.com/?ru=https%3a%2f%2flogin.microsoftonline.com%2fbsib2cdev.onmicrosoft.com%2freprocess%3fctx%3drQIIAa1SO2zTUBSV7UQChARCQiJMIHUCOX7v2Wk-UoZ8HOQS28JxEmyxOPZz8kzslyavpPHExAALqjplKowdEUKoYmXJ1BExMIBYEBNiYKQBiaVi63DPcO_R0T3n3lsCzMPKxmCAPIRlT_QLuCgqIfDEUgkD8aRgGMihD0NleuXC5S83V_u5Tnr3xdf39PX3vU9LLlfvaHXUaOJHeZrExJ_SGQ1Z3qfxIWeMGJvMKpI0pkOS5P9NaTImCV5zJIalwYwMkB-cFpCot8NGSFrjFM8mbznumOMe89xTPgOBLC_5LE7EbueA5w75a2V6W5F7buB4eq_kFSwE5sloWK1-5C-ZtbXOGuiUpPjzqc43PvtnxR98dnsHTxd7wlU6wQkJfJok2Gd5EjD6ECdL4RmHF1t2p3uPmGRLafdh2u4bc68PmB_rm06kAzfWFCetMdPuym4HACeyxm17RAykMh1p0G12gWu7sRu1iEbmpKeOVS2ixIk15EStyG06zLB9oDcAMGxr1LaHyOyrzEEqMJp6wUhVxWx2FS0Br4Tcf8M7Eja8YmEAoQJE4CNZVHw5EEtyaVMMoOwPQhmgchGthIs49sj4xl-_HwTuOMP9ynAvsyfXfvLz3O6DaF8_eH79Xd1tvFllpbYlTYblNFxYd8yoENyH0e5sPrNLEC5YU5Nrk21Ld9OWTcbzarECj86f8Q_8Bg2&mkt=en-US&hosted=0&device_platform=Windows+10&nca=1&domain_hint=BSIB2CDev.onmicrosoft.com";
#endif
                    if (lang.Contains("de"))

                    {
                        successMsg.Controls.Add(new LiteralControl("<h1>Whoops!</h1><p>Wir konnten Ihre Registrierung nicht abschließen, da Ihr Konto bereits vorhanden ist. Klicken Sie auf die Schaltflächen unten, um sich entweder anzumelden oder Ihr Passwort zurückzusetzen. Wenn Sie weiterhin Probleme haben, wenden Sie sich bitte an <a href='mailto:connect@beamsuntory.com'>connect@beamsuntory.com</a> zur Hilfe.</p><a class='btn' href='/'>Login</a> <a class='btn' href='" + resetUrl + "'>Reset Password</a>"));

                    }

                    else

                    {
                        successMsg.Controls.Add(new LiteralControl("<h1>Whoops!</h1><p>We were unable to complete your registration because your account already exists. Click the buttons below to either log in or to reset your password. If you are still experiencing issues, please contact <a href='mailto:connect@beamsuntory.com'>connect@beamsuntory.com</a> for assistance.</p><a class='btn' href='/'>Login</a> <a class='btn' href='" + resetUrl + "'>Reset Password</a>"));


                    }

                        
                    successMsg.Controls.Add(new LiteralControl("<style>.form .row {display:none !important;}.footer .btn {display:none;}</style>"));
                    successMsg.Controls.Add(new LiteralControl("<div class='error' style='display:none'><div>New User Details:</div><div>" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "</div></div>"));
                    signupFormPnl.Update();
                }
            }
        }
        protected void SendEmailToCreator(string currentUserEmailID, string webAppURL, string lang)
        {
            try
            {
                if (currentUserEmailID != string.Empty)
                {
                    SPWebApplication webApp = SPWebApplication.Lookup(new Uri(webAppURL));
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("connect@beamsuntory.com");
                    mail.To.Add(currentUserEmailID);
                    if (lang.Contains("de"))

                    {
                        mail.Subject = "Ihre Beam Suntory CONNECT Account wird gebaut!";


                        StringBuilder sbBodyText = new StringBuilder();
                        sbBodyText.AppendLine("<body style = 'font-family=\"sans -serif\"'>");
                        sbBodyText.AppendLine("Setz dich fest. Wir können hier nicht jeden reinlassen.");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("Das CONNECT-Team wird Ihre Anmeldeanfrage prüfen. Sobald Ihre Anmeldung genehmigt ist, erhalten Sie in ca. 3-5 Werktagen eine E-Mail mit dem Betreff 'Willkommen bei CONNECT'.");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("&emsp;-Die Begrüßungs-E-Mail enthält die Schritte zur 'Erstanmeldung'.");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("Bitte halten Sie für die Begrüßungs-E-Mail!");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("Wenn Sie in der Zwischenzeit Fragen haben, senden sie eine Email an <a href='mailto:CONNECT@Beamsuntory.com'>CONNECT@BeamSuntory.com</a> und wir helfen Ihnen gerne weiter!");
                        sbBodyText.AppendLine("<br /><br/>");
                        sbBodyText.AppendLine("Vielen Dank");
                        sbBodyText.AppendLine("<br /><br/>");
                        sbBodyText.AppendLine("<img src=cid:connectlogo  id='img' alt='' width='210px' height='53px'/>");
                        sbBodyText.AppendLine("</body>");

                        AlternateView av = AlternateView.CreateAlternateViewFromString(sbBodyText.ToString(), null, MediaTypeNames.Text.Html);
                        string path = Server.MapPath(@"Icons/connect email logo.png");
                        LinkedResource lr = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                        lr.ContentId = "connectlogo";
                        av.LinkedResources.Add(lr);
                        mail.AlternateViews.Add(av);
                        mail.Body = sbBodyText.ToString();
                        mail.IsBodyHtml = true;
                        // SmtpClient class sends the email by using the specified SMTP server
                        SmtpClient smtp = new SmtpClient(webApp.OutboundMailServiceInstance.Server.Address);
                        smtp.UseDefaultCredentials = true;
                        smtp.Send(mail);


                    }

                    else
                    {

                        mail.Subject = "Your Beam Suntory CONNECT Account is being built!";


                        StringBuilder sbBodyText = new StringBuilder();
                        sbBodyText.AppendLine("<body style = 'font-family=\"sans -serif\"'>");
                        sbBodyText.AppendLine("Sit tight. We can’t let just anybody in here.");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("The CONNECT team will review your sign-up request.  Once approved you will receive a 'Welcome to CONNECT' email in approx. 3-5 business days, letting you know you’re ready to go. ");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("&emsp;-The Welcome email will include 'First time login' steps");
                        sbBodyText.AppendLine("<br />");
                        sbBodyText.AppendLine("&emsp;-For those logging in with an iPad, there will be a guide showing you how to download the SuccessFactors Mobile Learning App");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("Please hold for the Welcome email!");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("Included in some of the great CONNECT content you’ll soon have access to is our Virtual Distillery Tour experience.  While you are waiting on your account buildout, why not <a href='http://vdex-jb.movmobile.com/' target='_blank'>tour our Jim Beam Distillery,</a> virtually!");
                        sbBodyText.AppendLine("<br /><br />");
                        sbBodyText.AppendLine("In the meantime, if you have questions, please forward this email to the CONNECT inbox at <a href='mailto:CONNECT@Beamsuntory.com'>CONNECT@BeamSuntory.com</a> and we will be happy to help!");
                        sbBodyText.AppendLine("<br /><br/>");
                        sbBodyText.AppendLine("Thank you");
                        sbBodyText.AppendLine("<br /><br/>");
                        sbBodyText.AppendLine("<img src=cid:connectlogo  id='img' alt='' width='210px' height='53px'/>");
                        sbBodyText.AppendLine("</body>");


                    
                    AlternateView av = AlternateView.CreateAlternateViewFromString(sbBodyText.ToString(),null, MediaTypeNames.Text.Html);
                    string path = Server.MapPath(@"Icons/connect email logo.png");
                    LinkedResource lr = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                    lr.ContentId = "connectlogo";
                    av.LinkedResources.Add(lr);
                    mail.AlternateViews.Add(av);
                    mail.Body = sbBodyText.ToString();
                    mail.IsBodyHtml = true;
                    // SmtpClient class sends the email by using the specified SMTP server
                    SmtpClient smtp = new SmtpClient(webApp.OutboundMailServiceInstance.Server.Address);
                    smtp.UseDefaultCredentials = true;
                    smtp.Send(mail);


                    }
                }
            }
            catch (Exception error)
            {
            }
        }

        internal Dictionary<string, string> GetUserDictionary(B2CUser User)
        {
            Dictionary<string, string> details = new Dictionary<string, string>();
            details.Add("preferredName", User.displayName);
            details.Add("country", User.country);
            details.Add("state", User.state);
            details.Add("FirstName", firstNameTxt.Text);
            details.Add("LastName", lastNameTxt.Text);
            //#if DEV
            if (User.extension_a67c327875c34024bd4523a3d66619ba_Birthdate != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_Birthdate", User.extension_a67c327875c34024bd4523a3d66619ba_Birthdate);
            if (User.extension_a67c327875c34024bd4523a3d66619ba_Email != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_Email", User.extension_a67c327875c34024bd4523a3d66619ba_Email);
            if (User.extension_a67c327875c34024bd4523a3d66619ba_Employer != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_Employer", User.extension_a67c327875c34024bd4523a3d66619ba_Employer);
            if (User.extension_a67c327875c34024bd4523a3d66619ba_TradeEmployer != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_TradeEmployer", User.extension_a67c327875c34024bd4523a3d66619ba_TradeEmployer);
            if (User.extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsRoleID != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsRoleID", User.extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsRoleID);
            if (User.extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsID != null)
                details.Add("extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsID", User.extension_a67c327875c34024bd4523a3d66619ba_SuccessFactorsID);
            /*#endif
            #if PROD
                        if (User.extension_be6dc6c96b4c411780751b4231962926_Birthdate != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_Birthdate", User.extension_be6dc6c96b4c411780751b4231962926_Birthdate);
                        if (User.extension_be6dc6c96b4c411780751b4231962926_Email != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_Email", User.extension_be6dc6c96b4c411780751b4231962926_Email);
                        if (User.extension_be6dc6c96b4c411780751b4231962926_Employer != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_Employer", User.extension_be6dc6c96b4c411780751b4231962926_Employer);
                        if (User.extension_be6dc6c96b4c411780751b4231962926_TradeEmployer != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_TradeEmployer", User.extension_be6dc6c96b4c411780751b4231962926_TradeEmployer);
                        if (User.extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsRoleID != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsRoleID", User.extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsRoleID);
                        if (User.extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsID != null)
                            details.Add("extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsID", User.extension_be6dc6c96b4c411780751b4231962926_SuccessFactorsID);
            #endif*/
            // if (distributeChk.Checked)
            if ((Request.QueryString["usertype"] == "distributor") || (Request.QueryString["usertype"] == "beamsuntorycontractor") || (Request.QueryString["usertype"] == "creative_agency"))
            {
                TradeUser = false;
                if (!String.IsNullOrEmpty(beamSuntorySponsorTxt.Text))
                    details.Add("extension_a67c327875c34024bd4523a3d66619ba_BeamSuntorySponsor", beamSuntorySponsorTxt.Text);

                if ((Request.QueryString["usertype"] == "distributor") || (Request.QueryString["usertype"] == "beamsuntorycontractor"))
                {

                    //#if DEV

                    if (User.extension_a67c327875c34024bd4523a3d66619ba_Region != null)
                        details.Add("extension_a67c327875c34024bd4523a3d66619ba_Region", User.extension_a67c327875c34024bd4523a3d66619ba_Region);
                    if (User.extension_a67c327875c34024bd4523a3d66619ba_Area != null)
                        details.Add("extension_a67c327875c34024bd4523a3d66619ba_Area", User.extension_a67c327875c34024bd4523a3d66619ba_Area);
                    if (User.extension_a67c327875c34024bd4523a3d66619ba_OnOffPremise != null)
                        details.Add("extension_a67c327875c34024bd4523a3d66619ba_OnOffPremise", User.extension_a67c327875c34024bd4523a3d66619ba_OnOffPremise);
                    if (!String.IsNullOrEmpty(GetCommercialRegion()))
                        details.Add("extension_a67c327875c34024bd4523a3d66619ba_CommercialRegion", GetCommercialRegion());
                    if (User.extension_a67c327875c34024bd4523a3d66619ba_Division != null)
                        details.Add("extension_a67c327875c34024bd4523a3d66619ba_Division", User.extension_a67c327875c34024bd4523a3d66619ba_Division);
                }
                /*#endif
                #if PROD
                                if (User.extension_be6dc6c96b4c411780751b4231962926_Region != null)
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_Region", User.extension_be6dc6c96b4c411780751b4231962926_Region);
                                if (!String.IsNullOrEmpty(beamSuntorySponsorTxt.Text))
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_BeamSuntorySponsor", beamSuntorySponsorTxt.Text);
                                if (User.extension_be6dc6c96b4c411780751b4231962926_Area != null)
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_Area", User.extension_be6dc6c96b4c411780751b4231962926_Area);
                                if (User.extension_be6dc6c96b4c411780751b4231962926_OnOffPremise != null)
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_OnOffPremise", User.extension_be6dc6c96b4c411780751b4231962926_OnOffPremise);
                                if (!String.IsNullOrEmpty(GetCommercialRegion()))
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_CommercialRegion", GetCommercialRegion());
                                if (User.extension_be6dc6c96b4c411780751b4231962926_Division != null)
                                    details.Add("extension_be6dc6c96b4c411780751b4231962926_Division", User.extension_be6dc6c96b4c411780751b4231962926_Division);
                #endif*/
            }
            return details;
        }
        private void CountryDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (countrycdl[countryDDL.SelectedIndex].Children.Count() > 0)
            {
                // populate state values with list items that have parent assigned to them
                statePnl.Visible = true;
                List<Classes.CascadingDropDown> states = countrycdl[countryDDL.SelectedIndex].Children;
                stateDDL.DataSource = states;
                stateDDL.DataValueField = "SystemValue";
                stateDDL.DataTextField = "Root";
                stateDDL.DataBind();
                signupFormPnl.Update();
            }
            else
            {
                // unselect and hide state dropdown if country doesn't have states
                stateDDL.SelectedIndex = -1;
                statePnl.Visible = false;

                // if area was visible due to certain state selection, hide and unselect
                if (areaPnl.Visible)
                {
                    areaDDL.SelectedIndex = -1;
                    areaPnl.Visible = false;
                }

                signupFormPnl.Update();
            }
        }

        private void StateDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SF_Roleid = Request.QueryString["usertype"];
            if ((SF_Roleid == "distributor") || (SF_Roleid == "beamsuntorycontractor"))
            {
                string state = stateDDL.SelectedItem.Text;
                List<string> texas = new List<string> { "", "Dallas/Ft Worth", "Austin/San Antonio", "Houston/South TX", "Chain Sales", "Inside Sales" };
                List<string> newyork = new List<string> { "", "UNY", "MNY", "Inside Sales" };
                List<string> florida = new List<string> { "", "NFL", "SFL", "Chains", "Inside Sales", "Chain Sales" };
                List<string> california = new List<string> { "", "NCA", "SCA", "Chain Sales", "Inside Sales" };
                List<string> nebraska = new List<string> { "", "General Sales", "Inside Sales" };
                List<string> illinois = new List<string> { "", "General Sales", "Inside Sales" };
                List<string> minnesota = new List<string> { "", "General Sales", "Inside Sales" };
                List<string> louisiana = new List<string> { "", "General Sales", "Inside Sales" };
                List<string> arizona = new List<string> { "", "General Sales", "Inside Sales" };
                List<string> ohiolowproof = new List<string> { "", "Low Proof" };
                List<string> ohiohighproof = new List<string> { "", "High Proof" };
                switch (state)
                {
                    case "Texas":
                        areaDDL.DataSource = texas;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "New York":
                        areaDDL.DataSource = newyork;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Florida":
                        areaDDL.DataSource = florida;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "California":
                        areaDDL.DataSource = california;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Nebraska":
                        areaDDL.DataSource = nebraska;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Illinois":
                        areaDDL.DataSource = illinois;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Minnesota":
                        areaDDL.DataSource = minnesota;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Louisiana":
                        areaDDL.DataSource = louisiana;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Arizona":
                        areaDDL.DataSource = arizona;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Ohio - Low Proof":
                        areaDDL.DataSource = ohiolowproof;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    case "Ohio - High Proof":
                        areaDDL.DataSource = ohiohighproof;
                        areaDDL.DataBind();
                        areaPnl.Visible = true;
                        signupFormPnl.Update();
                        break;
                    default:
                        areaPnl.Visible = false;
                        signupFormPnl.Update();
                        break;
                }
            }
        }
        #endregion

        protected void passTxt_TextChanged(object sender, EventArgs e)
        {
            ViewState["pass"] = passTxt.Text;
        }

        protected void sponserValid_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string SF_roleid = Request.QueryString["usertype"];
            if ((SF_roleid != "distributor") || (SF_roleid != "beamsuntorycontractor") || (SF_roleid != "creative_agency"))
                args.IsValid = true;
            else if (((SF_roleid == "distributor") || (SF_roleid != "beamsuntorycontractor")) || (SF_roleid == "creative_agency") && !String.IsNullOrEmpty(beamSuntorySponsorTxt.Text))
            {
                if (beamSuntorySponsorTxt.Text.Contains("@"))
                {
                    if (beamSuntorySponsorTxt.Text.Split('@')[0].ToLower() == "beamsuntory.com")
                        args.IsValid = true;
                    else
                        args.IsValid = false;
                }
                else
                    args.IsValid = false;
            }
            else
                args.IsValid = false;
        }


        protected void ageValid_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (!String.IsNullOrEmpty(birthdayTxt.Text))
                {
                    
                    TimeSpan ts = DateTime.Now - (Convert.ToDateTime(birthdayTxt.Text));
                    if (ts.Days > (365 * 21))
                        args.IsValid = true;
                    else
                        args.IsValid = false;
                }
                else
                    args.IsValid = false;
            }
            catch (Exception)
            { /* invalid date */
                args.IsValid = false;
            }
        }


    }
}