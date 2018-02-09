using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Helpers;
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

        IQueryable<User> GetUsers(string filter, int initialPage, int pageSize, out int totalRecords);
    }

    public class AccountService : IAccountService
    {
        private IRepository<User> _accountRepository;

        public AccountService(IRepository<User> productRepository) {
            _accountRepository = productRepository;
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

        public IQueryable<User> GetUsers(string filter, int start, int pageSize, out int totalRecords) {
            int initialPage = start / pageSize;
            var query = GetAll().Where(x => x.FirstName.ToLower().Contains(filter.ToLower()) || x.LastName.ToLower().Contains(filter.ToLower()) || x.Email.ToLower().Contains(filter.ToLower()));
            if (query.Any()) {
                var users = query
                    .OrderBy(x => x.FirstName)
                    .Skip(initialPage * pageSize)
                    .Take(pageSize)
                    .GroupBy(p => new { Total = query.Count() })
                    .First();
                totalRecords = users.Key.Total;
                return users.Select(u => u).AsQueryable();
            } else {
                totalRecords = 0;
                return null;
            }
        }
      
    }
}
