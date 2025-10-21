using BookShop.Models;

namespace BookShop.Services
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Customer> SearchCustomers(string searchTerm);
        Customer? GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }
}
