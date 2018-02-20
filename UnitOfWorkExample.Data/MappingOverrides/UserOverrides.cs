using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWorkExample.Domain.Entities;

namespace UnitOfWorkExample.Data.MappingOverrides
{
    public class UserOverrides : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping) {
            mapping.References(x => x.Customer,"CustomerId");
        }
    }
}
