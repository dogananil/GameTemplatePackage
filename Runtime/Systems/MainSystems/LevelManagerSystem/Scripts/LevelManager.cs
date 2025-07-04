using Cysharp.Threading.Tasks;
using Fusion;
using GravityRush.BootSystem;
using GravityRush.ServiceLocatorSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IBootAction
{
    protected LoadAddressable LoadAddressable => ServiceLocator.Instance.GetService<LoadAddressable>();
    protected NetworkRunner NetworkRunner => ServiceLocator.Instance.GetService<NetworkRunner>();
    protected PlayerManager PlayerManager => ServiceLocator.Instance.GetService<PlayerManager>();

    private string _currentLevelName;
    public string CurrentLevel => _currentLevelName;
    private LevelsHolderConfig _levelData;
    private NetworkCharacter _networkCharacterPrefab;
    public async UniTask<BootMessage> BootActionInit()
    {
        try
        {
            _levelData= await LoadAddressable.LoadAsset<LevelsHolderConfig>("LevelsHolderConfig",cancellationToken:this.GetCancellationTokenOnDestroy());
            _networkCharacterPrefab = await LoadAddressable.LoadAsset<NetworkCharacter>("NetworkCharacter", cancellationToken:this.GetCancellationTokenOnDestroy());
            return new BootMessage();
        }
        catch (System.Exception ex)
        {
            return new BootMessage(ex.Message, nameof(LevelManager));
        }
    }
    public async UniTask StartLevel(string levelName)
    {
        var levelConfig = _levelData.GetLevelList().FirstOrDefault(level => level.SceneName == levelName);
        if (levelConfig != null)
        {
            // Check if the scene is already loaded
            if (SceneManager.GetSceneByName(levelConfig.SceneName).isLoaded)
            {
                Debug.LogError($"Level with name {levelName} is already loaded.");
                return;
            }

            await SceneManager.LoadSceneAsync(levelConfig.SceneName, LoadSceneMode.Additive).ToUniTask();
            _currentLevelName = levelName;
            SpawnCharactersInLevel();
        }
        else
        {
            Debug.LogError($"Level with name {levelName} not found in the configuration.");
        }
    }
    private void SpawnCharactersInLevel()
    {
        // Implement character spawning logic here
        // This is a placeholder for actual implementation

        NetworkCharacter localNetworkCharacter=NetworkRunner.Spawn<NetworkCharacter>(prefab: _networkCharacterPrefab, position: Vector3.zero, rotation: Quaternion.identity, inputAuthority: NetworkRunner.LocalPlayer);
    }
}
