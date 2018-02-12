using System.Linq;
using UnitOfWorkExample.Data.Helpers;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Repositories;
using NHibernate;
using UnitOfWorkExample.Domain.Helpers;

namespace UnitOfWorkExample.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private UnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork){
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        protected ISession Session { get { return _unitOfWork.Session; } }

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public T GetById(int id)
        {
            return Session.Get<T>(id);
        }

        public void Create(T entity)
        {
            Session.Save(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public void Delete(int id)
        {
            Session.Delete(Session.Load<T>(id));
        }

        public IQueryable<T> GetAll(string query) {
            return Session.CreateQuery(query).List<T>().AsQueryable();
        }
    }
}
