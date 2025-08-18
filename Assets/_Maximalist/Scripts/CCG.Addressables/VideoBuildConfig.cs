using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay Andrade Hunt
//  Company:     Maximalist Ltd
//  Created:     18/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the  client.
// ------------------------------------------------------------------------------

namespace CCG.Addressables
{
    [CreateAssetMenu(menuName = "Video/Build Config", fileName = "VideoBuildConfig")]
    public class VideoBuildonfigSO : ScriptableObject
    {
        [Header("Addressables")]
        public AssetReferenceT<VideoClip> addressableClip;
        public string addressablesGroupName = "AndroidVideo";

        [Header("Build rule")]
        public bool includeOnlyOnAndroid = true;
    }
}