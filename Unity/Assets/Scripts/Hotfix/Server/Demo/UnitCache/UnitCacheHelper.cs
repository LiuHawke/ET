using System.Threading.Tasks;
using System.Xml;

namespace ET.Server
{
    public static partial class UnitCacheHelper
    {
        public static async ETTask AddOrUpdateUnitCache<T>(this T self, ActorId sceneInstanceId) where T : Entity, IUnitCache
        {
            Scene root = self.Root();
            Other2UnitCache_AddOrUpdateUnit request = Other2UnitCache_AddOrUpdateUnit.Create();
            request.UnitId = self.Id;
            request.EntityTypes.Add(typeof (T).FullName);
            request.EntityBytes.Add(self.ToBson());

            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }

        // public static async ETTask<Unit> GetUnitCache(Scene scene, long unitId)
        // {
        //     // long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).Instanceid;
        //     Other2UnitCache_GetUnit message = Other2UnitCache_GetUnit.Create();
        //     UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await Messagellelper.CallActor(instanceld, message);
        //     if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EnityList.Count <= 0) return null;
        //
        //     await Task.CompletedTask;
        //     return null;
        // }

        public static async ETTask<T> GetUnitCache<T>(Scene root, ActorId sceneInstanceId, long unitId) where T : Entity, IUnitCache
        {
            Other2UnitCache_GetUnit request = Other2UnitCache_GetUnit.Create();
            request.UnitId = unitId;
            request.ComponentNameList.Add(typeof (T).Name);
            UnitCache2Other_GetUnit response = (UnitCache2Other_GetUnit) await root.Root().GetComponent<MessageSender>().Call(sceneInstanceId, request);
            if (response.Error == ErrorCode.ERR_Success && response.EnityList.Count > 0)
            {
                return response.EnityList[0] as T;
            }

            return null;
        }
        
        public static async ETTask DeleteUnitCache(long unitId)
        {
            // long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).Instanceid;
            Other2UnitCache_DeleteUnit request = Other2UnitCache_DeleteUnit.Create();
            request.UnitId = unitId;
            await Task.CompletedTask;
        }
    }
}