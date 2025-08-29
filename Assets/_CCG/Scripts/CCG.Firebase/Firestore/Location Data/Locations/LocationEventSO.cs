// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay A Hunt
//  Company:     Maximalist Ltd
//  Created:     11/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.Firebase
{
	[CreateAssetMenu(fileName = "LocationEvent", menuName = "CCG/Location Event", order = 0)]
	public class LocationEventSO : ScriptableObject
	{
		//Time is auto saved when sent
		
		[Header("Required")]
		[SerializeField] private string locationName;

		public string LocationName => locationName?.Trim();
	}
}