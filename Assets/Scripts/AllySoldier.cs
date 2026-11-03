using System.Collections;
using UnityEngine;

// Soldado de la torre Militia: se queda en su posicion y ataca cuerpo a cuerpo a los enemigos cercanos
public class AllySoldier : MonoBehaviour, ITargeter
{
    public Vector3 targetPosition;   // posicion de guardia (la asigna MilitiaTower)
    [SerializeField] private float movementSpeed = 3f;

    [Header("Melee")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float leashRadius = 4f;   // distancia maxima que se aleja de su posicion

    [field: SerializeField] public LayerMask TargetLayerMask { get; set; }
    [field: SerializeField] public float DetectionRadius { get; set; }
    public ITargetable CurrentTarget { get; set; }
    public Transform CurrentTransform { get; set; }
    public Coroutine AttackCoroutine { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    private float nextScan;

    private void Awake()
    {
        CurrentTransform = transform;
    }

    private void Update()
    {
        if (AttackCoroutine != null) return;   // esta persiguiendo / atacando

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (Time.time >= nextScan)
        {
            nextScan = Time.time + 0.2f;
            ((ITargeter)this).DetectEnemies();
        }
    }

    public void AttackEnemies()
    {
        AttackCoroutine = StartCoroutine(MeleeAttack());
    }

    private IEnumerator MeleeAttack()
    {
        ITargetable target = CurrentTarget;

        while (target.CurrentTransform.gameObject.activeInHierarchy)
        {
            // No alejarse demasiado de la posicion de guardia
            if (Vector3.Distance(transform.position, targetPosition) > leashRadius) break;

            Vector3 enemyPosition = target.CurrentTransform.position;
            enemyPosition.y = transform.position.y;

            if (Vector3.Distance(transform.position, enemyPosition) <= attackRange)
            {
                target.Damage(Damage);
                yield return new WaitForSeconds(attackCooldown);
                break;
            }

            transform.position = Vector3.MoveTowards(transform.position, enemyPosition, movementSpeed * Time.deltaTime);
            transform.LookAt(enemyPosition);

            yield return null;
        }

        AttackCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
