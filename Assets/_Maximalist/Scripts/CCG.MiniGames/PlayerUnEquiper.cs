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
	public class PlayerUnEquiper : MonoBehaviour
	{
		[SerializeField] private EquipmentSlotType slotType;
		[SerializeField] private Transform locationToReturnTo;

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out PlayerEquipmentManager interactor))
			{
				PlayerEquipment equipment =	interactor.UnequipItem(slotType);

				if (locationToReturnTo == null)
					return;

				equipment.transform.position = locationToReturnTo.position;
				equipment.transform.rotation = locationToReturnTo.rotation;
			}
		}
	}
}
