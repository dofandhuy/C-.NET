using Assignment.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assignment.Repositories.Repository
{
    public class CustomerRepository: ICustomerRepository
    {
        private static string _connectionString = string.Empty;
        private readonly FuminiHotelManagementContext _context;
        private static CustomerRepository _instance = null;

        private CustomerRepository()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Repository chưa được khởi tạo với Connection String.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<FuminiHotelManagementContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            _context = new FuminiHotelManagementContext(optionsBuilder.Options);
        }
        public static void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = connectionString;
            }
        }
        public static CustomerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    
                    _instance = new CustomerRepository();
                }
                return _instance;
            }
        }


        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.EmailAddress == email);
        }

        public Customer GetCustomerByEmailAndPassword(string email, string password)
        {

            var customer = _context.Customers.FirstOrDefault(c => c.EmailAddress == email);

            if (customer != null && customer.Password == password) { return customer; }
            return null;
        }

        public void AddCustomer(Customer customer)
        {
            if (GetCustomerByEmail(customer.EmailAddress) != null)
            {
                throw new InvalidOperationException("Email đã tồn tại.");
            }
          

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Find(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void ChangeCustomerStatus(int customerId, byte status)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                customer.CustomerStatus = status;
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
        }
    }
}
