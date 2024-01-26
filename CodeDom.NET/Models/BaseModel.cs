using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDom.NET.Models
{
    public class BaseModel
    {
        public string Namespace { get; set; }
        public string ModelName { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public string? BaseType { get; set; }
    }
}
