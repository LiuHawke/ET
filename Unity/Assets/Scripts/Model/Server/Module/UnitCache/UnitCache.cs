using System.Collections.Generic;

namespace ET.Server
{
    public interface IUnitCache
    {
        
    }

    [ChildOf(typeof(UnitCacheComponent))]
    public class UnitCache: Entity, IAwake, IDestroy
    {
        public string key;
        public Dictionary<long, Entity> CacheComponentDic = new Dictionary<long, Entity>();
    }
}