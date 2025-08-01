using UnityEngine;

namespace CCG.Networking
{
	public class TeleportTrigger : MonoBehaviour
	{
		[Tooltip("Target position to teleport player to")]
		public Transform teleportTargetLocation;

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player"))
				return;

			if (other.TryGetComponent(out XRNetworkPlayerScript playerTeleport))
			{
				playerTeleport.CmdTeleportToPosition(teleportTargetLocation.position);
			}
		}
	}
}
