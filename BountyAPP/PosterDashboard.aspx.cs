using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
                Response.Redirect("Login.aspx"); // Redirect to login page if not logged in
            }

            if (!IsPostBack)
            {
                LoadProblems();
            }

        }

        protected void btnPostProblem_Click(object sender, EventArgs e)
        {
            string userEmail = Session["UserEmail"]?.ToString(); // Ensure it's not null
            if (string.IsNullOrEmpty(userEmail))
            {
                Response.Write("Error: User not logged in!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Problems (Title, Description, Bounty, PosterEmail, SolverEmail, Solved) VALUES (@Title, @Description, @Bounty, @Email, '', 0)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@Description", TextBox2.Text);
                    if (decimal.TryParse(TextBox3.Text, out decimal bountyValue))
                    {
                        cmd.Parameters.AddWithValue("@Bounty", bountyValue);
                    }
                    else
                    {
                        Response.Write("Invalid bounty amount!");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    Response.Write("Problem posted successfully!");
                    LoadProblems();


                }


            }
        }
        protected void btnClaim_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int problemId = Convert.ToInt32(btn.CommandArgument);
            string solverEmail = Session["UserEmail"].ToString(); // Assuming user is logged in

            string query = "UPDATE Problems SET SolverEmail = @SolverEmail WHERE ProblemID = @ProblemID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BountyDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SolverEmail", solverEmail);
                    cmd.Parameters.AddWithValue("@ProblemID", problemId);

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
                string query = "SELECT Title, Description, Bounty, SolverEmail, Solved FROM Problems WHERE PosterEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    conn.Close();
                }
            }
        }


    }
}