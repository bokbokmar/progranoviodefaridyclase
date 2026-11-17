using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public TowerLocation selectedTower;

    [SerializeField] private LayerMask towerLayerMask;

    [SerializeField] private UnityEvent clickedOnTower;
    [SerializeField] private UnityEvent clickedOnNothing;      // NUEVO: cierra el menu al hacer click fuera
    [SerializeField] private UnityEvent interactedWithTower;

    // Indice = valor del enum TowerType: [0] None (dejar vacio), [1] Militia, [2] Rangers, [3] Mortar
    public GameObject[] towerPrefabs;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // NUEVO: ignorar clicks que caen sobre botones/paneles de UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f, towerLayerMask))
        {
            // CAMBIO: GetComponentInParent por si el collider esta en un hijo
            TowerLocation location = hitInfo.collider.GetComponentInParent<TowerLocation>();
            if (location == null) return;

            selectedTower = location;
            clickedOnTower.Invoke();
        }
        else
        {
            selectedTower = null;
            clickedOnNothing.Invoke();
        }
    }

    public void SpawnTower(int newInt)
    {
        if (selectedTower == null || selectedTower.spawned) return;   // NUEVO: validaciones
        if (newInt < 1 || newInt >= towerPrefabs.Length) return;

        selectedTower.towerType = (TowerType)newInt;
        selectedTower.spawned = true;

        GameObject tower = Instantiate(towerPrefabs[newInt], selectedTower.transform);
        tower.transform.localPosition = Vector3.zero;
        tower.transform.localRotation = Quaternion.identity;
        selectedTower.towerReference = tower;

        selectedTower.transform.GetChild(0).gameObject.SetActive(false);

        interactedWithTower.Invoke();
    }

    public void SellTower()
    {
        if (selectedTower == null || !selectedTower.spawned) return;   // NUEVO

        selectedTower.towerType = TowerType.None;
        selectedTower.spawned = false;

        selectedTower.transform.GetChild(0).gameObject.SetActive(true);
        Destroy(selectedTower.towerReference);

        interactedWithTower.Invoke();
    }
}
