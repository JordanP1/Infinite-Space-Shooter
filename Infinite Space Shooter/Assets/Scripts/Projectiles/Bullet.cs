using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    private SpriteRenderer _renderer;
    [SerializeField] private LayerMask _collideWith; //Will only collide with this mask.
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed = 20f;

    public LayerMask CollideWith { get { return _collideWith; } }
    public int Damage { get { return _damage; } }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Bullet will move based on it's rotation and speed.
        transform.position += transform.up * _speed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        //Destroy bullet when off screen and moving in the appropriate direction.
        //Speed check to prevent destroying bullets coming from off screen onto the screen.
        Vector3 pos = transform.position;
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        Vector3 extents = _renderer.bounds.extents;

        //Calculate speed vector based on angle.
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 speed = new Vector2(Mathf.Sin(angle) * -1, Mathf.Cos(angle));

        if ((pos.x > screenBounds.x + extents.x && speed.x > 0) || //Right
            (pos.y > screenBounds.y + extents.y && speed.y > 0) || //Top
            (pos.x < -screenBounds.x - extents.x && speed.x < 0) || //Left
            (pos.y < -screenBounds.y - extents.y && speed.y < 0)) //Bottom
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Only collide with layers set in the mask.
        if (((1 << collision.gameObject.layer) & _collideWith) != 0)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            if (character != null)
            {
                bool invincible = character.Invincible; //Store invincible value since taking damage may change it.
                character.TakeDamage(_damage); //Damage the character.

                //Only destroy if the character isn't invincible, otherwise it will pass through.
                if (!invincible)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
