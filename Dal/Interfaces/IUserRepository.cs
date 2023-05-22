using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.DataObject;
namespace Dal.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> ReadByPasswordAsync(string password);
    }
}
