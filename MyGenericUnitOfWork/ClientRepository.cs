using AutoMapper;
using BussinessCore.DTO;
using BussinessCore.Model;
using MyGenericUnitOfWork.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(MyAppContext context)
            : base(context)
        {
            //context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            EntityMapper.CheckAndRegister();
        }

        public List<ClientDto> GetAllClientsSortByName()
        {
            return _entities.OrderBy(c => c.ClientName).ProjectToList<ClientDto>();
        }

        public async Task<List<ClientDto>> GetAllClientsSortByNameAsync()
        {
            return await _entities.OrderBy(c => c.ClientName).ProjectToListAsync<ClientDto>();
        }

    }
}
