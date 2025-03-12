using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BountyAPP
{
    public partial class PosterDashboard : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BountyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!IsPostBack)
            {
                LoadProblems();
                LoadUserWallet(Session["UserEmail"].ToString());
            }
        }

        protected void btnPostProblem_Click(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Write("Error: Session expired.");
                return;
            }

            string userEmail = Session["UserEmail"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Problems (Title, Description, Bounty, PosterEmail, SolverEmail, Solved, Solution, Paid) " +
                               "VALUES (@Title, @Description, @Bounty, @Email, '', 0, '', 0)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", TextBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", TextBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Bounty", decimal.Parse(TextBox3.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadProblems();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int problemId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "PaySolver")
            {
                Session["ProblemID"] = problemId;
                UpdateProblemStatus(problemId, true, true);
                Response.Redirect("payment gateway.aspx",false);

            }
            else if (e.CommandName == "Approve")
            {
                UpdateProblemStatus(problemId, true, false); // Set only Solved to true
            }

            LoadProblems();
        }

        private void UpdateProblemStatus(int problemId, bool solved, bool paid)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Problems SET Solved = @Solved, Paid = @Paid WHERE ProblemID = @ProblemID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProblemID", problemId);
                    cmd.Parameters.AddWithValue("@Solved", solved);
                    cmd.Parameters.AddWithValue("@Paid", paid);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadProblems()
        {
            if (Session["UserEmail"] == null) return;
            string userEmail = Session["UserEmail"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProblemID, Title, Description, Bounty, SolverEmail, Solved, Solution, Paid FROM Problems WHERE PosterEmail = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", userEmail);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
        protected void LoadUserWallet(string userEmail)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT wallet FROM Users WHERE Email = @UserEmail";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserEmail",userEmail);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    Session["Wallet"] = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    Label1.Text = "Wallet Balance: " + Session["Wallet"];   
                }
            }
        }



    }
}
