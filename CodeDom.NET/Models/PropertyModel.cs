using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDom.NET.Models
{
    public class PropertyModel
    {
        public string? Accessor { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public bool IsGet { get; set; }

        public bool IsSet { get; set; }
        public string? Comment { get; set; }
    }
}
