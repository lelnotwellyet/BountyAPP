using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace BountyAPP
{
    public partial class SubmitSolution : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProblemDetails();
            }
        }

        private void LoadProblemDetails()
        {
            int problemID;
            if (int.TryParse(Request.QueryString["ProblemID"], out problemID))
            {
                string connectionString = "Data Source=LAPTOP-TIKAU9EV\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Title, Description FROM Problems WHERE ProblemID = @ProblemID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProblemID", problemID);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblProblemTitle.Text = "Problem Title: " + reader["Title"].ToString();
                                lblDescription.Text = "Problem Description: " + reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid problem ID.');</script>");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int problemID;
            if (int.TryParse(Request.QueryString["ProblemID"], out problemID))
            {
                string solverEmail = Session["UserEmail"].ToString();
                string solutionText = txtSolution.Text.Trim();

                if (string.IsNullOrEmpty(solutionText))
                {
                    Response.Write("<script>alert('Please enter a solution.');</script>");
                    return;
                }

                string connectionString = "Data Source=LAPTOP-TIKAU9EV\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Problems SET Solution = @Solution WHERE ProblemID = @ProblemID AND SolverEmail = @SolverEmail";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Solution", solutionText);
                        cmd.Parameters.AddWithValue("@ProblemID", problemID);
                        cmd.Parameters.AddWithValue("@SolverEmail", solverEmail);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Write("<script>alert('Solution submitted successfully!'); window.location='SolverDashboard.aspx';</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Error submitting solution. Please try again.');</script>");
                        }
                        conn.Close();
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid problem ID.');</script>");
            }
        }
    }
}
