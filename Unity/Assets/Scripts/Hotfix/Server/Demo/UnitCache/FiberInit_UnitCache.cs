namespace ET.Server
{
    [Invoke((long)SceneType.UnitCache)]
    public class FiberInit_UnitCache:AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<UnitComponent>();
            
            await ETTask.CompletedTask;
        }
    }
}

