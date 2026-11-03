using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private WaveDatabase database;
    [SerializeField] private EnemyPool pool;
    [SerializeField] private EnemyPath path;

    [SerializeField] private UnityEvent<int> onWaveStarted;
    [SerializeField] private UnityEvent onAllWavesCompleted;

    private void Awake()
    {
        Enemy.AliveCount = 0;
    }

    private IEnumerator Start()
    {
        PrewarmPool();

        for (int w = 0; w < database.waves.Count; w++)
        {
            WaveData wave = database.waves[w];

            yield return new WaitForSeconds(wave.timeBeforeWave);
            onWaveStarted.Invoke(w + 1);

            foreach (WaveGroup group in wave.groups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    Enemy enemy = pool.Get(group.enemy);
                    enemy.Initialize(group.enemy, path.waypoints);
                    yield return new WaitForSeconds(group.interval);
                }

                yield return new WaitForSeconds(group.delayAfterGroup);
            }

            // Esperar a que todos mueran o lleguen al final
            yield return new WaitUntil(() => Enemy.AliveCount <= 0);
        }

        onAllWavesCompleted.Invoke();
    }

    // Prepara el pool con la cantidad que necesita la oleada mas grande
    private void PrewarmPool()
    {
        Dictionary<EnemyData, int> maxPerType = new Dictionary<EnemyData, int>();

        foreach (WaveData wave in database.waves)
        {
            Dictionary<EnemyData, int> inWave = new Dictionary<EnemyData, int>();

            foreach (WaveGroup group in wave.groups)
            {
                inWave.TryGetValue(group.enemy, out int current);
                inWave[group.enemy] = current + group.count;
            }

            foreach (KeyValuePair<EnemyData, int> pair in inWave)
            {
                maxPerType.TryGetValue(pair.Key, out int max);
                if (pair.Value > max) maxPerType[pair.Key] = pair.Value;
            }
        }

        foreach (KeyValuePair<EnemyData, int> pair in maxPerType)
        {
            pool.Prewarm(pair.Key, pair.Value);
        }
    }
}
