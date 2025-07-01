using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public List<GameObject> figurinesToSpawnList = new List<GameObject>();
    [SerializeField] private float spawnInterval = 0.3f;

    [SerializeField] private float minSpawnX = -2f;
    [SerializeField] private float maxSpawnX = 2f;

    private void Start()
    {
        if (figurinesToSpawnList.Count > 0)
        {
            StartCoroutine(SpawnFigurinesRoutine(figurinesToSpawnList));
        }
    }

    IEnumerator SpawnFigurinesRoutine(List<GameObject> currentFigurines)
    {
        for (int i = 0; i < currentFigurines.Count; i++)
        {
            float randomX = Random.Range(minSpawnX, maxSpawnX);
            Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

            Instantiate(currentFigurines[i], spawnPosition, Quaternion.identity);
            GameManager.Instance?.RegisterSpawnedFigurine();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void TriggerSpawning()
    {
        StopAllCoroutines();

        if (figurinesToSpawnList.Count > 0)
        {
            StartCoroutine(SpawnFigurinesRoutine(figurinesToSpawnList));
        }
    }
}