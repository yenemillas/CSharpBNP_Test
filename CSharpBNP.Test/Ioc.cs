using CSharpBNP.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBNP.Test
{
    public static class Ioc
    {

        private static Dictionary<Type, Func<object>> dico = new Dictionary<Type, Func<object>>
        {
           { typeof(ITransaction), () => new Transaction()}
        };

        public static TInterface Get<TInterface>() where TInterface : class
        {
            if (!dico.ContainsKey(typeof(TInterface)))
            {
                throw new ArgumentException("type inconnu");
            }

            return dico[typeof(TInterface)]() as TInterface;
        }
    }
}
