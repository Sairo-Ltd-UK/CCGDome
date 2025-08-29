// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     04/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

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
