using UnityEngine;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private GameObject constructionMenu;
    [SerializeField] private GameObject upgradeMenu;

    [SerializeField] private UnityEvent MilitiaTowerButton;
    [SerializeField] private UnityEvent RangersTowerButton;
    [SerializeField] private UnityEvent MortarTowerButton;
    [SerializeField] private UnityEvent SellTowerButton;

    public void TowerSelectionMenu()
    {
        constructionMenu.SetActive(false);
        upgradeMenu.SetActive(false);

        Vector2 menuPosition = Input.mousePosition;

        if (!selectionManager.selectedTower.spawned)
        {
            constructionMenu.SetActive(true);
            constructionMenu.GetComponent<RectTransform>().position = menuPosition;
        }
        else
        {
            upgradeMenu.SetActive(true);
            upgradeMenu.GetComponent<RectTransform>().position = menuPosition;
        }

    }

    public void InteractedWithUI()
    {
        constructionMenu.SetActive(false);
        upgradeMenu.SetActive(false);
    }


    public void MilitiaTowerSelected()
    {
        MilitiaTowerButton.Invoke();
    }

    public void RangersTowerSelected()
    {
        RangersTowerButton.Invoke();
    }

    public void MortarTowerSelected()
    {
        MortarTowerButton.Invoke();
    }

    public void SellTowerSelected()
    {
        SellTowerButton.Invoke();
    }
}
