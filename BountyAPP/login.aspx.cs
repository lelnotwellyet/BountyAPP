using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace BountyAPP
{
    public partial class login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Clicked(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-71QL0R7\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Users WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", TextBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", TextBox2.Text.Trim());

                    try
                    {
                        conn.Open();
                        object role = cmd.ExecuteScalar();

                        if (role != null)
                        {
                            string userRole = role.ToString().Trim().ToLower();
                            string userEmail = TextBox1.Text.Trim();

                            // Store both email and role in session
                            Session["UserEmail"] = userEmail;
                            Session["UserRole"] = userRole;

                            // If user is a solver, set SolverEmail as well
                            if (userRole == "solver")
                            {
                                Session["SolverEmail"] = userEmail;
                            }

                            // Debug output
                            Response.Write($"<script>console.log('Login successful - Role: {userRole}, Email: {userEmail}');</script>");

                            if (userRole == "poster")
                            {
                                Response.Redirect("PosterDashboard.aspx", false);
                            }
                            else if (userRole == "solver")
                            {
                                Response.Redirect("SolverDashboard.aspx", false);
                            }
                            else
                            {
                                Response.Write("<script>alert('Unknown Role: " + userRole + "');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid Email or Password');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write($"<script>alert('Database error: {ex.Message}');</script>");
                    }
                }
            }
        }
    }
}