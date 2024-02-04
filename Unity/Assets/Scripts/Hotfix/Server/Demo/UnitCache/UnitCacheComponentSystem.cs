using System;

namespace ET.Server
{
    [EntitySystemOf(typeof (UnitCacheComponent))]
    [FriendOf(typeof (UnitCacheComponent))]
    [FriendOf(typeof (UnitCache))]
    public static partial class UnitCacheComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitCacheComponent self)
        {
            self.UnitCacheNameList.Clear();
            // foreach (Type type in EventSystem.Instance.GetType())
            // {
            //     if (type != typeof (IUnitCache) && typeof (IUnitCache).IsAssignableFrom(type))
            //     {
            //         self.UnitCacheNameList.Add(type.Name);
            //     }
            // }
            
            foreach (string key in self.UnitCacheNameList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCachesDic.Add(key, unitCache);
            }
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.UnitCacheComponent self)
        {
            foreach (UnitCache unitCache in self.UnitCachesDic.Values)
            {
                unitCache?.Dispose();
            }

            self.UnitCachesDic.Clear();
        }

        public static async ETTask<Entity> Get(this UnitCacheComponent self, long unitId, string key)
        {
            if (!self.UnitCachesDic.TryGetValue(key, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCachesDic.Add(key, unitCache);
            }

            return await unitCache.Get(unitId);
        }
        
        
        public static async ETTask<T> Get<T>(this UnitCacheComponent self, long unitId) where T : Entity
        {
            string key = typeof (T).Name;
            
            if (!self.UnitCachesDic.TryGetValue(key,out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCachesDic.Add(key, unitCache);
            }
            return await unitCache.Get(unitId) as T;
        }

        public static async ETTask AddOrUpdate(this UnitCacheComponent self, long id, ListComponent<Entity> entityList)
        {
            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                foreach (Entity entity in entityList)
                {
                    string key = entity.GetType().Name;
                    if (!self.UnitCachesDic.TryGetValue(key, out UnitCache unitCache))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCachesDic.Add(key, unitCache);
                    }

                    unitCache.AddOrUpdate(entity);
                    list.Add(entity);
                }

                if (list.Count > 0)
                {
                    await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(id, list);
                }
            }
        }
    }
}