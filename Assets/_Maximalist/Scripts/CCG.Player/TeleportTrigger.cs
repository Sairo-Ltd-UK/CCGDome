using UnityEngine;

namespace CCG.Player
{
	public class TeleportTrigger : MonoBehaviour
	{
		[Tooltip("Target position to teleport player to")]
		public Transform teleportTargetLocation;
		[SerializeField] private AudioSource teloportAudioSource;
        [SerializeField] private AudioClip teloportSoundEffect;

        private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"TeleportTrigger: OnTriggerEnter with {other.name}");
			PlaySound();

            if (other.TryGetComponent(out FastTravelProvider fastTravelProvider) == false)
				return;

			fastTravelProvider.GenerateTeleportRequest(teleportTargetLocation.position, teleportTargetLocation.rotation);
		}

		private void PlaySound()
		{ 
			if(teloportAudioSource == null)
				return;

			if (teloportSoundEffect == null)
				return;

			teloportAudioSource.clip = teloportSoundEffect;

            teloportAudioSource.Play();
		}
	}
}
