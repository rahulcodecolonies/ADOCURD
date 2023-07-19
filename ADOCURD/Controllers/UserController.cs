using ADOCURD.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ADOCURD.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("GetAllUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            List<UserModel> users = new List<UserModel>();
            DataTable dt = new DataTable();
            SqlConnection cons = new SqlConnection(_configuration.GetConnectionString("con"));
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users", cons);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserModel pModel = new UserModel();
               
                pModel.Name = dt.Rows[i]["Name"].ToString();
                pModel.Email = dt.Rows[i]["Email"].ToString();
                pModel.Gender = dt.Rows[i]["Gender"].ToString();
                pModel.Class = dt.Rows[i]["Class"].ToString();
                pModel.Subject = dt.Rows[i]["Subject"].ToString();
                pModel.DOB = Convert.ToDateTime(dt.Rows[i]["DOB"]); // Convert to DateTime
                users.Add(pModel); // Use Add method instead of Add
            }
            return Ok(users);
        }
        [Route("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("con"));
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (Name, Email, Gender, Class, Subject, DOB) VALUES ('" + obj.Name + "', '" + obj.Email + "', '" + obj.Gender + "', '" + obj.Class + "', '" + obj.Subject + "', '" + obj.DOB.ToString("yyyy-MM-dd") + "')", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateUser")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("con"));
                SqlCommand cmd = new SqlCommand("Update Users set Name='" + obj.Name + "',Email='" + obj.Email + "',Gender='"+obj.Gender+"',Class='"+obj.Class+"',Subject='"+obj.Subject+"',DOB='"+obj.DOB+"' where Email='" + obj.Email + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("deleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string Email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("con")))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Email = @Email", con);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    con.Open();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok($"User with Email {Email} deleted successfully.");
                    }
                    else
                    {
                        return NotFound($"User with Email {Email} not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
