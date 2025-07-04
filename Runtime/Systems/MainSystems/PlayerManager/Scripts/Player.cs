using Fusion;
using GravityRush.ServiceLocatorSystem;
using UnityEngine;

public class Player : NetworkBehaviour
{
    protected NetworkRunner NetworkRunner => ServiceLocator.Instance.GetService<NetworkRunner>();
    // Player properties and methods go here
    // This is a placeholder for actual implementation
    public string PlayerName { get; set; }
    private bool _isSpawned;
    public bool IsSpawned => _isSpawned;
    private NetworkCharacter _networkCharacter;
    public NetworkCharacter NetworkCharacter => _networkCharacter;
    public override void Spawned()
    {
        if (_isSpawned)
            return;

        base.Spawned();
        // This method is called when the server has initialized the player
        _isSpawned = true;
        PlayerName= Object.InputAuthority.ToString();
        this.name = PlayerName;
        this.transform.SetParent(NetworkRunner.transform);

    }
    public void SetNetWorkCharacter(NetworkCharacter networkCharacter)
    {
        if (networkCharacter == null)
        {
            Debug.LogError("NetworkCharacter is null");
            return;
        }
        _networkCharacter = networkCharacter;
    }
}


