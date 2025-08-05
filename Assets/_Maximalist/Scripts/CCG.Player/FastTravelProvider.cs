using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine;

namespace CCG.Player
{
	public class FastTravelProvider : LocomotionProvider
    {
		[SerializeField] private TeleportationProvider teleportationProvider;

        public void GenerateTeleportRequest(Vector3 destinationPosition, Quaternion destinationRotation)
		{
			TeleportRequest teleportRequest = new TeleportRequest
			{
				destinationPosition = destinationPosition,
				destinationRotation = destinationRotation
			};

			Debug.Log($"Generating teleport request to position: {destinationPosition}, rotation: {destinationRotation}");

            if (teleportationProvider.QueueTeleportRequest(teleportRequest))
			{
				Debug.Log("Teleport request queued successfully.");
			}
			else
			{
				Debug.LogError("Failed to queue teleport request.");
			}
        }
    }
}
