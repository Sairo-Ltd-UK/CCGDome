using UnityEngine;

namespace CCG.Player
{
	public class FastTravelDestination : MonoBehaviour
	{
		[SerializeField] private AudioSource fastTravelAudioSource;
		[SerializeField] private AudioClip fastTravelSoundEffect;

		public void PlaySound()
		{
			if (fastTravelAudioSource == null)
				return;

			if (fastTravelSoundEffect == null)
				return;

			fastTravelAudioSource.clip = fastTravelSoundEffect;
			fastTravelAudioSource.Play();
		}
	}
}
