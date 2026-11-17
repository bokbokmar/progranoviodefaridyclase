using System.Collections.Generic;
using UnityEngine;

// "Base de datos" de oleadas: un asset con todas las oleadas del nivel
[CreateAssetMenu(menuName = "TD/Wave Database")]
public class WaveDatabase : ScriptableObject
{
    public List<WaveData> waves = new List<WaveData>();
}
