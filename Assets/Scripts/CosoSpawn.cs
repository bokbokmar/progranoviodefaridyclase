using System.Collections.Generic;
using UnityEngine;
public class CosoSpawn : MonoBehaviour
{
    [SerializeField] private List<Spawn> selectedSpawns;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;
    public void Update()
    {
        if (!Input.GetMouseButtonDown(0)) { return; }
       
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;

            if (Physics.Raycast(ray.origin, ray.direction, out info, maxDistance, layerMask))
            {
                // aqui deberia de poner el coso de set active para prender el primitivo de la torre
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    TryAddSpawn(info.transform.GetComponentInParent<Spawn>());
                }
                else
                {
                    selectedSpawns.Clear();

                    TryAddSpawn(info.transform.GetComponentInParent<Spawn>());
                }

            }

            else
            {
                // aca que no pase nada
                selectedSpawns.Clear();
            }
        }

        // igual que aca no pase nada 
    }
    private void TryAddSpawn(Spawn attemptSpawn)
    {
        if (selectedSpawns.Contains(attemptSpawn)) { return; }

        selectedSpawns.Add(attemptSpawn);
    }
}
