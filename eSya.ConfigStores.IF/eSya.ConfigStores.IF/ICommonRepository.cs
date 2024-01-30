using eSya.ConfigStores.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigStores.IF
{
    public interface ICommonRepository
    {
        Task<List<DO_BusinessLocation>> GetBusinessKey();

    }
}
