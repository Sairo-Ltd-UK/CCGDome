using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
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
    [RequireComponent(typeof(VideoPlayer))]
    public class AddressableVideoFromConfig : MonoBehaviour
    {
        [SerializeField] private VideoBuildonfigSO config;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool loadSynchronously = true;

        [SerializeField] private VideoPlayer player;
        private LoadedAddressable<VideoClip> loaded;

        void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        async void Start()
        {
            if (!playOnStart) return;
            await LoadAndPlay();
        }

        public async Task LoadAndPlay(CancellationToken ct = default)
        {
            if (!config)
            {
                Debug.LogError("VideoBuildConfig not assigned.");
                return;
            }

            if (loaded == null)
            {
                loaded = loadSynchronously
                    ? AddressableLoader.LoadSync(config.addressableClip)
                    : await AddressableLoader.LoadAsync(config.addressableClip, ct);
            }

            if (loaded.Asset == null)
            {
                Debug.LogError("Addressable VideoClip failed to load.");
                return;
            }

            player.source = VideoSource.VideoClip;
            player.clip = loaded.Asset;
            player.Play();
        }

        void OnDestroy()
        {
            // Free the Addressables handle when this object is destroyed
            if (loaded != null)
            {
                loaded.Release();
                loaded = null;
            }
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }
}