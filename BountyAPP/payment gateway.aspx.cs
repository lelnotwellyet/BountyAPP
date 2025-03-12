using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BountyAPP
{
    public partial class payment_gateway : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BountyDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ProblemID"] != null || Session["UserEmail"]==null)
            {
                LoadBounty(Convert.ToInt32(Session["ProblemID"]), Convert.ToString(Session["UserEmail"]));
            }
            else
            {
                Label1.Text = "Error: No Problem ID found.";
            }
        }
        private void LoadBounty(int problemID, string payee)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Bounty FROM Problems WHERE ProblemID = @ProblemID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProblemID", problemID);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Label1.Text = "Amount Being paid : " + reader["Bounty"].ToString() + "to" + payee;
                        }
                        else
                        {
                            Label1.Text = "Bounty not found.";
                        }
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}

