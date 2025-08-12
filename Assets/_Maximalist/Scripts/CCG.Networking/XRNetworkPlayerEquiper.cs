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
    public class XRNetworkPlayerEquiper : MonoBehaviour
    {
        [SerializeField] private GameObject itemToEquip;

        private void Start()
        {
            if(itemToEquip)
                itemToEquip.transform.SetParent(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out XRNetworkPlayerEquipmentManager interactor))
                return;

            interactor.EquipItemToRightHand(itemToEquip);
        }
    }
}
