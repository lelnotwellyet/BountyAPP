using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace BountyAPP
{
    public partial class ConfirmOTP : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BountyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] == null)
                {
                    lblMessage.Text = "Session expired. Please log in again.";
                    Response.Redirect("login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = $"Verifying OTP for: {Session["UserEmail"]}";
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                lblMessage.Text = "Session expired. Please log in again.";
                return;
            }

            string userEmail = Session["UserEmail"].ToString();
            string enteredOTP = txtOTP.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = @"
                        SELECT TOP 1 Id, Role, OTP, IsVerified
                        FROM Users
                        WHERE Email = @Email
                        ORDER BY Id DESC";

                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["Id"]);
                                string storedOTP = reader["OTP"].ToString().Trim();
                                string userRole = reader["Role"].ToString().Trim();
                                bool isVerified = Convert.ToBoolean(reader["IsVerified"]);

                                reader.Close();

                                if (isVerified)
                                {
                                    lblMessage.Text = "Account already verified. Redirecting...";
                                    RedirectToDashboard(userRole);
                                }
                                else if (storedOTP.Equals(enteredOTP, StringComparison.OrdinalIgnoreCase))
                                {
                                    VerifyUser(conn, userId);
                                    Session["UserRole"] = userRole;
                                    Session["IsVerified"] = true;

                                    lblMessage.Text = "OTP Verified! Redirecting...";
                                    RedirectToDashboard(userRole);
                                }
                                else
                                {
                                    lblMessage.Text = "Invalid OTP. Please try again.";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "User not found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
            }
        }

        private void VerifyUser(SqlConnection conn, int userId)
        {
            string updateQuery = "UPDATE Users SET IsVerified = 1 WHERE Id = @UserId";
            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
            {
                updateCmd.Parameters.AddWithValue("@UserId", userId);
                updateCmd.ExecuteNonQuery();
            }
        }

        private void RedirectToDashboard(string role)
        {
            string redirectUrl = role.Equals("Poster", StringComparison.OrdinalIgnoreCase)
                ? "~/PosterDashboard.aspx"
                : "~/SolverDashboard.aspx";

            Response.Redirect(redirectUrl, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
