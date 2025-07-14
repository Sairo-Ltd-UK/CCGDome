using UnityEngine;
using Mirror;


namespace CCG.Networking
{
	public class ServerBootstrapper : MonoBehaviour
	{
		void Start()
		{
#if UNITY_SERVER
			Debug.Log("Running in headless server mode. Starting Mirror server...");
			NetworkManager.singleton.StartServer();
#else
			Debug.Log("Not a server build. ServerBootstrapper will do nothing.");
#endif
		}
	}
}
