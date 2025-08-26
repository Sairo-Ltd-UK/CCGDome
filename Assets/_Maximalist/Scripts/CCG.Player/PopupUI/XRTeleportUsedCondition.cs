using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace CCG.Player.Prompt
{
	public class XRTeleportUsedCondition : CompletionCondition
	{
		[SerializeField] private TeleportationProvider provider;
		private bool teleported = false;

		public override void OnBegin()
		{
			teleported = false;

			if (provider != null)
			{
				provider.locomotionEnded += OnTeleportEnd;
			}
		}

		private void OnTeleportEnd(LocomotionProvider eventProvider)
		{
			Debug.Log("[TUC] OnTeleportEnd");

			if (eventProvider != provider)
				return;

			teleported = true;

			if (provider != null)
				provider.locomotionEnded -= OnTeleportEnd;
		}

		[ContextMenu("TestCheckComplete")]
		public void TestCheckComplete()
		{
			teleported = true;
		}


		public override bool CheckComplete()
		{
			return teleported;
		}
	}
}
