using System.Collections.Generic;

namespace EqToEq
{
    public static class EqToEqUiDataManager
    {
        public static IDictionary<EEqToEqUiGroup, EqToEqUiGroup> Groups { get; } = new Dictionary<EEqToEqUiGroup, EqToEqUiGroup>();
    }
}
