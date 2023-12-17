using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3SD.Context;
using Lab3SD.Models;
using Lab3SD.Repository;
using Lab3SD.ViewModels;

namespace Lab3SD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IMapper _mapper;

        public UserAccountController(IRepository<UserAccount> userAccountRepository, IMapper mapper)
        {
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
            _mapper = mapper;
        }

        // GET: api/UserAccount
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserAccounts()
        {
            return Ok(await _userAccountRepository.GetItems());
        }

        // GET: api/UserAccount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetUserAccount(int id)
        {
            var userAccount = await _userAccountRepository.GetItem(id);
            return userAccount == null ? NotFound() : Ok(_mapper.Map<UserAccountViewModel>(userAccount));
        }

        // PUT: api/UserAccount/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAccount(int id, UserAccount userAccount)
        {
            if (id != userAccount.UserId)
            {
                return BadRequest();
            }
            
            try
            {
                await _userAccountRepository.Update(userAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userAccountRepository.ItemExists(userAccount.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        
            return Content("Record updated successfully");
        }

        // POST: api/UserAccount
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAccount>> PostUserAccount(UserAccount userAccount)
        {
            try
            {
                await _userAccountRepository.Create(userAccount);
            }
            catch (DbUpdateException)
            {
                if (_userAccountRepository.ItemExists(userAccount.UserId))
                {
                    return Conflict($"Item with id {userAccount.UserId} already exists");
                }
                throw;
            }
        
            return CreatedAtAction("GetUserAccount", new { id = userAccount.UserId }, userAccount);
        }

        // DELETE: api/UserAccount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAccount(int id)
        {
            var userAccount = await _userAccountRepository.Delete(id);
            return userAccount == null ? NotFound() : Content($"Record №{id} deleted successfully");
        }
    }
}
