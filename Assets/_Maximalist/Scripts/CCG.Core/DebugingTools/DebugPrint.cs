// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.Core.Debuging
{
	public class DebugPrint : MonoBehaviour
	{
		[SerializeField] private string debugString = "This is a test debug message";
		public void PrintDebug()
		{
			DebugLogger.Log(debugString);
		}
	}
}