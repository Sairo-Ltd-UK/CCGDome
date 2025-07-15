//---------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.XR
{
	public class XRStaticVariables : MonoBehaviour
	{
		// here we will store player and game information, that needs to be saved and passed between scenes

		public static int handValue = 0; // 1 right hand, 2 left hand, used as a shortcut to tell interactable which controller was used
	}
}