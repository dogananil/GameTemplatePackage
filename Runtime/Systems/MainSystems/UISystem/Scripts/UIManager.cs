using Cysharp.Threading.Tasks;
using GravityRush.BootSystem;
using GravityRush.ServiceLocatorSystem;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace GravityRush.UISystem
{
    public class UIManager : MonoBehaviour, IBootAction
    {
        private Dictionary<string,View> _initedViews=new();
        private LoadAddressable LoadAddressableService => ServiceLocator.Instance.GetService<LoadAddressable>();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _loadingEnabled = false;
        public bool LoadingEnabled => _loadingEnabled;
        public async UniTask<BootMessage> BootActionInit()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                return new BootMessage();
            }
            catch (System.Exception ex)
            {
                return new BootMessage(ex.Message, nameof(UIManager));
            }
        }
        public async UniTask ShowAsync(IUIData data)
        {
            View uiPrefab = await GetView(data);
            // Show UI
            uiPrefab.Show(data);

        }
        internal async UniTask ShowLoading()
        {
            LoadingViewData data = new LoadingViewData() { ViewName = "LoadingView" };
            LoadingView uiPrefab = await GetView(data) as LoadingView;
            uiPrefab.Show(data);
            _loadingEnabled=true;
        }
        internal void HideLoading()
        {
            if (!_initedViews.ContainsKey("LoadingView"))
                return;

            View uiPrefab = _initedViews["LoadingView"];
            uiPrefab.Hide();
            _loadingEnabled = false;
        }
        public void Hide(IUIData data)
        {
            if (!_initedViews.ContainsKey(data.ViewName))
                return;
            View uiPrefab = _initedViews[data.ViewName];
            // Hide UI
            uiPrefab.Hide();
        }
        public void HideAll()
        {
            foreach (var view in _initedViews)
            {
                if(view.Key!="LoadingView")
                    view.Value.Hide();
            }
        }
        private async Task<View> GetView(IUIData data)
        {
            View uiPrefab;
            // Show UI
            if (!_initedViews.ContainsKey(data.ViewName))
            {
                // Load UI prefab
                uiPrefab = await LoadAddressableService.LoadAsset<View>(data.ViewName, _cancellationTokenSource.Token);
                // Instantiate UI prefab
                uiPrefab = Instantiate(uiPrefab,transform);
                // Initialize UI prefab
                uiPrefab.Init();
                // Add UI prefab to the canvas
                _initedViews.Add(data.ViewName, uiPrefab);
            }
            uiPrefab = _initedViews[data.ViewName];
            return uiPrefab;
        }
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
    public interface IUIData
    {
        public string ViewName { get; set; }
    }
}


