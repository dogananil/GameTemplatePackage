using Cysharp.Threading.Tasks;
using Fusion;
using GravityRush.ServiceLocatorSystem;
using GravityRush.UISystem;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace GravityRush.BootSystem
{
    public class Boot : MonoBehaviour
    {
        private List<IBootAction> _bootActions;
        private CancellationTokenSource _cancellationTokenSource;

        private async void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            try
            {
                await RegisterServices();
                InitializeBootActions();
                await ExecuteBootActions();
                await CompleteBoot();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Boot process was canceled.");
            }
        }

        private async UniTask CompleteBoot()
        {
            UIManager UIManager = ServiceLocator.Instance.GetService<UIManager>();
            RegionSelectionData regionSelectionData = new()
            {
                Regions = await NetworkRunner.GetAvailableRegions(cancellationToken: _cancellationTokenSource.Token),
                ViewName = "RegionSelectionView"
            };
            await UIManager.ShowAsync(regionSelectionData);

            NetworkManager networkManager = ServiceLocator.Instance.GetService<NetworkManager>();
            await UniTask.WaitUntil(() => !string.IsNullOrEmpty(networkManager.Region));

            HomeViewData homeViewData = new()
            {
                ViewName = "HomeView"
            };
            await UIManager.ShowAsync(homeViewData);
        }

        private async UniTask RegisterServices()
        {
            // Register LoadAddressable service first
            await ServiceLocator.Instance.RegisterService<LoadAddressable>(cancellationToken: _cancellationTokenSource.Token);

            // Example service registration using Addressables
            await ServiceLocator.Instance.RegisterService<App>(cancellationToken: _cancellationTokenSource.Token);
            // Register other services here
            await ServiceLocator.Instance.RegisterService<UIManager>(cancellationToken: _cancellationTokenSource.Token);
            await ServiceLocator.Instance.RegisterService<NetworkManager>(cancellationToken: _cancellationTokenSource.Token);
            await ServiceLocator.Instance.RegisterService<LevelManager>(cancellationToken: _cancellationTokenSource.Token);
            await ServiceLocator.Instance.RegisterService<PlayerManager>(cancellationToken: _cancellationTokenSource.Token);
        }

        private void InitializeBootActions()
        {
            _bootActions = new List<IBootAction>();

            // Add boot actions in the desired order
            _bootActions.Add(ServiceLocator.Instance.GetService<App>());
            _bootActions.Add(ServiceLocator.Instance.GetService<UIManager>());
            _bootActions.Add(ServiceLocator.Instance.GetService<NetworkManager>());
            _bootActions.Add(ServiceLocator.Instance.GetService<LevelManager>());
            _bootActions.Add(ServiceLocator.Instance.GetService<PlayerManager>());

            // Add other boot actions here
        }

        private async UniTask ExecuteBootActions()
        {
            foreach (var action in _bootActions)
            {
                var bootMessage = await action.BootActionInit().AttachExternalCancellation(_cancellationTokenSource.Token);
                if (!string.IsNullOrEmpty(bootMessage.ErrorMessage))
                {
                    Debug.LogError($"Boot action error in {bootMessage.Source}: {bootMessage.ErrorMessage}");
                }
            }
        }
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
    public interface IBootAction
    {
        UniTask<BootMessage> BootActionInit();
    }

    public class BootMessage
    {
        public string ErrorMessage { get; set; }
        public string Source { get; set; }

        public BootMessage(string errorMessage = "", string source = "")
        {
            ErrorMessage = errorMessage;
            Source = source;
        }
    }
    public enum ExceptionType
    {
        None,
        UserError,
        SystemError,
        NetworkError,
        DataError,
        TimeoutError,
        DeveloperMistake,
        UnknownError
    }
}