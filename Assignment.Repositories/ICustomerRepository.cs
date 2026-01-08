using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model.Models;

namespace Assignment.Repositories
{
    public  interface ICustomerRepository
    {
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerByEmailAndPassword(string email, string password);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        Customer GetCustomerById(int id);
        List<Customer> GetAllCustomers();
        // Phương thức khác: DeleteCustomer (thường là đổi CustomerStatus = 0)
        void ChangeCustomerStatus(int customerId, byte status);
    }
}
