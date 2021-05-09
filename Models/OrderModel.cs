using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Oid { get; set; }
        [Key]
        public string Username { get; set; }
        public string Pid { get; set; }
       
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Shipping_Address { get; set; }

    }
}
