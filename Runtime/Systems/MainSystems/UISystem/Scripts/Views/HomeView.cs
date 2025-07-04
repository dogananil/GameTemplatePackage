using Cysharp.Threading.Tasks;
using GravityRush;
using GravityRush.ServiceLocatorSystem;
using GravityRush.UISystem;
using UnityEngine;
using UnityEngine.UI;
public class HomeViewData: IUIData
{
    public string ViewName { get; set; }
}
public class HomeView : View
{
    protected App App => ServiceLocator.Instance.GetService<App>();
    protected UIManager UIManager => ServiceLocator.Instance.GetService<UIManager>();

    [SerializeField] Button singleplayerButton;
    [SerializeField] Button multiplayerButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    public override void Show(IUIData data)
    {
        base.Show(data);
        if (data is not HomeViewData homeViewData)
            return;
        singleplayerButton.onClick.AddListener(OnSingleplayerButtonClicked);
        multiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        App.Quit();
    }

    private void OnSettingsButtonClicked()
    {
        UIManager.ShowAsync(new SettingsViewData
        {
            ViewName = nameof(SettingsView)
        }).Forget();
    }

    private void OnMultiplayerButtonClicked()
    {
        App.StartGame(Fusion.GameMode.Shared).Forget();
    }

    private void OnSingleplayerButtonClicked()
    {
        App.StartGame(Fusion.GameMode.Single).Forget();
    }
}
