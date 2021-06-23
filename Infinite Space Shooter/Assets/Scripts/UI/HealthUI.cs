using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI to display the remaining health to the player.
public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarPrefab;

    private List<RectTransform> _healthBars; //Contains individual health bars to display health.

    private void Start()
    {
        //Setup initial health bars on start, and then update via event whenever health changes.
        SetupHealth();

        //Subscribe to the health changed event so we know when to add or remove health bars.
        GameManager.Instance.Player.HealthChanged += Player_HealthChanged;
    }

    /// <summary>
    /// Add or remove health bars whenever player health changes.
    /// </summary>
    /// <param name="health">Player health.</param>
    private void Player_HealthChanged(int health)
    {
        if (health == _healthBars.Count) { return; } //If health wasn't changed, do nothing.

        if (health < _healthBars.Count) //Lost Health.
        {
            //Get the difference in health and remove the amount of health bars based on the difference.
            int diff = _healthBars.Count - health;
            for (int i = 0; i < diff; i++)
            {
                int index = _healthBars.Count - 1;
                RectTransform healthBar = _healthBars[index];
                _healthBars.RemoveAt(index);
                Destroy(healthBar.gameObject);
            }
        }
        else //Gained Health.
        {
            //Get the difference in health and add the amount of health bars based on the difference.
            int diff = health - _healthBars.Count;
            for (int i = 0; i < diff; i++)
            {
                RectTransform healthBar = Instantiate(_healthBarPrefab, transform);
                _healthBars.Add(healthBar); //Keep a reference to each health bar.
            }
        }
    }

    /// <summary>
    /// Initial health bar setup at the start of the scene.
    /// </summary>
    private void SetupHealth()
    {
        Player player = GameManager.Instance.Player;
        int health = player.Health;

        if (_healthBars == null) { _healthBars = new List<RectTransform>(); }

        //Add the amount of health bars based on player health.
        for (int i = 0; i < health; i++)
        {
            RectTransform healthBar = Instantiate(_healthBarPrefab, transform);
            _healthBars.Add(healthBar); //Keep a reference to each health bar.
        }
    }

    private void OnDestroy()
    {
        //Unsubscribe to the event when destroyed (ie: reloading scene after game over).
        GameManager.Instance.Player.HealthChanged -= Player_HealthChanged;
    }
}
