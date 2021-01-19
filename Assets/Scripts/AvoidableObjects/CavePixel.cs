using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavePixel : AvoidableObject
{
    public override void Start()
    {
        base.Start();
    }

    public override void EndCollision()
    {
        gameObject.SetActive(false);
    }

    public override void PlayerCollision(Collider2D player)
    {
        base.PlayerCollision(player);
    }

    public override void BulletInteraction(Collider2D bullet)
    {
        bullet.gameObject.SetActive(false);
    }   
}
