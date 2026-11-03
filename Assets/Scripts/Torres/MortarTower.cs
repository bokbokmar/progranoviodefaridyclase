using System.Collections;
using UnityEngine;

// Torre AOE: lanza bombas en arco que explotan haciendo dano en area
public class MortarTower : MonoBehaviour, ITargeter
{
    [field: SerializeField] public LayerMask TargetLayerMask { get; set; }
    [field: SerializeField] public float DetectionRadius { get; set; }
    public ITargetable CurrentTarget { get; set; }
    public Transform CurrentTransform { get; set; }
    public Coroutine AttackCoroutine { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    [Header("Mortar")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float bombSpeed = 8f;
    [SerializeField] private float arcHeight = 4f;
    [SerializeField] private float attackCooldown = 2f;

    private static readonly Collider[] buffer = new Collider[64];
    private GameObject currentBomb;
    private float nextScan;

    private void Awake()
    {
        CurrentTransform = transform;
    }

    private void Update()
    {
        // Buscar enemigos 5 veces por segundo en lugar de cada frame
        if (Time.time < nextScan) return;
        nextScan = Time.time + 0.2f;

        ((ITargeter)this).DetectEnemies();
    }

    public void AttackEnemies()
    {
        AttackCoroutine = StartCoroutine(MortarAttack());
    }

    private IEnumerator MortarAttack()
    {
        ITargetable target = CurrentTarget;

        Vector3 start = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        Vector3 end = target.CurrentTransform.position;

        currentBomb = Instantiate(bombPrefab, start, Quaternion.identity);

        float duration = Mathf.Max(0.3f, Vector3.Distance(start, end) / bombSpeed);
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;

            // La bomba sigue al enemigo mientras siga vivo; si murio, cae donde estaba
            if (target.CurrentTransform.gameObject.activeInHierarchy)
            {
                end = target.CurrentTransform.position;
            }

            Vector3 position = Vector3.Lerp(start, end, progress);
            position.y += Mathf.Sin(Mathf.Clamp01(progress) * Mathf.PI) * arcHeight;
            currentBomb.transform.position = position;

            yield return null;
        }

        Explode(end);

        Destroy(currentBomb);
        currentBomb = null;

        yield return new WaitForSeconds(attackCooldown);
        AttackCoroutine = null;
    }

    private void Explode(Vector3 center)
    {
        int count = Physics.OverlapSphereNonAlloc(center, explosionRadius, buffer, TargetLayerMask);

        for (int i = 0; i < count; i++)
        {
            ITargetable victim = buffer[i].GetComponent<ITargetable>();
            if (victim != null) victim.Damage(Damage);
        }
    }

    // Si la torre se vende con una bomba en el aire, destruir la bomba
    private void OnDestroy()
    {
        if (currentBomb != null) Destroy(currentBomb);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
