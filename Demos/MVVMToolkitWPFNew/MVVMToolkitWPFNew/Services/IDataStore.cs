using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolkitWPFNew.Services
{
    public interface IDataStore
    {
        public Task StoreUser(CancellationToken token);
    }
}
