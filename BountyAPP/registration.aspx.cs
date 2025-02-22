using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace BountyAPP
{
    public partial class registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-71QL0R7\\SQLEXPRESS;Initial Catalog=BountyDB;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Name, Email, Password, Role) VALUES(@Name, @Email, @Password, @Role)";

                using (SqlCommand cmd = new SqlCommand(query, conn))  
                {
                    cmd.Parameters.AddWithValue("@Name", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@Email", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Password", TextBox3.Text); 
                    cmd.Parameters.AddWithValue("@Role", DropDownList1.SelectedValue);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                        Response.Write("Registration Successful! :)");
                    else
                        Response.Write("Registration Failed :(");
                }
            }
        }
    }
}
