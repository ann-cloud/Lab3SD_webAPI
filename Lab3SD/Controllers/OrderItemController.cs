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
    public class OrderItemController : ControllerBase
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemController(IRepository<OrderItem> orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository ?? throw new ArgumentNullException(nameof(orderItemRepository));
            _mapper = mapper;
        }

        // GET: api/OrderItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return Ok(await _orderItemRepository.GetItems());
        }

        // GET: api/OrderItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetItem(id);
            return orderItem == null ? NotFound() : Ok(_mapper.Map<OrderItemViewModel>(orderItem));
        }

        // PUT: api/OrderItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }
            
            try
            {
                await _orderItemRepository.Update(orderItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_orderItemRepository.ItemExists(orderItem.OrderItemId))
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

        // POST: api/OrderItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            try
            {
                await _orderItemRepository.Create(orderItem);
            }
            catch (DbUpdateException)
            {
                if (_orderItemRepository.ItemExists(orderItem.OrderItemId))
                {
                    return Conflict($"Item with id {orderItem.OrderItemId} already exists");
                }
                throw;
            }
        
            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItem);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.Delete(id);
            return orderItem == null ? NotFound() : Content($"Record №{id} deleted successfully");
        }
    }
}
