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

namespace CCG.Networking
{
    public class XRNetworkPlayerEquipmentManager : MonoBehaviour
    {
        public Transform rightHandEquipmentSlot;

        public void EquipItemToRightHand(GameObject itemToEquip)
        {
            if(rightHandEquipmentSlot == null) 
                return;

            UnequipItemInRightHand();
            itemToEquip.transform.SetParent(rightHandEquipmentSlot);
            itemToEquip.transform.localPosition = Vector3.zero;
            itemToEquip.transform.localRotation = Quaternion.identity;
        }

        public void UnequipItemInRightHand()
        {
            if (rightHandEquipmentSlot == null)
                return;

            rightHandEquipmentSlot.DetachChildren();
        }
    }
}
