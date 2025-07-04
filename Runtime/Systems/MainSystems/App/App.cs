using Cysharp.Threading.Tasks;
using Fusion;
using GravityRush.BootSystem;
using GravityRush.ServiceLocatorSystem;
using GravityRush.UISystem;
using UnityEngine;
public class App : MonoBehaviour, IBootAction
{
    protected LoadAddressable LoadAddressableService => ServiceLocator.Instance.GetService<LoadAddressable>();
    protected NetworkManager NetworkManager=> ServiceLocator.Instance.GetService<NetworkManager>();
    protected UIManager UIManager => ServiceLocator.Instance.GetService<UIManager>();
    protected LevelManager LevelManager => ServiceLocator.Instance.GetService<LevelManager>();
    protected PlayerManager PlayerManager => ServiceLocator.Instance.GetService<PlayerManager>();
    public async UniTask<BootMessage> BootActionInit()
    {
        try
        {
            return new BootMessage();
        }
        catch (System.Exception ex)
        {
            return new BootMessage(ex.Message, nameof(App));
        }
    }

    internal void Quit()
    {
        Application.Quit();
    }

    internal async UniTask StartGame(GameMode gameMode)
    {
        await UIManager.ShowLoading();
        UIManager.HideAll();
        await NetworkManager.HostOrJoinGame(gameMode);
        await PlayerManager.SpawnLocalPlayer(Vector3.zero);
        await LevelManager.StartLevel("Level1");
        await UniTask.Delay(5000);
        UIManager.HideLoading();
    }
}

