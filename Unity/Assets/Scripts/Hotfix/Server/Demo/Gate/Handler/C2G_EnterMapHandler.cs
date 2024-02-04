using System;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_EnterMapHandler: MessageSessionHandler<C2G_EnterMap, G2C_EnterMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            //从数据库或者缓存中加载出Unit实体及其相关组件
            (bool isNewPlayer, Unit unit) = await UnitHelper.LoadUnit(player);

            //玩家Unit上线后的初始化操作
            await UnitHelper.InitUnit(unit, isNewPlayer);
            response.MyId = unit.Id;

            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.Zone(), "Game");

            // // 等到一帧的最后面再传送，先让G2C_EnterMap返回，否则传送消息可能比G2C_EnterMap还早
            TransferHelper.TransferAtFrameFinish(unit, startSceneConfig.ActorId, startSceneConfig.Name).Coroutine();
        }
    }
}