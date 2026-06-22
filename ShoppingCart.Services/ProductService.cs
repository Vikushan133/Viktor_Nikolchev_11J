using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class ProductService
    {
        private readonly ShoppingCartContext _context;

        public ProductService(ShoppingCartContext context)
        {
            _context = context;
        }

        public void Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            return _context.Products.Include(p => p.CategoryRef).OrderBy(p => p.Name).ToList();
        }

        public List<Product> GetByCategory(int categoryId)
        {
            return _context.Products.Include(p => p.CategoryRef).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Include(p => p.CategoryRef).FirstOrDefault(p => p.ProductId == id);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
