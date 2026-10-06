using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SelectionManager : MonoBehaviour
{
    [SerializeField] private List<Squad> selectedSquads;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private UnityEvent OnSelectedChange;
    public void Update()
    {
        if (!Input.GetMouseButtonDown(0)) { return; }
        // Empieza haciendole click izquierdo
        {
            //empieza si el raycast detecta algo
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;

            if (Physics.Raycast(ray.origin, ray.direction, out info, maxDistance, layerMask))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    TryAddSquad(info.transform.GetComponentInParent<Squad>());
                }
                else
                {
                    selectedSquads.Clear();

                    TryAddSquad(info.transform.GetComponentInParent<Squad>());
                }

            }

            else
            {
                selectedSquads.Clear();
            }
        }

    }

    //alch no se que hace nomas hago caso
    private void TryAddSquad(Squad attemptSquad)
    {
        if (selectedSquads.Contains(attemptSquad)) { return; }

        selectedSquads.Add(attemptSquad);
        OnSelectedChange.Invoke();
    }

    public List<Squad> GetSelectedSquads()
    {
        return new List<Squad>(selectedSquads);
    }
}
