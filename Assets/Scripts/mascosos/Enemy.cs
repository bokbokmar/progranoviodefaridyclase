using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, ITargetable
{
    public static int AliveCount;   // enemigos activos (lo usa WaveSpawner)

    [field: SerializeField] public int Health { get; set; }
    public Transform CurrentTransform { get; set; }

    public Transform[] path;

    private EnemyData data;
    private int waypointIndex;
    private bool alive;
    private Action<Enemy> releaseAction;   // lo asigna el pool

    private void Awake()
    {
        CurrentTransform = transform;

        // Rigidbody kinematico: permite mover el collider sin recalcular la fisica estatica
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void SetReleaseAction(Action<Enemy> action)
    {
        releaseAction = action;
    }

    public void AssignPath(Transform[] newPath)
    {
        path = newPath;
    }

    // Se llama cada vez que el enemigo sale del pool
    public void Initialize(EnemyData newData, Transform[] newPath)
    {
        data = newData;
        Health = data.health;
        AssignPath(newPath);

        transform.position = path[0].position;
        waypointIndex = 1;
        alive = true;
        AliveCount++;
    }

    private void Update()
    {
        if (!alive) return;

        if (waypointIndex >= path.Length)
        {
            ReachEnd();
            return;
        }

        Vector3 target = path[waypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, target, data.speed * Time.deltaTime);

        Vector3 direction = target - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if ((transform.position - target).sqrMagnitude < 0.01f)
        {
            waypointIndex++;
        }
    }

    // Llego al final del camino: baja la variable de vida
    private void ReachEnd()
    {
        if (!alive) return;

        alive = false;
        AliveCount--;
        GameManager.Instance.LoseLife(data.livesDamage);
        releaseAction(this);
    }

    // Lo llama ITargetable.Damage cuando Health <= 0
    public void Die()
    {
        // Evita regresar dos veces al pool (por ejemplo si dos flechas pegan a la vez)
        if (!alive) return;

        alive = false;
        AliveCount--;
        releaseAction(this);
    }
}
