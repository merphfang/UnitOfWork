using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitOfWorkExample.Data.MappingOverrides;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Helpers;

namespace UnitOfWorkExample.Data.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        static UnitOfWork() 
        {
            var connectionStr = MsSqlConfiguration.MsSql2012.ConnectionString(x => x.FromConnectionStringWithKey("UnitOfWorkExample"));
            _sessionFactory = Fluently.Configure()
                .Database(connectionStr)
                .Mappings(x => x.AutoMappings
                .Add(AutoMap.AssemblyOf<Account>(new AutomappingConfiguration()).UseOverridesFromAssemblyOf<AccountOverrides>())
                .Add(AutoMap.AssemblyOf<Customer>(new AutomappingConfiguration()).UseOverridesFromAssemblyOf<CustomerOverrides>())

                 )
                .BuildSessionFactory();
        }

        public UnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch 
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                //throw;
            }
            finally
            {
                Session.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}
