// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     4/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace CCG.Audio
{
	public class MusicManager : MonoBehaviour
	{
		public static MusicManager Instance;

		[Header("Default Track")]
		[SerializeField] private AudioClip mainRoomClip;
		[SerializeField] private float fadeDuration = 1.5f;
		[Space]
		[SerializeField] private AudioSource activeSource;
		[SerializeField] private AudioSource inactiveSource;
		private Coroutine fadeCoroutine;

		private void Awake()
		{
			if (AudioManager.IsClient == false)
				return;

			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			if (mainRoomClip != null)
				PlayMusic(mainRoomClip);
		}

		public void PlayMusic(AudioClip clip)
		{
			if (clip == null) return;

			// Already playing this clip? Do nothing
			if (activeSource.isPlaying && activeSource.clip == clip)
				return;

			if (fadeCoroutine != null)
				StopCoroutine(fadeCoroutine);

			// Swap active/inactive roles
			AudioSource temp = activeSource;
			activeSource = inactiveSource;
			inactiveSource = temp;

			// Load new clip into the now-active source
			activeSource.clip = clip;
			activeSource.volume = 0f;
			activeSource.Play();

			// Crossfade
			fadeCoroutine = StartCoroutine(Crossfade(inactiveSource, activeSource, fadeDuration));
		}

		public void PlayMainRoomMusic()
		{
			if (mainRoomClip != null)
				PlayMusic(mainRoomClip);
		}

		private IEnumerator Crossfade(AudioSource from, AudioSource to, float duration)
		{
			float t = 0f;
			float startVolFrom = from.isPlaying ? from.volume : 0f;

			while (t < duration)
			{
				t += Time.deltaTime;
				float progress = t / duration;

				if (from.isPlaying)
					from.volume = Mathf.Lerp(startVolFrom, 0f, progress);

				to.volume = Mathf.Lerp(0f, 1f, progress);

				yield return null;
			}

			if (from.isPlaying)
				from.Stop();

			to.volume = 1f;
		}
	}
}
