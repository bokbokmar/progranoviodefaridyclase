using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveGroup
{
    public EnemyData enemy;
    public int count = 5;
    public float interval = 1f;          // segundos entre enemigos
    public float delayAfterGroup = 1f;
}

// Una oleada = una lista de grupos de enemigos
[CreateAssetMenu(menuName = "TD/Wave Data")]
public class WaveData : ScriptableObject
{
    public float timeBeforeWave = 3f;
    public List<WaveGroup> groups = new List<WaveGroup>();
}
