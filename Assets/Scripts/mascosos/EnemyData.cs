using UnityEngine;

[CreateAssetMenu(menuName = "TD/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Enemy prefab;
    public int health = 100;
    public float speed = 3f;
    public int livesDamage = 1;   // vidas que quita al llegar al final
}
