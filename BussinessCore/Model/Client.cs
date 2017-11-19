using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Client : BaseEntity
    {
        public string ClientName { get; set; }
        public string ClientPassWord { get; set; }
        public string Email { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
