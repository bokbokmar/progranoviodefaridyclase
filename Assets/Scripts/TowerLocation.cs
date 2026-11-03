using UnityEngine;
using UnityEngine.Events;

public class TowerLocation : MonoBehaviour
{
    public TowerType towerType; 
    public bool spawned = false;
    public GameObject towerReference;
}

public enum TowerType
{
    None, // No tiene torre // 0
    Militia, // Spawns soldier squad // 1
    Rangers, // Fires quickly at incoming enemies // 2
    Mortar, // Fires area attacks at incoming enemies // 3
}
