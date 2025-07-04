using Cysharp.Threading.Tasks;
using GravityRush.UISystem;
using System.Threading;
using UnityEngine;
public class LoadingViewData : IUIData
{
    public string ViewName { get; set; }
}
public class LoadingView : View
{
    [SerializeField] private float rotationSpeed = 180f; // Degrees per second
    [SerializeField] private float animationDuration = -1f; // Set to -1 for indefinite rotation
    [SerializeField] private Transform _loadingHolderTransform;

    private bool _isRotating = false;
    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    public override void Show(IUIData data)
    {
        base.Show(data);
        if (data is not LoadingViewData loadingViewData)
            return;
        StartLoading().Forget();
    }
    private async UniTask StartLoading()
    {
        if (_isRotating)
        {
            Debug.LogWarning("Rotation is already in progress.");
            return;
        }

        _isRotating = true;
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken linkedToken = _cancellationTokenSource.Token;

        float startTime = Time.time;

        try
        {
            while (!linkedToken.IsCancellationRequested && (animationDuration <= 0f || (Time.time - startTime < animationDuration)))
            {
                _loadingHolderTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                await UniTask.Yield(linkedToken);
            }
        }
        finally
        {
            _isRotating = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
    public override void Hide()
    {
        base.Hide();
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
