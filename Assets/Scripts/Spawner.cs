using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    [Range(0f, 1f)]
    public float bombChance;
    public float maxLifeTime, minSpawnTime, maxSpawnTime, minForce, maxForce, minAngle, maxAngle;
    private Collider spawnArea;

    public float BombChance { get => bombChance; set => bombChance = value; }

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2.0f);

        while (enabled)
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            if(Random.value < BombChance)
            {
                prefab = bombPrefab;
            }

            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(minAngle, maxAngle));

            GameObject fruit = Instantiate(prefab, position, rotation);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);

            Destroy(fruit, maxLifeTime);

            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
