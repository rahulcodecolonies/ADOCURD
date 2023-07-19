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
            SqlCommand cmd = new SqlCommand("GetUsers", cons);
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
                pModel.DOB = Convert.ToDateTime(dt.Rows[i]["DOB"]); 
                users.Add(pModel);
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
                SqlCommand cmd = new SqlCommand("InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                cmd.Parameters.AddWithValue("@Class", obj.Class);
                cmd.Parameters.AddWithValue("@Subject", obj.Subject);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);

              
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
                SqlCommand cmd = new SqlCommand("UpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                cmd.Parameters.AddWithValue("@Class", obj.Class);
                cmd.Parameters.AddWithValue("@Subject", obj.Subject);
                cmd.Parameters.AddWithValue("@DOB", obj.DOB);
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
                    SqlConnection con = new SqlConnection(_configuration.GetConnectionString("con"));                
                    SqlCommand cmd = new SqlCommand("DeleteUser", con);
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
