using GravityRush.UISystem;
using UnityEngine;
public class TestViewData: IUIData
{
    public string ViewName { get; set; }
}
public class TestView : View
{
    public override void Show(IUIData data)
    {
        base.Show(data);
        TestViewData viewData = (TestViewData)data;
        Debug.Log(viewData.ViewName);
    }
}
