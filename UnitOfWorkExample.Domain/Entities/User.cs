using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWorkExample.Domain.Entities
{
    public class User : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual string Roles { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
