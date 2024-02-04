namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCacheComponent))]
    [FriendOf(typeof(UnitCache))]
    public static partial class UnitCacheSystem
    {
        [EntitySystem]
        private static void Awake(this UnitCacheComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this UnitCacheComponent self)
        {

        }

        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentDic.TryGetValue(unitId,out entity))
            {
                entity = await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            return entity;
        }
        
        public static void Delete(this UnitCache self, long id)
        {
            if (self.CacheComponentDic.TryGetValue(id,out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentDic.Remove(id);
            }
        }
        
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (self.CacheComponentDic.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity != oldEntity)
                {
                    oldEntity.Dispose();
                }

                self.CacheComponentDic.Remove(entity.Id);
            }

            self.CacheComponentDic.Add(entity.Id, entity);
        }
    }
}

