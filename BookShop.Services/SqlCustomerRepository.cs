using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public SqlCustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await _context.Customers.AsNoTracking().Include(c => c.Orders).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            try
            {
                return await _context.Customers.AsNoTracking()
                    .Include(c => c.Orders)
                    .Where(c => c.Name.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await _context.Customers.AsNoTracking().Include(c => c.Orders).FirstOrDefaultAsync(c => c.CustomerId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
