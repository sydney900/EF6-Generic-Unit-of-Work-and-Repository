using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessCore.DTO
{
    public class ClientDto
    {
        public long Id { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public DateTime LastModified { get; set; }
    }
}
