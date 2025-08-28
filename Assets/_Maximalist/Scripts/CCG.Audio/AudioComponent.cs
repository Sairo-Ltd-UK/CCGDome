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
