using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Helpers;
using UnitOfWorkExample.Domain.Repositories;

namespace UnitOfWorkExample.Domain.Services
{
    public interface ICustomerService
    {
        IList<Customer> GetAll();
        Customer GetById(int id);
        void Create(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Customer GetCustomerDetails(Customer customer);

        IQueryable<Customer> GetCustomers(string filter, int initialPage, int pageSize, string sortCol, string sortDir, out int totalRecords);
    }

    public class CustomerService : ICustomerService
    {
        private IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository) {
            _customerRepository = customerRepository;
        }

        public IList<Customer> GetAll() {
            return _customerRepository
                .GetAll()
                .ToList();
        }

        public Customer GetById(int id) {
            return _customerRepository.GetById(id);
        }

        public void Create(Customer customer) {
            _customerRepository.Create(customer);
        }

        public void Update(Customer customer) {
            _customerRepository.Update(customer);
        }

        public void Delete(int id) {
            _customerRepository.Delete(id);
        }

        public Customer GetCustomerDetails(Customer customer) {
            var customers = _customerRepository.GetAll().SingleOrDefault(x => x.Email.ToLower() == customer.Email.ToLower());

            return customers;
        }

        public IQueryable<Customer> GetCustomers(string filter, int start, int pageSize, string sortCol, string sortDir, out int totalRecords) {
            int initialPage = start / pageSize;
            var query = _customerRepository.GetAll().Where(x => x.Name.ToLower().Contains(filter.ToLower()) || x.Email.ToLower().Contains(filter.ToLower()));
            if (query.Any()) {
                IQueryable<Customer> sortedQuery = GetSortedQuery(query, sortCol, sortDir);
                List<Customer> customers;
                customers = sortedQuery
                   .Skip(start)
                   .Take(pageSize)
                   .ToList();
                totalRecords = query.Count();

                return customers.Select(u => u).AsQueryable();
            } else {
                totalRecords = 0;
                return null;
            }
        }

        private IQueryable<Customer> GetSortedQuery(IQueryable<Customer> query, string sortCol, string sortDir) {
            // sort expression
            Expression<Func<Customer, object>> sortExpression;
            switch (sortCol) {

                //case "CreatedDate":
                //    sortExpression = (x => x.CreatedDate);
                //    break;
                case "Name":
                    sortExpression = (x => x.Name);
                    break;
                default:
                    sortExpression = (x => x.Email);
                    break;
            }
            if (sortDir == "desc") {
                return query.OrderByDescending(sortExpression).AsQueryable();
            } else {
                return query.OrderBy(sortExpression).AsQueryable();
            }
        }

      
    }
}
