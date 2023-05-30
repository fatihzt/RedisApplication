using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Entity
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserId")]

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public User User { get; set; }

        // İlişki: Sipariş ürünlerine referans (One-to-Many ilişkisi)
        public ICollection<OrderItem> OrderItems { get; set; }
    }
    
}
