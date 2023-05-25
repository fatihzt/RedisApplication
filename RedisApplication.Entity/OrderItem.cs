using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Entity
{
    public class OrderItem
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        [ForeignKey("ProducttId")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // İlişki: Siparişe referans (Foreign Key)
        
        public Order Order { get; set; }

        // İlişki: Ürüne referans (Foreign Key)
        public Product Product { get; set; }
    }
}
