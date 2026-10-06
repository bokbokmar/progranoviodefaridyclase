using UnityEngine;

public class Squad : MonoBehaviour
{
    [Header ("Soldier cosos")]
    [SerializeField] private float speed;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damagePerSecond;

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}