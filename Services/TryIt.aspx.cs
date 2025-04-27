using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LMS.Security;

namespace LibraryManagementSystem.Services
{
    public partial class TryIt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string service = Request.QueryString["service"];
                string operation = Request.QueryString["op"];

                if (string.IsNullOrEmpty(service) || string.IsNullOrEmpty(operation))
                {
                    litResults.Text = "Error: Missing service or operation parameters";
                    return;
                }

                // Show service and operation in the header
                litServiceName.Text = service;
                litOperationName.Text = operation;

                // Show the appropriate panel based on the service and operation
                if (service.Equals("SecurityService", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.Equals("HashPassword", StringComparison.OrdinalIgnoreCase))
                    {
                        pnlHashPassword.Visible = true;
                    }
                    else if (operation.Equals("VerifyPassword", StringComparison.OrdinalIgnoreCase))
                    {
                        pnlVerifyPassword.Visible = true;
                    }
                    else
                    {
                        litResults.Text = $"Error: Unknown operation '{operation}' for service '{service}'";
                    }
                }
                else
                {
                    litResults.Text = $"Error: Unknown service '{service}'";
                }
            }
        }

        protected void btnHashPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string password = txtPassword.Text;
                if (string.IsNullOrEmpty(password))
                {
                    litResults.Text = "Please enter a password";
                    return;
                }

                string hashedPassword = SecurityHelper.HashPassword(password);
                litResults.Text = hashedPassword;
            }
            catch (Exception ex)
            {
                litResults.Text = $"Error: {ex.Message}";
            }
        }

        protected void btnVerifyPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string password = txtVerifyPassword.Text;
                string hash = txtHashedPassword.Text;

                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                {
                    litResults.Text = "Please enter both password and hash";
                    return;
                }

                bool isValid = SecurityHelper.VerifyPassword(password, hash);
                litResults.Text = isValid ? "Password is valid" : "Password is invalid";
            }
            catch (Exception ex)
            {
                litResults.Text = $"Error: {ex.Message}";
            }
        }
    }
} 