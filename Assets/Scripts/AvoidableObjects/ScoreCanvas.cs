using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCanvas : AvoidableObject
{
    public override void Start()
    {
        base.Start();
    }

    public override void EndCollision()
    {
        transform.gameObject.SetActive(false);
    }

    public override void PlayerCollision(Collider2D col)
    {
    }
}
