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
