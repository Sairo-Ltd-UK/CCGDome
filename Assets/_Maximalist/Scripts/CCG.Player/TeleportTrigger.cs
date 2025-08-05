using UnityEngine;

namespace CCG.Player
{
	public class TeleportTrigger : MonoBehaviour
	{
		[Tooltip("Target position to teleport player to")]
		public Transform teleportTargetLocation;

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"TeleportTrigger: OnTriggerEnter with {other.name}");

			if (other.TryGetComponent(out FastTravelProvider fastTravelProvider) == false)
				return;

			fastTravelProvider.GenerateTeleportRequest(teleportTargetLocation.position, teleportTargetLocation.rotation);
		}
	}
}
