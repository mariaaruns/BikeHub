using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikehub.Hybrid.DeviceServices.Toast
{
    public interface IToastService
    {
        Task Show(string message);
    }
}
