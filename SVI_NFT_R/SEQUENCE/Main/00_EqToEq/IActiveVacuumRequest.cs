using System;
using System.Threading.Tasks;

namespace EqToEq
{
    public interface IActiveVacuumRequest
    {
        Task<bool> TaskVacuumOnRequest(EPosition placement, TimeSpan timeout);
    }
}
