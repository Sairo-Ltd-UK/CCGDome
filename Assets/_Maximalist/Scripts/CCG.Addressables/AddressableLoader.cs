using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
    public static class AddressableLoader
    {
        // Synchronous: blocks until the local asset finishes loading
        public static LoadedAddressable<T> LoadSync<T>(AssetReferenceT<T> reference) where T : UnityEngine.Object
        {
            var handle = reference.LoadAssetAsync<T>();
            var asset = handle.WaitForCompletion();
            if (!handle.IsValid() || asset == null)
                throw new Exception($"Failed to load addressable of type {typeof(T).Name} from {reference.AssetGUID}");
            return new LoadedAddressable<T>(handle, asset);
        }

        // Asynchronous: await when you do not want to block the main thread
        public static async Task<LoadedAddressable<T>> LoadAsync<T>(
            AssetReferenceT<T> reference,
            CancellationToken ct = default) where T : UnityEngine.Object
        {
            var handle = reference.LoadAssetAsync<T>();
            while (!handle.IsDone)
            {
                if (ct.IsCancellationRequested)
                {
                    if (handle.IsValid()) UnityEngine.AddressableAssets.Addressables.Release(handle);
                    ct.ThrowIfCancellationRequested();
                }
                await Task.Yield();
            }

            var asset = handle.Result;
            if (!handle.IsValid() || asset == null)
                throw new Exception($"Failed to load addressable of type {typeof(T).Name} from {reference.AssetGUID}");

            return new LoadedAddressable<T>(handle, asset);
        }
    }

    /// <summary>
    /// A small wrapper that carries the loaded asset and knows how to release it.
    /// Use in a using(...) block or call Dispose/Release when done.
    /// </summary>
    public sealed class LoadedAddressable<T> : IDisposable where T : UnityEngine.Object
    {
        public readonly AsyncOperationHandle<T> Handle;
        public readonly T Asset;
        bool _released;

        internal LoadedAddressable(AsyncOperationHandle<T> handle, T asset)
        {
            Handle = handle;
            Asset = asset;
        }

        public void Release()
        {
            if (_released) return;
            if (Handle.IsValid()) UnityEngine.AddressableAssets.Addressables.Release(Handle);
            _released = true;
        }

        public void Dispose() => Release();
    }
}