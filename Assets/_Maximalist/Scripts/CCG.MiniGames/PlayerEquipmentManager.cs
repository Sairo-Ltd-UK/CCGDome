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
    public class PlayerEquipmentManager : MonoBehaviour
    {
        public Transform rightHandEquipmentSlot;
        public PlayerEquipment rightHandEquipment;

        public void EquipItemToRightHand(PlayerEquipment itemToEquip)
        {
            if (rightHandEquipmentSlot == null) 
                return;

            //CW I dont have a or key on this keyboard
            if (itemToEquip == null)
                return;

            UnequipItemInRightHand();
            itemToEquip.transform.SetParent(rightHandEquipmentSlot);
            itemToEquip.transform.localPosition = Vector3.zero;
            itemToEquip.transform.localRotation = Quaternion.identity;
            itemToEquip.OnEquiped();

            rightHandEquipment = itemToEquip;
        }

        public void UnequipItemInRightHand()
        {
            if (rightHandEquipmentSlot == null)
                return;

            rightHandEquipmentSlot.DetachChildren();

            if (rightHandEquipment == null)
                return;

            rightHandEquipment.OnUnEquiped();
            rightHandEquipment = null;
        }
    }
}
