// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     12/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames
{
	public class PlayerEquiper : MonoBehaviour
	{
		[SerializeField] private PlayerEquipment itemToEquip;
		[SerializeField] private EquipmentSlotType slotType;

		private void Start()
		{
			if (itemToEquip)
				itemToEquip.transform.SetParent(null);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PlayerEquipmentManager interactor))
				interactor.EquipItem(slotType, itemToEquip);
		}
	}
}
