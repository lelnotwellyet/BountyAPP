// SolverDashboard.aspx.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BountyAPP
{
    public partial class SolverDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            lblUserEmail.Text = Session["UserEmail"].ToString();

            if (!IsPostBack)
            {
                LoadProblems();
            }
        }

        private void LoadProblems()
        {
            string connectionString = "Data Source=LAPTOP-TIKAU9EV\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        ProblemID, 
                        Title, 
                        Description, 
                        Bounty, 
                        SolverEmail as ClaimedBy, 
                        CASE WHEN Solved = 0 OR Solved IS NULL THEN 'No' ELSE 'Yes' END as Solved 
                    FROM Problems 
                    ORDER BY ProblemID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Response.Write($"<script>alert('Error loading problems: {ex.Message}')</script>");
                    }
                }
            }
        }

        protected void btnClaim_Click(object sender, EventArgs e)
        {
            string solverEmail = Session["UserEmail"]?.ToString();
            if (string.IsNullOrEmpty(solverEmail))
            {
                Response.Write("<script>alert('Please log in again.')</script>");
                return;
            }

            Button btn = (Button)sender;
            int problemID = Convert.ToInt32(btn.CommandArgument);
            string connectionString = "Data Source=LAPTOP-TIKAU9EV\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Problems SET SolverEmail = @SolverEmail WHERE ProblemID = @ProblemID AND (SolverEmail IS NULL OR SolverEmail = '')",
                    conn))
                {
                    cmd.Parameters.AddWithValue("@SolverEmail", solverEmail);
                    cmd.Parameters.AddWithValue("@ProblemID", problemID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Response.Write("<script>alert('Problem claimed successfully!')</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Problem could not be claimed. It may have already been taken.')</script>");
                    }
                }
            }
            LoadProblems();
        }
        protected void btnSubmitSolution_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int problemID = Convert.ToInt32(btn.CommandArgument);

            // Redirect to the solution submission page
            Response.Redirect($"SubmitSolution.aspx?ProblemID={problemID}");
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}