using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class UnitCacheComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<string, UnitCache> UnitCachesDic = new Dictionary<string, UnitCache>();

        public List<string> UnitCacheNameList = new List<string>();
    }
}