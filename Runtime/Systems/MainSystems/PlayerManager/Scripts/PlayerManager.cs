using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using GravityRush.BootSystem;
using GravityRush.ServiceLocatorSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimulationBehaviour, IBootAction, INetworkRunnerCallbacks
{
    protected NetworkRunner NetworkRunner => ServiceLocator.Instance.GetService<NetworkRunner>();
    protected LoadAddressable LoadAddressable => ServiceLocator.Instance.GetService<LoadAddressable>();

    private Player _localPlayer;
    public Player Local;

    private Dictionary<string, Player> _players = new();
    private Player _playerPrefab;
    public async UniTask<BootMessage> BootActionInit()
    {
        try
        {
            // Initialize player-related systems here
            _players.Clear();
            _playerPrefab = await LoadAddressable.LoadAsset<Player>("Player", this.GetCancellationTokenOnDestroy());
            return new BootMessage();
        }
        catch (System.Exception ex)
        {
            return new BootMessage(ex.Message, nameof(PlayerManager));
        }
    }
    private void AddPlayerToList(Player player)
    {
        // Add player to the list
        // This is a placeholder for actual implementation
        if (player == null)
            throw new PlayerManagerException("Player cannot be null", ExceptionType.DeveloperMistake);
        if (_players.ContainsKey(player.name))
            throw new PlayerManagerException($"Player with name {player.name} already exists", ExceptionType.DeveloperMistake);
        _players.Add(player.name, player);
    }
    public Player GetPlayer(string playerName)
    {
        // Get player from the list
        // This is a placeholder for actual implementation
        if (_players.TryGetValue(playerName, out var player))
            return player;
        throw new PlayerManagerException($"Player with name {playerName} not found", ExceptionType.DeveloperMistake);
    }
    private void RemovePlayer(string playerName)
    {
        // Remove player from the list
        // This is a placeholder for actual implementation
        if (!_players.Remove(playerName))
            throw new PlayerManagerException($"Player with name {playerName} not found", ExceptionType.DeveloperMistake);
    }
    public async UniTask SpawnLocalPlayer( Vector3 position)
    {
        _localPlayer = await SpawnPlayer(position, NetworkRunner.LocalPlayer);
    }
    public async UniTask<Player> SpawnPlayer(Vector3 position, PlayerRef? playerRef = null)
    {
        if (_playerPrefab == null)
        {
            throw new PlayerManagerException("Player prefab is not loaded", ExceptionType.DeveloperMistake);
        }

        Player newPlayer;

        newPlayer = NetworkRunner.Spawn(_playerPrefab, position, inputAuthority: playerRef.Value);

        newPlayer.PlayerName = playerRef.Value.ToString();

        AddPlayerToList(newPlayer);

        return newPlayer;
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Handle player joining
        if (player == null)
            throw new PlayerManagerException("PlayerRef cannot be null", ExceptionType.DeveloperMistake);
        SpawnPlayer(Vector3.zero, player).Forget();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Handle player leaving
        // This is a placeholder for actual implementation
        if (player == null)
            throw new PlayerManagerException("PlayerRef cannot be null", ExceptionType.DeveloperMistake);
        var playerName = player.ToString();
        RemovePlayer(playerName);
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
public class PlayerManagerException : Exception
{
    public PlayerManagerException(string message, ExceptionType exceptionType) : base(message)
    {
    }

}
