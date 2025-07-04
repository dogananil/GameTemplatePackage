using Fusion;
using GravityRush.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class NetworkCharacter : NetworkBehaviour, INetworkCharacterMovement
{
    protected LevelManager LevelManager => ServiceLocator.Instance.GetService<LevelManager>();
    protected PlayerManager PlayerManager => ServiceLocator.Instance.GetService<PlayerManager>();
    public override void Spawned()
    {
        base.Spawned();
        // This method is called when the server has initialized the player
        // You can initialize your character here
        // For example, set the character's name or position
        this.name = $"{Object.InputAuthority.ToString()}_Character";
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(LevelManager.CurrentLevel));
        PlayerManager.GetPlayer(Object.InputAuthority.ToString()).SetNetWorkCharacter(this);
        // Set the character controller
        InitNetworkMovement();

    }
    #region NetworkMovement
    public CharacterController CharacterController { get; set; }
    public float PlayerSpeed { get; set; }
    private void InitNetworkMovement()
    {
        // Initialize the character controller
        CharacterController = GetComponent<CharacterController>();
        if (CharacterController == null)
        {
            Debug.LogError("CharacterController component not found on the GameObject.");
            return;
        }
        // Set the player speed
        PlayerSpeed = 5f; // Set the player speed
    }
    public override void FixedUpdateNetwork()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        CharacterController.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
    #endregion

}
public interface INetworkCharacterMovement
{
    CharacterController CharacterController { get; set; }
    float PlayerSpeed { get; set; }
}
