using UnityEngine;

namespace CCG.Audio
{
	public class AudioComponent : MonoBehaviour
	{
		[SerializeField] private AudioSource targetSource;

		private void Reset()
		{
			targetSource = GetComponent<AudioSource>();

			if(targetSource == null)
				targetSource = gameObject.AddComponent<AudioSource>();

			targetSource.enabled = false;
		}

		private void OnEnable()
		{
			if(AudioManager.IsClient == false)
				return;

			targetSource.enabled = true;
		}

		private void OnDisable()
		{
			if (AudioManager.IsClient == false)
				return;

			targetSource.enabled = false;
		}

	}
}
