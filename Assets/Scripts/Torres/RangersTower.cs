using System.Collections;
using UnityEngine;

// Torre DPS: dispara flechas a un enemigo
public class RangersTower : MonoBehaviour, ITargeter
{
    [field: SerializeField] public LayerMask TargetLayerMask { get; set; }
    [field: SerializeField] public float DetectionRadius { get; set; }
    public ITargetable CurrentTarget { get; set; }
    public Transform CurrentTransform { get; set; }
    public float projectileSpeed;
    public Coroutine AttackCoroutine { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float attackCooldown = 0.5f;   // NUEVO: pausa entre flechas

    private GameObject currentArrow;                         // NUEVO
    private float nextScan;                                  // NUEVO

    private void Awake()
    {
        CurrentTransform = transform;
    }

    private void Update()
    {
        // CAMBIO: escanear 5 veces por segundo, no cada frame
        if (Time.time < nextScan) return;
        nextScan = Time.time + 0.2f;

        ((ITargeter)this).DetectEnemies();
    }

    public void AttackEnemies()
    {
        AttackCoroutine = StartCoroutine(AttackCoroutineMethod());
    }

    public IEnumerator AttackCoroutineMethod()
    {
        ITargetable target = CurrentTarget;
        Vector3 start = transform.position + Vector3.up;

        currentArrow = Instantiate(arrowPrefab, start, Quaternion.identity);
        bool hit = false;

        // CAMBIO: si el enemigo muere (lo mato otra torre) la flecha se cancela
        while (target.CurrentTransform.gameObject.activeInHierarchy)
        {
            Vector3 targetPosition = target.CurrentTransform.position;

            if (Vector3.Distance(currentArrow.transform.position, targetPosition) <= 0.05f)
            {
                hit = true;
                break;
            }

            currentArrow.transform.position = Vector3.MoveTowards(currentArrow.transform.position, targetPosition, projectileSpeed * Time.deltaTime);
            currentArrow.transform.LookAt(targetPosition);

            yield return null;
        }

        if (hit) target.Damage(Damage);

        Destroy(currentArrow);
        currentArrow = null;

        yield return new WaitForSeconds(attackCooldown);
        AttackCoroutine = null;
    }

    // NUEVO: si la torre se vende con una flecha en el aire, destruir la flecha
    private void OnDestroy()
    {
        if (currentArrow != null) Destroy(currentArrow);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
