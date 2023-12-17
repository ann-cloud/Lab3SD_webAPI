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
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetItems());
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetItem(id);
            return product == null ? NotFound() : Ok(_mapper.Map<ProductViewModel>(product));
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            
            try
            {
                await _productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productRepository.ItemExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        
            // return Content("Record updated successfully");
            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                await _productRepository.Create(product);
            }
            catch (DbUpdateException)
            {
                if (_productRepository.ItemExists(product.ProductId))
                {
                    return Conflict($"Item with id {product.ProductId} already exists");
                }
                throw;
            }
        
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.Delete(id);
            // return product == null ? NotFound() : Content($"Record №{id} deleted successfully");
            
            return product == null ? NotFound() : NoContent();
        }
    }
}
