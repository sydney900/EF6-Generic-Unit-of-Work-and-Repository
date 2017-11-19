using Core.Model;
using MyGenericUnitOfWork.Base;

namespace MyGenericUnitOfWork
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(MyAppContext context)
            : base(context)
        {

        }
    }
}
