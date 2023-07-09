using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] float powerUpSpawnDelay;
    [SerializeField] int powerUpCount;
    [SerializeField] int enemyCount;
    private float _radius;
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
        _radius = Random.Range(3f, GameManager.Instance.gameArenaRadius);
        SpawnEnemies(enemyCount);
        GameManager.Instance.allWrestlers = ShuffleWrestlerOrders(GameManager.Instance.allWrestlers);
        SetStartPositionWrestlers(GameManager.Instance.allWrestlers);
        Invoke(nameof(SpawnPowerUpsInCircleShape),3f + powerUpSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Decrease PowerUp count by one and check the
    // number of all PowerUps in the scene.
    // If the number is zero then spawn more PowerUps
    public void DecreaseAndCheckPowerUps()
    {
        _currentPrefabCount--;
        if (_currentPrefabCount == 0)
        {
            _radius = Random.Range(3f, GameManager.Instance.gameArenaRadius);
            powerUpCount = Random.Range(5, 10);
            _currentPrefabCount = powerUpCount;
            Invoke(nameof(SpawnPowerUpsInCircleShape), powerUpSpawnDelay);
        }
    }
    
    // Shuffles Wrestlers List, it provides randomness in the player's position (and the other wrestlers')
    private List<Wrestler> ShuffleWrestlerOrders(List<Wrestler> wrestlers)
    {
        System.Random random = new System.Random();
        return wrestlers.OrderBy(x => random.Next()).ToList();
    }
    
    private void SpawnPowerUpsInCircleShape()
    {
        int angle = 360 / powerUpCount;
        for (int i = 0; i < powerUpCount; i++)
        {
            // Wrestlers will be spawned in the circular form
            Vector3 prefabPos = new Vector3((_radius * Mathf.Cos(Mathf.Deg2Rad * (angle * i))), 0.5f, (_radius * Mathf.Sin(Mathf.Deg2Rad * (angle * i))));
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
            Vector3 randomPos = new Vector3((_radius * Mathf.Cos(Mathf.Deg2Rad * (angle * i))), 0.5f, (_radius * Mathf.Sin(Mathf.Deg2Rad * (angle * i))));
            wrestlers[i].transform.position = randomPos;
            // Wrestlers will look origin point in the beginning of the game
            wrestlers[i].transform.rotation = Quaternion.LookRotation(-randomPos, Vector3.up);
        }
    }
}
