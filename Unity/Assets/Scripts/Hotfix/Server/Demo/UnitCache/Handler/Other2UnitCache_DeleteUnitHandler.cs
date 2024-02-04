namespace ET.Server
{
    [MessageHandler(SceneType.UnitCache)]
    public class Other2UnitCache_DeleteUnitHandler: MessageHandler<Scene, Other2UnitCache_DeleteUnit, UnitCache2Other_DeleteUnit>
    {
        protected override ETTask Run(Scene unit, Other2UnitCache_DeleteUnit request, UnitCache2Other_DeleteUnit response)
        {
            throw new System.NotImplementedException();
        }
    }
}

