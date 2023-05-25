using RedisApplication.Business.Service;
using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Business.Manager
{
    public class OrderManager : IOrderService
    {
        public int Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order Get(Expression<Func<Order, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAll(Expression<Func<Order, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
