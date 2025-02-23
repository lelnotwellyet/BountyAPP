using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace BountyAPP
{
    public partial class registration : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BountyDB"].ConnectionString;

        // Email configuration
        private static readonly string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
        private static readonly int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        private static readonly string SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
        private static readonly string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (DropDownList1.Items.Count == 0)
                {
                    DropDownList1.Items.Add(new System.Web.UI.WebControls.ListItem("Poster", "Poster"));
                    DropDownList1.Items.Add(new System.Web.UI.WebControls.ListItem("Solver", "Solver"));
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string otp = GenerateOTP(6);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Users (Name, Email, Password, Role, OTP, IsVerified) 
                        VALUES (@Name, @Email, @Password, @Role, @OTP, 0)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", TextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", TextBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", TextBox3.Text);
                        cmd.Parameters.AddWithValue("@Role", DropDownList1.SelectedValue);
                        cmd.Parameters.AddWithValue("@OTP", otp);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            if (SendOTP(TextBox2.Text.Trim(), otp))
                            {
                                Session["UserEmail"] = TextBox2.Text.Trim();
                                Response.Redirect("ConfirmOTP.aspx");
                            }
                            else
                            {
                                lblMessage.Text = "Registration successful but failed to send OTP. Please contact support.";
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Registration failed. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error during registration: {ex.Message}";
            }
        }

        private string GenerateOTP(int length)
        {
            const string chars = "0123456789";
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);
                char[] result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[data[i] % chars.Length];
                }
                return new string(result);
            }
        }

        private bool SendOTP(string toEmail, string otp)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(SmtpUsername);
                    mail.To.Add(toEmail);
                    mail.Subject = "Your OTP for BountyAPP Registration";
                    mail.Body = $@"
                        <html>
                        <body>
                            <h2>Welcome to BountyAPP!</h2>
                            <p>Your One-Time Password (OTP) for registration is:</p>
                            <h3 style='color: #4CAF50;'>{otp}</h3>
                            <p>This OTP will expire soon. Please enter it to complete your registration.</p>
                            <p>If you didn't request this OTP, please ignore this email.</p>
                        </body>
                        </html>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(SmtpHost, SmtpPort))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error somewhere secure
                System.Diagnostics.Debug.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}