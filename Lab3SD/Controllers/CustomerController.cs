using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3SD.Context;
using Lab3SD.Models;
using Lab3SD.Repository;
using Lab3SD.ViewModels;
using Microsoft.Data.SqlClient;

namespace Lab3SD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;


        public CustomerController(IRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return Ok(await _customerRepository.GetItems());
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetItem(id);
            return customer == null ? NotFound() : Ok(_mapper.Map<CustomerViewModel>(customer));
        }
        
        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            
            try
            {
                await _customerRepository.Update(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_customerRepository.ItemExists(customer.CustomerId))
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
        
        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                await _customerRepository.Create(customer);
            }
            catch (DbUpdateException)
            {
                if (_customerRepository.ItemExists(customer.CustomerId))
                {
                    return Conflict($"Item with id {customer.CustomerId} already exists");
                }
                throw;
            }
        
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }
        
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.Delete(id);
            return customer == null ? NotFound() : Content($"Record №{id} deleted successfully");
        }

        [HttpGet("GetAllWithAdoNet")]
        public ActionResult<List<Customer>> Select()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection myConnection = new SqlConnection("Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true"))
            {
                using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Customers", myConnection))
                {
                    myConnection.Open();

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                Phone = reader.GetString(reader.GetOrdinal("phone")),
                                Address = reader.GetString(reader.GetOrdinal("address")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }

            return Ok(customers);
        }
        
        [HttpGet("FilterWithAdoNet/{firstName}")]
        public ActionResult<List<Customer>> SelectByFirstName(string firstName)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection myConnection = new SqlConnection("Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true"))
            {
                using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Customers WHERE first_name LIKE @FirstName + '%'", myConnection))
                {
                    sqlCmd.Parameters.AddWithValue("@FirstName", firstName);
                    myConnection.Open();

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                Phone = reader.GetString(reader.GetOrdinal("phone")),
                                Address = reader.GetString(reader.GetOrdinal("address")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }

            return Ok(customers);
        }

        [HttpGet("GetCustomerOrderHistory/{customerId}")]
        public ActionResult<List<CustomerOrderHistory>> GetCustomerOrderHistory(int customerId)
        {
            List<CustomerOrderHistory> orderHistory = new List<CustomerOrderHistory>();
        
            try
            {
                using SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true");
                connection.Open();
        
                using SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetCustomerOrderHistory(@customerId)", connection);
                cmd.Parameters.AddWithValue("@customerId", customerId);
        
                using SqlDataReader reader = cmd.ExecuteReader();
        
                while (reader.Read())
                {
                    CustomerOrderHistory order = new CustomerOrderHistory
                    {
                        OrderId = reader.GetInt32("order_id"),
                        ProductsOrdered = reader.GetString("products_ordered"),
                        TotalSum = reader.GetDecimal("total_sum"),
                        OrderDate = reader.GetDateTime("order_date"),
                        Status = reader.GetString("status"),
                    };
        
                    orderHistory.Add(order);
                }
        
                return Ok(orderHistory);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("FilterAndCallProcedure/{firstName}/{customerId}")]
        public ActionResult<CombinedCustomerData> GetCustomerData(string firstName, int customerId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true"))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Некоректний формат імені користувача, відкат транзакції
                        if (int.TryParse(firstName, out _))
                        {
                            transaction.Rollback();
                            return BadRequest("Invalid value for First Name. It cannot be a number.");
                        }
                        
                        // Вибірка клієнтів за їхнім ім'ям
                        List<Customer> filteredCustomers = new List<Customer>();
                        using (SqlCommand filterCmd = new SqlCommand("SELECT * FROM Customers WHERE first_name LIKE @FirstName + '%'", connection, transaction))
                        {
                            filterCmd.Parameters.AddWithValue("@FirstName", firstName);

                            using (SqlDataReader customerReader = filterCmd.ExecuteReader())
                            {
                                while (customerReader.Read())
                                {
                                    Customer customer = new Customer
                                    {
                                        CustomerId = customerReader.GetInt32(customerReader.GetOrdinal("customer_id")),
                                        FirstName = customerReader.GetString(customerReader.GetOrdinal("first_name")),
                                        LastName = customerReader.GetString(customerReader.GetOrdinal("last_name")),
                                        Phone = customerReader.GetString(customerReader.GetOrdinal("phone")),
                                        Address = customerReader.GetString(customerReader.GetOrdinal("address")),
                                        UserId = customerReader.GetInt32(customerReader.GetOrdinal("user_id")),
                                    };

                                    filteredCustomers.Add(customer);
                                }
                            }
                        }

                        // Виклик процедури, що показує історію замовлень клієнта за його ід
                        List<CustomerOrderHistory> orderHistory = new List<CustomerOrderHistory>();
                        using (SqlCommand orderCmd = new SqlCommand("SELECT * FROM dbo.GetCustomerOrderHistory(@customerId)", connection, transaction))
                        {
                            orderCmd.Parameters.AddWithValue("@customerId", customerId);

                            using (SqlDataReader orderReader = orderCmd.ExecuteReader())
                            {
                                while (orderReader.Read())
                                {
                                    CustomerOrderHistory order = new CustomerOrderHistory
                                    {
                                        OrderId = orderReader.GetInt32(orderReader.GetOrdinal("order_id")),
                                        ProductsOrdered = orderReader.GetString(orderReader.GetOrdinal("products_ordered")),
                                        TotalSum = orderReader.GetDecimal(orderReader.GetOrdinal("total_sum")),
                                        OrderDate = orderReader.GetDateTime(orderReader.GetOrdinal("order_date")),
                                        Status = orderReader.GetString(orderReader.GetOrdinal("status")),
                                    };

                                    orderHistory.Add(order);
                                }
                            }
                        }

                        transaction.Commit();

                        return Ok(new CombinedCustomerData
                        {
                            FilteredCustomers = filteredCustomers,
                            OrderHistory = orderHistory
                        });
                    }
                    catch (SqlException filterException)
                    {
                        transaction.Rollback();
                        return StatusCode(500, $"Filter query execution error: {filterException.Message}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return StatusCode(500, $"Internal server error: {ex.Message}");
                    }
                }
            }
        }
        
        [HttpPost("AddWithAdoNet")] 
        public void Insert(Customer customer)  
        {  
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString =
                "Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true";
            SqlCommand sqlCmd = new SqlCommand();  
            sqlCmd.CommandType = CommandType.Text;  
            sqlCmd.CommandText = "INSERT INTO Customers (customer_id, first_name, last_name, phone, address, user_id) Values (@CustomerID, @FirstName, @LastName, @Phone, @Address, @UserID)";  
            sqlCmd.Connection = myConnection;  
            
            sqlCmd.Parameters.AddWithValue("@CustomerID", customer.CustomerId);  
            sqlCmd.Parameters.AddWithValue("@FirstName", customer.FirstName);  
            sqlCmd.Parameters.AddWithValue("@LastName", customer.LastName);
            sqlCmd.Parameters.AddWithValue("@Phone", customer.Phone);  
            sqlCmd.Parameters.AddWithValue("@Address", customer.Address);  
            sqlCmd.Parameters.AddWithValue("@UserID", customer.UserId);  
            myConnection.Open();  
            sqlCmd.ExecuteNonQuery();  
            myConnection.Close();  
        } 
        
        [HttpPost("AddAndSelectWithAdoNet")]
        public ActionResult<List<Customer>> AddAndSelect(Customer customer)
        {
            using (SqlConnection myConnection = new SqlConnection("Data Source=localhost;Initial Catalog=master;User=SA;Password=reallyStrongPwd123;TrustServerCertificate=true"))
            {
                myConnection.Open();
                SqlTransaction transaction = myConnection.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    // Додаю користувача
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Customers (customer_id, first_name, last_name, phone, address, user_id) VALUES (@CustomerID, @FirstName, @LastName, @Phone, @Address, @UserID)", myConnection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@CustomerID", customer.CustomerId);
                        insertCmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        insertCmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        insertCmd.Parameters.AddWithValue("@Phone", customer.Phone);
                        insertCmd.Parameters.AddWithValue("@Address", customer.Address);
                        insertCmd.Parameters.AddWithValue("@UserID", customer.UserId);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Вибираю всіх користувачів
                    List<Customer> customers = new List<Customer>();
                    using (SqlCommand selectCmd = new SqlCommand("SELECT * FROM Customers", myConnection, transaction))
                    {
                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer selectedCustomer = new Customer
                                {
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                                    Address = reader.GetString(reader.GetOrdinal("address")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                };

                                customers.Add(selectedCustomer);
                            }
                        }
                    }

                    transaction.Commit();

                    return Ok(customers);
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    
                    // ІД клієнта повторюється
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        return Conflict("Duplicate customer ID. Please choose a different ID.");
                    }
                    
                    // Номер телефону занадто довгий для колонки бази даних
                    else if (customer.Phone.Length >= 20)
                    {
                        return Conflict("Phone number is too long. Please choose a different phone number format.");
                    }
                    else 
                    {
                        return StatusCode(500, $"Internal server error: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}
