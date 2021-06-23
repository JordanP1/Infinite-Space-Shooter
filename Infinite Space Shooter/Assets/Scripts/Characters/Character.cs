using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private int _health = 1;
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] private float _shootDelay = 0.1f; //Determines how fast the character can shoot bullets.
    [SerializeField] private AudioSource _shootSound; //Sound effect when shooting
    [SerializeField] private AudioSource _hitSound; //Sound effect when being hit by a bullet.

    private float _nextShot = 0f; //When the character can shoot again.
    private bool _isDead = false;
    private Color _originalColor;
    private Coroutine _flashCoroutine;
    private bool _invincible = false; //Can't take damage if true.

    protected SpriteRenderer _renderer;

    public delegate void HealthChangedHandler(int health);
    public event HealthChangedHandler HealthChanged; //Event for when the character's health changes (damaged or recovers).

    public int MaxHealth { get { return _maxHealth; } }
    public int Health
    {
        get { return _health; }
        set
        {
            //Don't let health exceed max health or fall below 0.
            int current = _health;
            _health = value >= MaxHealth ? MaxHealth : value <= 0 ? 0 : value;

            //Trigger event if health changed.
            if (current != _health)
            {
                HealthChanged?.Invoke(_health);
            }
        }
    }
    public float MovementSpeed { get { return _movementSpeed; } }
    public bool Invincible { get { return _invincible; } }

    protected abstract void OnDamaged(); //Callback for when character takes damage.
    protected abstract void OnDeath(); //Callback for when character dies.

    protected virtual void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
    }

    /// <summary>
    /// Shoot bullets.
    /// </summary>
    protected void Shoot()
    {
        //Shoot within shoot delay.
        if (_nextShot < Time.time)
        {
            Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);

            //Play shooting sound if exists.
            if (_shootSound != null)
            {
                _shootSound.Play();
            }

            _nextShot = Time.time + _shootDelay;
        }
    }

    /// <summary>
    /// Reduce character's health by damage.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    public void TakeDamage(int damage)
    {
        //Do nothing if the character is dead or invincible.
        if (_isDead || _invincible) { return; }

        Health -= damage;

        //Trigger the damaged callback.
        OnDamaged();

        //Handle death when health runs out.
        if (Health <= 0)
        {
            _isDead = true;
            OnDeath();
        }
        else
        {
            //Play hit sound if not dead and if it exists.
            if (_hitSound != null)
            {
                _hitSound.Play();
            }
        }
    }
    
    /// <summary>
    /// Flash the character sprite.
    /// </summary>
    /// <param name="count">Amount of times to flash.</param>
    /// <param name="interval">Delay between flashes.</param>
    /// <param name="setInvincible">Make character invincible during the flashing.</param>
    protected void DamagedFlash(int count, float interval, bool setInvincible)
    {
        //Stop existing coroutine if already playing.
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _invincible = setInvincible;
        _flashCoroutine = StartCoroutine(Flash(count, interval)); //Begin flash.
    }

    /// <summary>
    /// Flash the character based on the count every interval.
    /// </summary>
    /// <param name="count">Number of flashes.</param>
    /// <param name="interval">Delay between each flash.</param>
    /// <returns></returns>
    private IEnumerator Flash(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            _renderer.color = Color.white;
            yield return new WaitForSeconds(interval);
            _renderer.color = _originalColor;
            yield return new WaitForSeconds(interval);
        }

        _invincible = false;
    }
}
