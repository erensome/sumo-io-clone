using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform powerUpParent;
    [SerializeField] GameObject powerUpPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float radius;
    [SerializeField] float powerUpSpawnDelay;
    [SerializeField] int powerUpCount;
    [SerializeField] int enemyCount;
    
    private int _currentPrefabCount;
    
    private void Awake()
    {
        // Implement Singleton
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _currentPrefabCount = powerUpCount;
        SpawnEnemies(enemyCount);
        GameManager.Instance.allWrestlers.Add(GameManager.Instance.playerController);
        SetStartPositionWrestlers(GameManager.Instance.allWrestlers);
        Invoke(nameof(SpawnPowerUpsInCircleShape),3f + powerUpSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Decrease PowerUp count by one and check the
    // count of all PowerUps in the scene.
    public void DecreaseAndCheckPowerUps()
    {
        _currentPrefabCount--;
        if (_currentPrefabCount == 0)
        {
            radius = Random.Range(7f, 15f);
            powerUpCount = Random.Range(3, 10);
            _currentPrefabCount = powerUpCount;
            Invoke(nameof(SpawnPowerUpsInCircleShape), powerUpSpawnDelay);
        }
    }
    
    private void SpawnPowerUpsInCircleShape()
    {
        int angle = 360 / powerUpCount;
        for (int i = 0; i < powerUpCount; i++)
        {
            Vector3 prefabPos = new Vector3((radius * Mathf.Cos(Mathf.Deg2Rad * (angle * i))), 0.5f, (radius * Mathf.Sin(Mathf.Deg2Rad * (angle * i))));
            var powerUp = Instantiate(powerUpPrefab, prefabPos, Quaternion.identity);
            powerUp.transform.parent = powerUpParent.transform;
        }
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.transform.parent = enemyParent.transform;
            GameManager.Instance.allWrestlers.Add(enemy.GetComponent<Wrestler>());
        }
    }

    private void SetStartPositionWrestlers(List<Wrestler> wrestlers)
    {
        int angle = 360 / wrestlers.Count;
        for (int i = 0; i < wrestlers.Count; i++)
        {
            // Wrestlers will be positioned in the circular form
            Vector3 randomPos = new Vector3((radius * Mathf.Cos(Mathf.Deg2Rad * (angle * i))), 0.5f, (radius * Mathf.Sin(Mathf.Deg2Rad * (angle * i))));
            wrestlers[i].transform.position = randomPos;
            // Wrestlers will look origin point in the beginning of the game
            wrestlers[i].transform.rotation = Quaternion.LookRotation(-randomPos, Vector3.up);
        }
    }
}
