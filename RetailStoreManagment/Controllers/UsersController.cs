using BussinessLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RetailStoreMangementApi.Controllers
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string Password { get; set; }
    }
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("ListUsers", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<UserDTO>> GetAll()
        {
            return Ok(Users.GetAllUsers());
        }

        [HttpGet("FindUserBy/{id}", Name = "FindUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<UserDTO> GetById(int id)
        {
            var user = Users.GetUserByID(id);
            if (user == null)
                return NotFound();

            return Ok(user.userDTO);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("login-history")]
        public ActionResult<List<LoginHistoryDTO>> GetLoginHistory()
        {
            return Ok(Users.GetHistoryUserlogin());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddUser", Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<UserDTO> Add([FromBody] UserDTO userDTO)
        {
            var user = new Users
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password,
                Permitions = (Users.enMainMenuPermitions)userDTO.Permition,
                isActive = userDTO.IsActive
            };

            var newId = user.Addnewuser();
            if (newId == -1)
                return BadRequest("Failed to add user.");

            return CreatedAtAction(nameof(GetById), new { id = newId }, user.userDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser/{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Update(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.ID)
                return BadRequest("ID mismatch.");

            var user = new Users(userDTO);
            if (!user.UpdateUser())
                return BadRequest("Failed to update user.");

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult Delete(int id)
        {
            var user = Users.GetUserByID(id);
            if (user == null)
                return NotFound();

            if (!user.DeleteUser())
                return BadRequest("Failed to delete user.");

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ChangePassword/{id}", Name = "ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            var user = Users.GetUserByID(id);
            if (user == null)
                return NotFound();

            if (!user.Changepassword(request.Password))
                return BadRequest("Failed to change password.");

            return NoContent();
        }
    }
}
