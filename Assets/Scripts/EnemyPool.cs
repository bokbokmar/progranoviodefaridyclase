using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Object pooling de enemigos: un ObjectPool por cada tipo de EnemyData
public class EnemyPool : MonoBehaviour
{
    private readonly Dictionary<EnemyData, ObjectPool<Enemy>> pools = new Dictionary<EnemyData, ObjectPool<Enemy>>();

    private ObjectPool<Enemy> GetPool(EnemyData data)
    {
        if (pools.TryGetValue(data, out ObjectPool<Enemy> existing)) return existing;

        ObjectPool<Enemy> pool = null;
        pool = new ObjectPool<Enemy>(
            createFunc: () =>
            {
                Enemy enemy = Instantiate(data.prefab, transform);
                enemy.SetReleaseAction(pool.Release);
                return enemy;
            },
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 200);

        pools[data] = pool;
        return pool;
    }

    public Enemy Get(EnemyData data)
    {
        return GetPool(data).Get();
    }

    // Crea los enemigos por adelantado para no hacer Instantiate durante la partida
    public void Prewarm(EnemyData data, int amount)
    {
        ObjectPool<Enemy> pool = GetPool(data);
        List<Enemy> temp = new List<Enemy>(amount);

        for (int i = 0; i < amount; i++) temp.Add(pool.Get());
        foreach (Enemy enemy in temp) pool.Release(enemy);
    }
}
