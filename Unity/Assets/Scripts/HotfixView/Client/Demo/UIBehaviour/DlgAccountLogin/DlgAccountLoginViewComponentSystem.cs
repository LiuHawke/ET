
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[EntitySystemOf(typeof(DlgAccountLoginViewComponent))]
	[FriendOfAttribute(typeof(ET.Client.DlgAccountLoginViewComponent))]
	public static partial class DlgAccountLoginViewComponentSystem
	{
		[EntitySystem]
		private static void Awake(this DlgAccountLoginViewComponent self)
		{
			self.uiTransform = self.Parent.GetParent<UIBaseWindow>().uiTransform;
		}


		[EntitySystem]
		private static void Destroy(this DlgAccountLoginViewComponent self)
		{
			self.DestroyWidget();
		}
	}


}
