using RedisApplication.Core.Abstract;
using RedisApplication.Core.Entityframework;
using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Core.Concrete
{
    public class EfProductCore:EfEntityRepositoryBase<Product,DataBaseContext>,IProductCore
    {
    }
}
