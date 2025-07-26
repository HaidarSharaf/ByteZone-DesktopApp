using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteZonee
{
    public class Order
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }
}
