namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgAccountLogin :Entity,IAwake,IUILogic
	{

		public DlgAccountLoginViewComponent View { get => this.GetComponent<DlgAccountLoginViewComponent>();} 

		 

	}
}
