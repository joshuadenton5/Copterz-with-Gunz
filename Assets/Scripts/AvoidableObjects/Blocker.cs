using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : AvoidableObject
{
    public override void Start()
    {
        base.Start();
        ValueFromKill = 500;
    }

    public override void EndCollision()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public override void PlayerCollision(Collider2D player)
    {
        //game over
        PlayerController _playerController = player.GetComponent<PlayerController>();
        _playerController.OnDeath();
    }

    public override void BulletInteraction(Collider2D bullet)
    {
        transform.parent.gameObject.SetActive(false); 
        bullet.gameObject.SetActive(false);
        _gameController.UpdateScoreVal(ValueFromKill);//adding to score
        _gameController.ProcessTextElement(transform, ValueFromKill);
    }
}
