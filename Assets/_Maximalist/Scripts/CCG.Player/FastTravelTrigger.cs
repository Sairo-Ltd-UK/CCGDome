using UnityEngine;

namespace CCG.Player
{
	public class FastTravelTrigger : MonoBehaviour
	{
		[Tooltip("Target position to teleport player to")]
		[SerializeField] private FastTravelDestination fastTravelTargetLocation;

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"FastTravelTrigger: OnTriggerEnter with {other.name}");
			fastTravelTargetLocation.PlaySound();

			if (other.TryGetComponent(out FastTravelProvider fastTravelProvider) == false)
				return;

			fastTravelProvider.GenerateTeleportRequest(fastTravelTargetLocation.transform.position, fastTravelTargetLocation.transform.rotation);
		}

		private void OnDrawGizmos()
		{
			if(fastTravelTargetLocation == null)
				return;

			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, fastTravelTargetLocation.transform.position);
		}

	}
}
