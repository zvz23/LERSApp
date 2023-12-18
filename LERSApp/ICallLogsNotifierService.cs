using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LERSApp
{
    public interface ICallLogsNotifierService
    {
        void Start();
        void Stop();
        bool IsRunning();
    }
}
