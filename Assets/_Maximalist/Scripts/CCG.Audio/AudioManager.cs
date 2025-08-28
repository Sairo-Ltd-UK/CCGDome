// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     4/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CCG.Audio
{
	public static class AudioManager
	{
		public static bool IsClient = false;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static async void InitializeAudioManager()
		{
#if UNITY_SERVER

			if (Application.isEditor)
			{
				IsClient = true;
				return;
			}

			IsClient = false;

#elif !UNITY_SERVER
			IsClient = true;
#endif
		}

	}
}
