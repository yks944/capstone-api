using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class MedicineModel
    {
        
        public string Category_Id { get; set; }
        public string Category_Name { get; set; }
        [Key]
        public string Medicine_Id { get; set; }
        public string Medicine_Name { get; set; }
        public string Medicine_Img_Url { get; set; }
        public double Medicine_Price { get; set; }
        public string Medicine_Seller { get; set; }
        public string Medicine_Description { get; set; }
        public int Medicine_Qty { get; set; }
    }
}
