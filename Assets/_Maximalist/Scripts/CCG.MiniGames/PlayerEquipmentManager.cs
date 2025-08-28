// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     12/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCG.MiniGames
{
	public enum EquipmentSlotType
	{
		RightHand,
		LeftHand,
	}

	[Serializable]
	public class EquipmentSlot
	{
		public EquipmentSlotType slotType;
		public Transform slotTransform;
		[HideInInspector] public PlayerEquipment equippedItem;
	}

	public class PlayerEquipmentManager : MonoBehaviour
	{
		[SerializeField] private EquipmentSlot[] slots;

		private Dictionary<EquipmentSlotType, EquipmentSlot> slotLookup;

		private void Awake()
		{
			slotLookup = new Dictionary<EquipmentSlotType, EquipmentSlot>();
			foreach (var slot in slots)
				slotLookup[slot.slotType] = slot;
		}

		public void EquipItem(EquipmentSlotType slotType, PlayerEquipment itemToEquip)
		{
			if (!slotLookup.TryGetValue(slotType, out var slot))
				return;

			if (slot.slotTransform == null || itemToEquip == null)
				return;

			UnequipItem(slotType);

			itemToEquip.transform.SetParent(slot.slotTransform);
			itemToEquip.transform.localPosition = Vector3.zero;
			itemToEquip.transform.localRotation = Quaternion.identity;
			itemToEquip.OnEquiped();

			slot.equippedItem = itemToEquip;
		}

		public PlayerEquipment UnequipItem(EquipmentSlotType slotType)
		{
			if (!slotLookup.TryGetValue(slotType, out var slot))
				return null;

			if (slot.slotTransform == null)
				return null;

			slot.slotTransform.DetachChildren();

			if (slot.equippedItem == null)
				return null;

			PlayerEquipment equippedItem = slot.equippedItem;

			equippedItem.OnUnEquiped();
			equippedItem = null;

			return equippedItem;
		}
	}
}

