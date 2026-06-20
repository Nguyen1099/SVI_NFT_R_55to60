using SVI_NFT_R;
using System.Collections.Generic;
using static SVI_NFT_R.CCIMDefine;

public static partial class UnitManager
{
    private class UnitGroupOption
    {
        public EUnit Name { get; private set; }
        public List<IUnitNode> UnitNodes { get; private set; }

        public UnitGroupOption(EUnit name, params IUnitNode[] unitNodes)
        {
            Name = name;
            UnitNodes = new List<IUnitNode>(unitNodes);
        }
    }
}
