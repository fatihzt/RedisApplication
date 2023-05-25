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
    public class OrderItemManager : IOrderItemService
    {
        public int Add(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public OrderItem Get(Expression<Func<OrderItem, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<OrderItem> GetAll(Expression<Func<OrderItem, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public bool Update(OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
