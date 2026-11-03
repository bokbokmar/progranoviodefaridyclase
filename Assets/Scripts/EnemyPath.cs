using UnityEngine;

// Camino pre-establecido por el desarrollador: arrastrar Transforms en orden
public class EnemyPath : MonoBehaviour
{
    public Transform[] waypoints;

    private void OnDrawGizmos()
    {
        if (waypoints == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.3f);
            if (i > 0 && waypoints[i - 1] != null)
            {
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }
    }
}
