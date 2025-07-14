using UnityEngine;

namespace CCG.Networking
{
    public static class UGSBootstrap
    {
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static async void Init()
		{
			//await UnityServices.InitializeAsync();
			//await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
	}
}
