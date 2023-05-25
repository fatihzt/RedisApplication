using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Core.Abstract
{
    public interface IProductCore:IEntityRepository<Product>
    {
    }
}
