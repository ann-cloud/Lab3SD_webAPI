using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3SD.Context;
using Lab3SD.Models;
using Lab3SD.Repository;
using Microsoft.Data.SqlClient;

namespace Lab3SD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserRoleController(IRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
        }

        // GET: api/UserRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoles()
        {
            return Ok(await _userRoleRepository.GetItems());
        }

        // GET: api/UserRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var userRole = await _userRoleRepository.GetItem(id);
            return userRole == null ? NotFound() : Ok(userRole);
        }

        // PUT: api/UserRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRole(int id, UserRole userRole)
        {
            if (id != userRole.RoleId)
            {
                return Conflict($"Item id and entered id differ");
            }

            try
            {
                await _userRoleRepository.Update(userRole);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!_userRoleRepository.ItemExists(userRole.RoleId))
                {
                    return NotFound();
                }

                return Conflict(e.Message);
            }
            catch (Exception exception)
            {
                Conflict(exception.Message);
            }
        
            return Content("Record updated successfully");
        }

        // POST: api/UserRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRole>> PostUserRole(UserRole userRole)
        {
            try
            {
                await _userRoleRepository.Create(userRole);
            }
            catch (DbUpdateException e)
            {
                if (_userRoleRepository.ItemExists(userRole.RoleId))
                {
                    return Conflict($"Item with id {userRole.RoleId} already exists");
                }
                return Conflict(e.Message);
            }
            catch (Exception exception)
            {
                Conflict(exception.Message);
            }
            return CreatedAtAction("GetUserRole", new { id = userRole.RoleId }, userRole);
        }

        // DELETE: api/UserRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var userRole = await _userRoleRepository.Delete(id);
            return userRole == null ? NotFound() : Content($"Record №{id} deleted successfully");
        }
        
        [HttpPost("AddWithAdoNet")] 
        public void InsertRole(UserRole userRole)  
        {  
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString =
                "Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true";
            SqlCommand sqlCmd = new SqlCommand();  
            sqlCmd.CommandType = CommandType.Text;  
            sqlCmd.CommandText = "INSERT INTO User_Roles (role_id, role) Values (@UserRoleID, @RoleName)";  
            sqlCmd.Connection = myConnection;  
            
            sqlCmd.Parameters.AddWithValue("@UserRoleID", userRole.RoleId);  
            sqlCmd.Parameters.AddWithValue("@RoleName", userRole.Role);  
            myConnection.Open();  
            sqlCmd.ExecuteNonQuery();  
            myConnection.Close();  
        }  
        
        [HttpDelete("DeleteWithAdoNet/{id}")]
        public void DeleteRole(int id)  
        {  
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString =
                "Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true";            SqlCommand sqlCmd = new SqlCommand();  
            sqlCmd.CommandType = CommandType.Text;  
            sqlCmd.CommandText = $"DELETE FROM User_Roles WHERE role_id = {id}";  
            sqlCmd.Connection = myConnection; 
            
            myConnection.Open();  
            sqlCmd.ExecuteNonQuery();  
            myConnection.Close();  
        }  
        
        [HttpPut("UpdateWithAdoNet/{id}")]
        public void UpdateRole(int id, UserRole userRole)  
        {
            if (id == userRole.RoleId)
            {
                SqlConnection myConnection = new SqlConnection();
                myConnection.ConnectionString =
                    "Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true";
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "UPDATE User_Roles SET role = @RoleName WHERE role_id = @UserRoleID";  
                sqlCmd.Connection = myConnection;

                sqlCmd.Parameters.AddWithValue("@UserRoleID", userRole.RoleId);  
                sqlCmd.Parameters.AddWithValue("@RoleName", userRole.Role);  
                myConnection.Open();
                sqlCmd.ExecuteNonQuery();
                myConnection.Close();
            }
        }  
    }
}
