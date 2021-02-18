using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidableObject : MonoBehaviour
{
    protected GameController _gameController;
    protected float ValueFromKill { get; set; }

    public virtual void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BulletInteraction(collision);
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerCollision(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            EndCollision();
        }       
    }

    public virtual void BulletInteraction(Collider2D col){}

    public virtual void EndCollision(){}

    public virtual void PlayerCollision(Collider2D col)
    {
        PlayerController _playerController = col.GetComponent<PlayerController>();
        _playerController.OnDeath();
    }
}
