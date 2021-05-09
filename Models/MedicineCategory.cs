using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class MedicineCategory
    {
        [Key]
        public string Category_Id { get; set; }
        public string Category_Name { get; set; }
    }
}
