using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITargeter 
{
    public LayerMask TargetLayerMask { get; set; }
    public float DetectionRadius { get; set; }
    public ITargetable CurrentTarget { get; set; }
    public Transform CurrentTransform { get; set; }
    public Coroutine AttackCoroutine { get; set; }
    public int Damage { get; set; }

    public virtual void DetectEnemies()
    {
        List<Collider> colliders = Physics.OverlapSphere(CurrentTransform.position, DetectionRadius, TargetLayerMask).ToList();
        float distance = 1000f;

        if(colliders.Count == 0)
        {
            CurrentTarget = null;
        }

        foreach (Collider collider in colliders)
        {
            if (Vector3.Distance(CurrentTransform.position, collider.transform.position) < distance)
            {
                CurrentTarget = collider.GetComponent<ITargetable>();
                distance = Vector3.Distance(CurrentTransform.position, collider.transform.position);
            }
        }

        if (CurrentTarget != null && AttackCoroutine == null)
        {
            AttackEnemies();
        }
    }

    public abstract void AttackEnemies();


}
