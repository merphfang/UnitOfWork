using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Repositories;

namespace UnitOfWorkExample.Domain.Services
{
    public interface IAccountService
    {
        IList<User> GetAll();
        User GetById(int id);
        void Create(User user);
        void Update(User user);
        void Delete(int id);
        User GetUserDetails(User user);

        IQueryable<User> GetUsers(string filter, int initialPage, int pageSize,string sortCol,string sortDir, out int totalRecords);
    }

    public class AccountService : IAccountService
    {
        private IRepository<User> _accountRepository;

        public AccountService(IRepository<User> accountRepository) {
            _accountRepository = accountRepository;
        }

        public IList<User> GetAll() {
            return _accountRepository
                .GetAll()
                .ToList();
        }

        public User GetById(int id) {
            return _accountRepository.GetById(id);
        }

        public void Create(User user) {
            _accountRepository.Create(user);
        }

        public void Update(User user) {
            _accountRepository.Update(user);
        }

        public void Delete(int id) {
            _accountRepository.Delete(id);
        }

        public User GetUserDetails(User user) {
            var users = _accountRepository.GetAll().SingleOrDefault(x => x.Email.ToLower() == user.Email.ToLower());

            return users;
        }

        public IQueryable<User> GetUsers(string filter, int start, int pageSize, string sortCol, string sortDir, out int totalRecords) {
            int initialPage = start / pageSize;
            var query = _accountRepository.GetAll().Where(x => x.FirstName.ToLower().Contains(filter.ToLower()) || x.LastName.ToLower().Contains(filter.ToLower()) || x.Email.ToLower().Contains(filter.ToLower()) || x.Customer.Name.ToLower().Contains(filter.ToLower()));
            if (query.Any()) {
                IQueryable<User> sortedQuery = GetSortedQuery(query, sortCol, sortDir);
                    List<User> users;
                 users = sortedQuery
                    .Skip(start)
                    .Take(pageSize)
                    .ToList();
                    totalRecords = query.Count();
                
                return users.Select(u => u).AsQueryable();
            } else {
                totalRecords = 0;
                return null;
            }
        }

        private IQueryable<User> GetSortedQuery(IQueryable<User> query,string sortCol, string sortDir) {
            // sort expression
            Expression<Func<User, object>> sortExpression;
            switch (sortCol) {
               
                case "CreatedDate":
                    sortExpression = (x => x.CreatedDate);
                    break;
                case "FirstName":
                    sortExpression = (x => x.FirstName);
                    break;
                case "LastName":
                    sortExpression = (x => x.LastName);
                    break;
                case "Customer.Name":
                    sortExpression = (x => x.Customer.Name);
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
