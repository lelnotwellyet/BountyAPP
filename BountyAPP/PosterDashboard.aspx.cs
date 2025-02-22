using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BountyAPP
{
    public partial class PosterDashboard : System.Web.UI.Page
    {
        string connectionString = "Data Source=DESKTOP-71QL0R7\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProblems();
            }
        }

        protected void btnPostProblem_Click(object sender, EventArgs e)
        {
            string userEmail = Session["UserEmail"]?.ToString();
            if (string.IsNullOrEmpty(userEmail))
            {
                Response.Write("Error: User not logged in!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Problems (Title, Description, Bounty, PosterEmail, SolverEmail, Solved, Solution, Paid) " +
                             "VALUES (@Title, @Description, @Bounty, @Email, '', 0, '', 0)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@Description", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Bounty", decimal.Parse(TextBox3.Text));
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

            switch (e.CommandName)
            {
                case "Approve":
                    UpdateProblemStatus(problemId, true, false);
                    break;
                case "Reject":
                    UpdateProblemStatus(problemId, false, false);
                    break;
                case "PaySolver":
                    UpdateProblemStatus(problemId, true, true);
                    break;
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
            string userEmail = Session["UserEmail"]?.ToString();
            if (string.IsNullOrEmpty(userEmail)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                    ProblemID, 
                    Title, 
                    Description, 
                    Bounty, 
                    SolverEmail,
                    Solved,
                    Solution,
                    Paid
                    FROM Problems 
                    WHERE PosterEmail = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
    }
}