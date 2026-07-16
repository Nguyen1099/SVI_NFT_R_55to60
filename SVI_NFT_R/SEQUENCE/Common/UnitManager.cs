using SVI_NFT_R;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static SVI_NFT_R.CCIMDefine;

public static partial class UnitManager
{
    public static bool IsInitialize { get; private set; } = false;
    public static IReadOnlyDictionary<EUnit, UnitGroup> Units => mUnits;
    private static readonly Dictionary<EUnit, UnitGroup> mUnits = new Dictionary<EUnit, UnitGroup>(Enum.GetNames(typeof(EUnit)).Length);

    public static bool Initialize(CDocument document)
    {
        if (IsInitialize == true)
        {
            return false;
        }

        // TODO: 유닛 그룹 맵핑 정의
        var motion = document.m_objProcessMain.m_objProcessMotion;
        var unitMapping = new UnitGroupOption[]
        {
            new UnitGroupOption(EUnit.ST01, motion.InShuttle, motion.InRobot, motion.InspStage, motion.OutRobot, motion.OutFlip, motion.OutShuttle),
        };
        bool bFailed = false;
        foreach (var item in unitMapping)
        {
            mUnits[item.Name] = new UnitGroup();
            if (mUnits[item.Name].Initialize(document, item.Name, item.UnitNodes) == false)
            {
                bFailed = true;
            }
        }
        if (bFailed == true)
        {
            return false;
        }

        Debug.Assert(mUnits.Keys.Count == Enum.GetNames(typeof(EUnit)).Length, "맵핑되지 않은 유닛 그룹이 있습니다. enum에 정의된 유닛 그룹은 반드시 맵핑해야 합니다.");

        IsInitialize = true;
        return true;
    }

    public static void DeInitialize()
    {
        if (IsInitialize == false)
        {
            return;
        }

        Parallel.ForEach(mUnits.Values, unitGroup => unitGroup.DeInitialize());
        mUnits.Clear();

        IsInitialize = false;
    }
}
