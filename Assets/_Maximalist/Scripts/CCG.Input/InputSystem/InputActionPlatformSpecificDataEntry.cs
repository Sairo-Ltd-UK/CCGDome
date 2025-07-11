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

namespace CCG.CustomInput
{
	[System.Serializable]
	public struct InputActionPlatformSpecificDataEntry
	{
		[SerializeField] private string inputActionTextName;
		[SerializeField] private Sprite inputActionSprite;

		public string InputActionTextName { get => inputActionTextName; }
		public Sprite InputActionSprite { get => inputActionSprite; }
	}
}

