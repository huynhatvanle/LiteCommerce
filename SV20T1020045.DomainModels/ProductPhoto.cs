using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020045.DomainModels
{
    public class ProductPhoto
    {
        public long PhotoID { get; set; }
        public int ProductID { get; set; } 
        public string Photo { get; set; } = "";
        public string Description { get; set; } = "";
        public int DisplayOrder {  get; set; }  
        public bool IsHidden { get; set; }  
    }
}
