using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OphtalmoTesting
{
    public interface ICrud
    {
        void Add();
        void AddOrUpdate();
        void Delete();
    }
}
