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
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IRepository<Order> orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await _orderRepository.GetItems());
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderRepository.GetItem(id);
            return order == null ? NotFound() : Ok(_mapper.Map<OrderViewModel>(order));
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            
            try
            {
                await _orderRepository.Update(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_orderRepository.ItemExists(order.OrderId))
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

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            try
            {
                await _orderRepository.Create(order);
            }
            catch (DbUpdateException)
            {
                if (_orderRepository.ItemExists(order.OrderId))
                {
                    return Conflict($"Item with id {order.OrderId} already exists");
                }
                throw;
            }
        
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.Delete(id);
            return order == null ? NotFound() : Content($"Record №{id} deleted successfully");
        }
    }
}
