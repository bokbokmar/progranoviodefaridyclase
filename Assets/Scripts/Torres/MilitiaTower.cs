using UnityEngine;

public class MilitiaTower : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject soldier = Instantiate(soldierPrefab, this.transform);
            soldier.transform.position = transform.position;

            Quaternion rotation = Quaternion.AngleAxis(-30 + (i * 30), Vector3.up);
            soldier.GetComponent<AllySoldier>().targetPosition = transform.position + (rotation * transform.forward) * 1.5f;
        }
    }
}
