using Fusion;
using GravityRush.ServiceLocatorSystem;
using GravityRush.UISystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionSelectionView : View
{
    private NetworkManager NetworkManager => ServiceLocator.Instance.GetService<NetworkManager>();

    [SerializeField] TMP_Dropdown _regionsDropdown;
    [SerializeField] Button _chooseRegionButton;

    public override void Show(IUIData data)
    {
        base.Show(data);
        if (data is not RegionSelectionData regionSelectionData)
            return;

        _regionsDropdown.ClearOptions();
        _regionsDropdown.AddOptions(regionSelectionData.Regions.Select(reg => $"{reg.RegionCode} = {reg.RegionPing}").ToList());

        _chooseRegionButton.onClick.AddListener(ChooseRegion);
    }

    private void ChooseRegion()
    {
        NetworkManager.SetRegion(_regionsDropdown.options[_regionsDropdown.value].text.Split('=')[0].Trim());
        Hide();
    }

    public override void Hide()
    {
        base.Hide();
        _regionsDropdown.ClearOptions();
        _chooseRegionButton.onClick.RemoveAllListeners();
    }
}
[System.Serializable]
internal class RegionSelectionData: IUIData
{
    public List<RegionInfo> Regions;
    public string ViewName { get; set; }
}

