using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Vector2 _directionalSpeed = new Vector2(0, 1); //The direction the enemy will move.
    [SerializeField] private float _rotationSpeed = 0f; //When not 0, the enemy will rotate continuously.
    [SerializeField] private bool _lookAtPlayer = false; //If true, the enemy will constantly look at the player.
    [SerializeField] private EnemyExplosion _explosionEffectPrefab; //Explosion effect for when enemy is destroyed.

    public Vector2 DirectionalSpeed
    {
        get { return _directionalSpeed; }
        set { _directionalSpeed = value; }
    }

    private void Update()
    {
        //Move enemy based on directional speed.
        transform.position += new Vector3(_directionalSpeed.x, -_directionalSpeed.y, 0) * Time.deltaTime;

        if (_lookAtPlayer)
        {
            Player player = GameManager.Instance.Player;

            if (player != null)
            {
                //Face the enemy to look at the player.
                Vector3 dir = player.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            //If not set to look at player, rotate the enemy based on rotation speed.
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }

        //Shoot projectiles.
        Shoot();
    }

    private void LateUpdate()
    {
        //Destroy enemy when off screen and moving in the appropriate direction.
        //Directional speed check is used to prevent destroying enemies coming from off screen onto the screen.
        Vector3 pos = transform.position;
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        Vector3 extents = _renderer.bounds.extents;

        if ((pos.x > screenBounds.x + extents.x && _directionalSpeed.x > 0) || //Right
            (pos.y > screenBounds.y + extents.y && _directionalSpeed.y < 0) || //Top
            (pos.x < -screenBounds.x - extents.x && _directionalSpeed.x < 0) || //Left
            (pos.y < -screenBounds.y - extents.y && _directionalSpeed.y > 0)) //Bottom
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage the character when colliding, based on the bullet's parameters.
        if (((1 << collision.gameObject.layer) & _bulletPrefab.CollideWith) != 0)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            if (character != null)
            {
                character.TakeDamage(_bulletPrefab.Damage);
            }
        }
    }

    /// <summary>
    /// Triggers whenever the enemy takes damage.
    /// </summary>
    protected override void OnDamaged()
    {
        //Add to the score and flash the enemy sprite.
        GameManager.Instance.AddScore(100);
        DamagedFlash(1, 0.1f, false);
    }

    /// <summary>
    /// Triggers when the enemy dies.
    /// </summary>
    protected override void OnDeath()
    {
        //When the enemy dies, there is a chance to spawn in a recovery pickup item.
        Item recoveryItem = ItemManager.Instance.Recovery;
        if (Random.value <= recoveryItem.DropRate)
        {
            Instantiate(recoveryItem.Prefab, transform.position, recoveryItem.Prefab.transform.rotation);
        }

        //Play explosion effect if one exists.
        if (_explosionEffectPrefab != null)
        {
            Instantiate(_explosionEffectPrefab, transform.position, _explosionEffectPrefab.transform.rotation);
        }

        GameManager.Instance.AddScore(1000); //Add to the score.
        Destroy(gameObject); //Destroy enemy.
    }
}
