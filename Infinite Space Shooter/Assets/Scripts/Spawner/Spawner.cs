using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The spawner will create enemies periodically based on criteria given.
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _updateInterval = 0.1f; //An update interval for when to check spawn conditions
                                                           //rather than checking every frame.
    [SerializeField] private float _maxDifficultyTime = 300f; //Difficulty will scale as time lapses, and this is the
                                                              //time in seconds where the game reaches the highest difficulty.
    [SerializeField] private SpawnerEnemy[] _enemies; //Populate this in the editor with enemies that will spawn during the game.

    private float _nextUpdate; //Used with _updateInterval to update in intervals rather than throttle every frame.
    private float[] _nextEnemySpawn; //Internal delay for when a specific enemy is ready to check for another spawn.

    private void Start()
    {
        _nextEnemySpawn = new float[_enemies.Length];
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad >= _nextUpdate)
        {
            //Modifier for increasing the game's difficulty by spawning in more enemies more frequently
            //as time goes on.
            float difficultyMod = Mathf.Clamp(Time.timeSinceLevelLoad / _maxDifficultyTime, 0f, 1f);

            for (int i = 0; i < _enemies.Length; i++)
            {
                if (Time.timeSinceLevelLoad >= _nextEnemySpawn[i])
                {
                    SpawnerEnemy se = _enemies[i];

                    //Frequency is the odds an enemy will spawn during each spawn check cycle.
                    //1 is 100%
                    //0 is 0%
                    //We adjust the frequency based on start and end values multiplied with the
                    //difficulty mod to get increasing odds as the game progresses.
                    float frequency = DifficultyMod(se.FrequencyEnd, se.FrequencyStart, difficultyMod);
                    if (Random.value <= frequency)
                    {
                        SpriteRenderer renderer = se.EnemyPrefab.GetComponent<SpriteRenderer>();
                        Vector3 extents = renderer.bounds.extents;

                        //Spawn randomly based on the desired side of the screen.
                        Vector2 pos = GetRandomSpawnPosition(se.ScreenSpawnPosition, extents);
                        //Face the correct direction based on what side of the screen the enemy approaches from.
                        Quaternion rot = GetSpawnRotation(se.ScreenSpawnPosition);
                        Enemy enemy = Instantiate(se.EnemyPrefab, pos, rot);
                        //Set the direction the enemy will move based on the side of the screen it spawns on.
                        enemy.DirectionalSpeed = GetSpawnDirectionalSpeed(se.ScreenSpawnPosition, enemy.MovementSpeed);
                    }

                    //Set a delay for when the enemy can spawn again, which speeds up as the game progresses.
                    float minSpawnDelay = DifficultyMod(se.MinSpawnDelayEnd, se.MinSpawnDelayStart, difficultyMod);
                    float maxSpawnDelay = DifficultyMod(se.MaxSpawnDelayEnd, se.MaxSpawnDelayStart, difficultyMod);
                    _nextEnemySpawn[i] = Time.timeSinceLevelLoad + Random.Range(minSpawnDelay, maxSpawnDelay);
                }
            }

            _nextUpdate = Time.timeSinceLevelLoad + _updateInterval;
        }
    }

    /// <summary>
    /// Generates a value between the min and max value based on the mod.
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="mod"></param>
    /// <returns></returns>
    private float DifficultyMod(float min, float max, float mod)
    {
        float diff = max - min;
        float diffMod = diff * mod;
        float result = max - diffMod;

        return result;
    }

    /// <summary>
    /// Get a random position to spawn an enemy based on what side of the screen it should start from.
    /// </summary>
    /// <param name="screenPosition">The starting side of the screen.</param>
    /// <param name="extents">The extents bounds of the enemy sprite.</param>
    /// <returns>A random position to spawn at.</returns>
    private Vector2 GetRandomSpawnPosition(ScreenPosition screenPosition, Vector3 extents)
    {
        switch (screenPosition)
        {
            case ScreenPosition.Top:
            default:
                return TopPosition(extents);
            case ScreenPosition.Left:
                return LeftPosition(extents);
            case ScreenPosition.Right:
                return RightPosition(extents);
        }
    }

    /// <summary>
    /// Get the rotation for the enemy based on the side of the screen it will spawn from.
    /// </summary>
    /// <param name="screenPosition">The starting side of the screen.</param>
    /// <returns>The enemy rotation.</returns>
    private Quaternion GetSpawnRotation(ScreenPosition screenPosition)
    {
        switch (screenPosition)
        {
            case ScreenPosition.Top:
            default:
                return Quaternion.AngleAxis(0f, Vector3.forward);
            case ScreenPosition.Left:
                return Quaternion.AngleAxis(90f, Vector3.forward);
            case ScreenPosition.Right:
                return Quaternion.AngleAxis(-90f, Vector3.forward);
        }
    }

    /// <summary>
    /// Get the directional speed based on the side of the screen the enemy starts on.
    /// </summary>
    /// <param name="screenPosition">The starting side of the screen.</param>
    /// <param name="movementSpeed">The enemies movement speed.</param>
    /// <returns>The directional speed for the enemy.</returns>
    private Vector2 GetSpawnDirectionalSpeed(ScreenPosition screenPosition, float movementSpeed)
    {
        switch (screenPosition)
        {
            case ScreenPosition.Top:
            default:
                return new Vector2(0, movementSpeed);
            case ScreenPosition.Left:
                return new Vector2(movementSpeed, 0);
            case ScreenPosition.Right:
                return new Vector2(-movementSpeed, 0);
        }
    }

    /// <summary>
    /// Gets a random spawn position for the top of the screen.
    /// </summary>
    /// <param name="extents">The extents bounds of the enemy sprite.</param>
    /// <returns>A random spawn point at the top of the screen.</returns>
    private Vector2 TopPosition(Vector3 extents)
    {
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        float xPos = Random.Range(-screenBounds.x + extents.x, screenBounds.x - extents.x); //Randomize x position within screen bounds.
        float yPos = screenBounds.y + extents.y;

        return new Vector2(xPos, yPos);
    }

    /// <summary>
    /// Gets a random spawn position for the left side of the screen.
    /// </summary>
    /// <param name="extents">The extents bounds of the enemy sprite.</param>
    /// <returns>A random spawn point at the left side of the screen.</returns>
    private Vector2 LeftPosition(Vector3 extents)
    {
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        float xPos = -screenBounds.x - extents.x;
        float yPos = Random.Range(-screenBounds.y + extents.y, screenBounds.y - extents.y); //Randomize y position within screen bounds.

        return new Vector2(xPos, yPos);
    }

    /// <summary>
    /// Gets a random spawn position for the right side of the screen.
    /// </summary>
    /// <param name="extents">The extents bounds of the enemy sprite.</param>
    /// <returns>A random spawn point at the right side of the screen.</returns>
    private Vector2 RightPosition(Vector3 extents)
    {
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        float xPos = screenBounds.x + extents.x;
        float yPos = Random.Range(-screenBounds.y + extents.y, screenBounds.y - extents.y); //Randomize y position within screen bounds.

        return new Vector2(xPos, yPos);
    }
}
