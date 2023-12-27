namespace ET.Server
{
    [ChildOf(typeof (AccountInfoCompoent))]
    public class AccountInfo: Entity, IAwake
    {
        public string Account;

        public string Password;
    }
}