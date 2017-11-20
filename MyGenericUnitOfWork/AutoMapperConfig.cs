using AutoMapper;
using BussinessCore.DTO;
using BussinessCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork
{
    public static class EntityMapper
    {
        private static bool configed = false;
        private static object syncObjectRegister = new object();

        private static void Register()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Client, ClientDto>();
                cfg.CreateMap<Product, ProductDto>();
            });
        }

        public static void CheckAndRegister()
        {
            lock (syncObjectRegister)
            {
                if (!configed)
                {
                    Register();
                    configed = true;
                }
            }
        }

        public static V MapTo<T, V>(this T t) where V : class where T: class
        {
            return Mapper.Map<V>(t);
        }
    }
}
