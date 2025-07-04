using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
namespace GravityRush.ServiceLocatorSystem
{   
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    var locatorObject = new GameObject("ServiceLocator");
                    _instance = locatorObject.AddComponent<ServiceLocator>();
                    DontDestroyOnLoad(locatorObject);
                }
                return _instance;
            }
        }

        public async UniTask RegisterService<T>(string addressableKey = null, CancellationToken cancellationToken = default, bool dontDestroyOnLoad = false) where T : Component
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                if (!string.IsNullOrEmpty(addressableKey) && typeof(T) != typeof(LoadAddressable))
                {
                    var loadAddressable = GetService<LoadAddressable>();
                    if (loadAddressable != null)
                    {
                        var prefab = await loadAddressable.LoadAsset<T>(addressableKey, cancellationToken);
                        if (prefab != null)
                        {
                            var serviceObject = Instantiate(prefab.gameObject);
                            if (dontDestroyOnLoad)
                            {
                                DontDestroyOnLoad(serviceObject);
                            }
                            var service = serviceObject.GetComponent<T>();
                            _services[type] = service;
                        }
                    }
                    else
                    {
                        Debug.LogError("LoadAddressable service is not registered.");
                    }
                }
                else
                {
                    var serviceObject = new GameObject(type.Name);
                    var service = serviceObject.AddComponent<T>();
                    _services[type] = service;
                }
            }
            else
            {
                Debug.LogWarning($"Service of type {type} is already registered.");
            }
        }

        public T GetService<T>()
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            else
            {
                Debug.LogError($"Service of type {type} is not registered.");
                return default;
            }
        }

        public void UnregisterService<T>()
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
            else
            {
                Debug.LogWarning($"Service of type {type} is not registered.");
            }
        }
    }
}