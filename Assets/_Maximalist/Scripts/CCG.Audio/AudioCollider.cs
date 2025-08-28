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
	public class RoomMusicTrigger : MonoBehaviour
	{
		public AudioClip musicClip;

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("LocalPlayer") == false)

				return;

			if (musicClip != null)
				MusicManager.Instance.PlayMusic(musicClip);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("LocalPlayer") == false)
				return;

			MusicManager.Instance.PlayMainRoomMusic();
		}
	}
}
