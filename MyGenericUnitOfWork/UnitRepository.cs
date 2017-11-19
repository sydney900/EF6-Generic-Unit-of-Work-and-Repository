using Core.Model;
using MyGenericUnitOfWork.Base;

namespace MyGenericUnitOfWork
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(MyAppContext context)
            : base(context)
        {

        }
    }
}
