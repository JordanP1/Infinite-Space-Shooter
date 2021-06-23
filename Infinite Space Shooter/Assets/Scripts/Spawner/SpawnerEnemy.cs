using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will be used to determine which side of the screen an enemy will spawn from.
[System.Serializable]
public enum ScreenPosition
{
    Top,
    Left,
    Right
}

//Used to set up spawn conditions for enemies.
[System.Serializable]
public class SpawnerEnemy
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private ScreenPosition _screenSpawnPosition; //Which side of the screen the enemy will spawn at.
    [SerializeField] private float _minSpawnDelayStart = 2f; //Minimum spawn delay at the start of the game.
    [SerializeField] private float _minSpawnDelayEnd = 1f; //Minimum spawn delay at max difficulty as the game progresses.
    [SerializeField] private float _maxSpawnDelayStart = 5f; //Maximum spawn delay at the start of the game.
    [SerializeField] private float _maxSpawnDelayEnd = 2f; //Maximum spawn delay at max difficulty.
    [SerializeField, Range(0, 1)] private float _frequencyStart = 1f; //The chance of spawning at the start of the game.
    [SerializeField, Range(0, 1)] private float _frequencyEnd = 1f; //The chance of spawning at max difficulty.

    public Enemy EnemyPrefab { get { return _enemyPrefab; } }
    public ScreenPosition ScreenSpawnPosition { get { return _screenSpawnPosition; } }
    public float MinSpawnDelayStart { get { return _minSpawnDelayStart; } }
    public float MinSpawnDelayEnd { get { return _minSpawnDelayEnd; } }
    public float MaxSpawnDelayStart { get { return _maxSpawnDelayStart; } }
    public float MaxSpawnDelayEnd { get { return _maxSpawnDelayEnd; } }
    public float FrequencyStart { get { return _frequencyStart; } }
    public float FrequencyEnd { get { return _frequencyEnd; } }
}
