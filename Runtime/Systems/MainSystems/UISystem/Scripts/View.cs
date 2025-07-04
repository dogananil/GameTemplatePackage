using GravityRush.UISystem;
using UnityEngine;
using UnityEngine.UI;
public class View: MonoBehaviour
{
    [SerializeField] Button closeButton;

    public virtual void Init()
    {
        closeButton?.onClick.AddListener(Hide);
    }
    public virtual void Show(IUIData data)
    {
        this.gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
