using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pickup : MonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField]
    private LayerMask _collideWith; //Only allow pickup from certain layers (Player).

    private Vector2 _directionalSpeed = new Vector2(0, 0.5f); //The speed and direction the pickup falls down off the screen.

    public abstract void OnPickup(); //Callback for when item is picked up.

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Move the item across the screen based on directional speed.
        transform.position += new Vector3(_directionalSpeed.x, -_directionalSpeed.y, 0) * Time.deltaTime;
    }

    private void LateUpdate()
    {
        //Destroy the pickup when it falls off screen.
        Vector3 pos = transform.position;
        Vector2 screenBounds = GameManager.Instance.ScreenBounds;
        Vector3 extents = _renderer.bounds.extents;

        if (pos.y < -screenBounds.y - extents.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Trigger pickup and destroy when colliding with the set layer.
        if (((1 << collision.gameObject.layer) & _collideWith) != 0)
        {
            OnPickup();
            Destroy(gameObject);
        }
    }
}
