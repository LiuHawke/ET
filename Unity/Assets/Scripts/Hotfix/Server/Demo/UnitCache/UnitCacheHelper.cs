using System;
using System.Threading.Tasks;
using System.Xml;

namespace ET.Server
{
    public static partial class UnitCacheHelper
    {
        /// <summary>
        /// 保存或者更新玩家缓存
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static async ETTask AddOrUpdateUnitCache<T>(this T self) where T : Entity, IUnitCache
        {
            Other2UnitCache_AddOrUpdateUnit request = Other2UnitCache_AddOrUpdateUnit.Create();
            request.UnitId = self.Id;
            request.Entitys.Add(self.ToBson());

            await self.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.UnitCache.ActorId, request);
        }

        /// <summary>
        /// 获取玩家缓存
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static async ETTask<Unit> GetUnitCache(Scene scene, long unitId)
        {
            Other2UnitCache_GetUnit message = Other2UnitCache_GetUnit.Create();
            message.UnitId = unitId;
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit) await scene.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.UnitCache.ActorId, message);
            if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
            {
                return null;
            }
        
            int indexOf = queryUnit.ComponentNameList.IndexOf(nameof (Unit));
            Unit unit = MongoHelper.Deserialize<Unit>(queryUnit.EntityList[indexOf]);;
            if (unit == null)
            {
                return null;
            }
            scene.GetComponent<UnitComponent>().AddChild(unit);
            foreach (byte[] bytes in queryUnit.EntityList)
            {
                Entity entity = MongoHelper.Deserialize<Entity>(bytes);
                if (entity == null || entity is Unit)
                {
                    continue;
                }
                unit.AddComponent(entity);
            }
            return unit;
        }

        /// <summary>
        /// 获取玩家组件缓存
        /// </summary>
        /// <param name="unitId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async ETTask<T> GetUnitComponentCache<T>(Scene root, long unitId) where T : Entity, IUnitCache
        {
            Other2UnitCache_GetUnit request = Other2UnitCache_GetUnit.Create();
            request.UnitId = unitId;
            request.ComponentNameList.Add(typeof (T).Name);
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit) await root.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.UnitCache.ActorId, request);
            if (queryUnit.Error == ErrorCode.ERR_Success && queryUnit.EntityList.Count > 0)
            {
                return queryUnit.EntityList[0] as T;
            }
            return null;
        }
        
        /// <summary>
        /// 删除玩家缓存
        /// </summary>
        /// <param name="unitId"></param>
        public static async ETTask DeleteUnitCache(Scene root, long unitId)
        {
            Other2UnitCache_DeleteUnit request = Other2UnitCache_DeleteUnit.Create();
            request.UnitId = unitId;
            await root.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.UnitCache.ActorId, request);
        }
        
        
        /// <summary>
        /// 保存Unit及Unit身上组件到缓存服及数据库中
        /// </summary>
        /// <param name="unit"></param>
        public static void AddOrUpdateUnitAllCache(Scene root, Unit unit)
        {
            Other2UnitCache_AddOrUpdateUnit request = Other2UnitCache_AddOrUpdateUnit.Create();
            request.UnitId = unit.Id;
            request.Entitys.Add(unit.ToBson());
            
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is IUnitCache)
                {
                    request.Entitys.Add(entity.ToBson());
                }
                
            }
            root.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.UnitCache.ActorId, request).Coroutine();
        }
    }
}