using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private List<Squad> selectedSquads;
    [SerializeField] private Transform squadCardPanel;
    [SerializeField] private GameObject [] squadCards;
    [SerializeField] private GameObject squadCardPrefab;

    public void VisualizeSquadChange()
    {

        foreach (Squad squad in selectedSquads)
        {
            for (int i = 0; i < squad.transform.childCount; i++)
            {
                if (squad.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    squad.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        for(int i = 0; i < squadCards.Length; i++)
        {
            Destroy(squadCards[i]);
        }

        selectedSquads = selectionManager.GetSelectedSquads();

        foreach (Squad squad in selectedSquads)
        {
            for (int i = 0; i < squad.transform.childCount; i++)
            {
                if(squad.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    squad.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        squadCards = new GameObject[selectedSquads.Count];
        for(int i = 0; i < selectedSquads.Count; i++)
        {
            squadCards[i] = Instantiate(squadCardPrefab, squadCardPanel);
            squadCards[i] = transform.GetChild(0).GetComponent<Image>().fillAmount = selectedSquads[i].GetHealthPercentage();
        }
    }
}
