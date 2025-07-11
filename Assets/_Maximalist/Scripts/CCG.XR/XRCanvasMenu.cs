// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CCG.XR
{
	public class XRCanvasMenu : MonoBehaviour
	{
		private string mapName;

		public void ButtonMap(int _map)
		{
			if (_map == 1)
			{
				mapName = "SceneVR-Basic";
			}
			else if (_map == 2)
			{
				mapName = "SceneVR-Common";
			}
			else if (_map == 3)
			{
				mapName = "SceneVR-UnityDemo";
			}

			//Debug.Log(name + " loading map: " + mapName);

			SceneManager.LoadScene(mapName);

		}
	}
}