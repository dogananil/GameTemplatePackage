using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAddressable : MonoBehaviour
{
    public async UniTask<T> LoadAsset<T>(string addressableKey, CancellationToken cancellationToken) where T : UnityEngine.Object
    {
        AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(addressableKey);

        try
        {
            while (!handle.IsDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    handle.Release();
                    throw new OperationCanceledException(cancellationToken);
                }
                await UniTask.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                UnityEngine.Object result = handle.Result;

                if (result is T asset)
                {
                    return asset;
                }
                else if (result is GameObject gameObject && typeof(T).IsSubclassOf(typeof(Component)))
                {
                    var component = gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                    else
                    {
                        throw new LoadAddressableException($"The prefab at {addressableKey} does not contain a component of type {typeof(T)}.");
                    }
                }
                else
                {
                    throw new LoadAddressableException($"The asset at {addressableKey} is not of type {typeof(T)}.");
                }
            }
            else
            {
                throw new LoadAddressableException($"Failed to load asset at {addressableKey}. Status: {handle.Status}");
            }
        }
        catch (OperationCanceledException)
        {
            handle.Release();
            throw;
        }
    }
}


public class LoadAddressableException : Exception
{
    public LoadAddressableException(string message) : base(message) { }
    public LoadAddressableException(string message, Exception innerException) : base(message, innerException) { }
}
