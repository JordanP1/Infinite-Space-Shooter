using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : Character
{
    [SerializeField] private ParticleSystem _explosionEffect; //Explosion effect for when the player dies.
    [SerializeField] private AudioSource _explosionSound; //Sound effect for the explosion.

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Health <= 0 || Time.timeScale == 0) { return; } //Stop all input when the player dies or if the game is paused.

        MovementInput(); //Handle directional input.

        //Handle shooting input.
        if (Input.GetButton("Shoot"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        //Keep player within the bounds of the screen.
        Vector3 pos = transform.position;
        Vector3 playerBounds = _renderer.bounds.extents; //Half the bounding box size.
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        float padding = 0.25f; //Padding so the player isn't directly on the edge of the screen

        //Keep the player within the bounds of the screen.
        pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + playerBounds.x + padding, screenBounds.x - playerBounds.x - padding);
        pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + playerBounds.y + padding, screenBounds.y - playerBounds.y - padding);
        transform.position = pos;
    }

    /// <summary>
    /// Handle directional input to move and control the player.
    /// </summary>
    private void MovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    /// <summary>
    /// Move the player based on directional input received in MovementInput().
    /// </summary>
    private void Move()
    {
        _rigidbody.velocity = new Vector2(_moveDirection.x * MovementSpeed, _moveDirection.y * MovementSpeed);
    }

    /// <summary>
    /// A callback for when the player takes damage.
    /// </summary>
    protected override void OnDamaged()
    {
        DamagedFlash(3, 0.1f, true); //Flash the player sprite when damage is taken.
    }

    /// <summary>
    /// A callback for when the player dies.
    /// </summary>
    protected override void OnDeath()
    {
        _moveDirection = Vector3.zero; //Stop player movement.
        StartCoroutine(ExplosionEffect()); //Play the explosion effect which also triggers the game over screen.
    }

    IEnumerator ExplosionEffect()
    {
        _explosionEffect.Play();
        _explosionSound.Play();
        yield return new WaitForSeconds(2f);
        _collider.enabled = false;
        _renderer.enabled = false;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.TriggerGameOver(); //Enable the game over screen.
    }
}
