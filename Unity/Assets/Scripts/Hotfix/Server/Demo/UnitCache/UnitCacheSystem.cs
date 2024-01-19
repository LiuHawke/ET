namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCacheComponent))]
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
    }
}

