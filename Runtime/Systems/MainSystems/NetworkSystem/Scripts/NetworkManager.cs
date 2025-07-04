using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Photon.Realtime;
using GravityRush.BootSystem;
using GravityRush.ServiceLocatorSystem;
using System;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour, IBootAction
{
    private NetworkRunner NetworkRunner => ServiceLocator.Instance.GetService<NetworkRunner>();
    CancellationTokenSource _cancellationTokenSource;

    private string _region;
    public string Region => _region;
    public async UniTask<BootMessage> BootActionInit()
    {
        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await ServiceLocator.Instance.RegisterService<NetworkRunner>(addressableKey: "NetworkRunner", cancellationToken: _cancellationTokenSource.Token, dontDestroyOnLoad: true);
            return new BootMessage();
        }
        catch (System.Exception ex)
        {
            return new BootMessage(ex.Message, nameof(NetworkManager));
        }
    }
    public void SetRegion(string region)
    {
        _region = region;
    }
    private async UniTask<StartGameResult> StartSimulation(string region, GameMode gameMode)
    {
        var appSettings = BuildCustomAppSetting(region);

        // Start the game asynchronously without blocking the main thread
        try
        {
            var result = await NetworkRunner.StartGame(new StartGameArgs()
            {
                SessionName = "Simulation",
                CustomLobbyName = "Simulation",
                GameMode = gameMode,
                CustomPhotonAppSettings = appSettings
            });

            return result;
        }
        catch
        (System.Exception ex)
        {
            Debug.LogError($"Failed to start simulation: {ex.Message}");
            return null;
        }

    }
    private FusionAppSettings BuildCustomAppSetting(string region)
    {
        var appSettings = PhotonAppSettings.Global.AppSettings.GetCopy();

        appSettings.UseNameServer = true;

        if (!string.IsNullOrEmpty(region))
        {
            appSettings.FixedRegion = region.ToLower();
        }

        return appSettings;
    }

    internal async UniTask HostOrJoinGame(GameMode gameMode)
    {
        // Check if the game is already running
        if (NetworkRunner.IsRunning)
        {
            Debug.Log("Game is already running.");
            return;
        }
        try
        {
            StartGameResult result = await StartSimulation(_region, gameMode);

        }
        catch (OperationCanceledException)
        {
            Debug.Log("Game start operation was canceled.");
            return;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start the game: {ex.Message}");
            return;
        }
    }
}
