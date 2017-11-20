using BussinessCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessCore.Model
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public byte[] TimeStamp { get; set; }
    }
}
