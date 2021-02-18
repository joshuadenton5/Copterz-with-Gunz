using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : AvoidableObject //will kill player
{
    public override void Start()
    {
        base.Start();
        ValueFromKill = 500;
        _gameController = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();
    }

    public override void EndCollision()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public override void PlayerCollision(Collider2D player)
    {
        base.PlayerCollision(player);
    }

    public override void BulletInteraction(Collider2D bullet)
    {
        bullet.gameObject.SetActive(false);
        _gameController.UpdateScoreVal(ValueFromKill); //adding to score
        ProcessTextElement(transform, ValueFromKill);

        GameObject _postBlocker = NewObjectFromPool(transform, 4);
        _postBlocker?.GetComponent<Post_Blocker>().Explode();
        transform.parent.gameObject.SetActive(false);
    }

    public void ProcessTextElement(Transform t, float val) //function to show the score flash up
    {
        GameObject _scoreCanvas = NewObjectFromPool(t, 3);
        UnityEngine.UI.Text text = _scoreCanvas?.GetComponentInChildren<UnityEngine.UI.Text>();
        text.text = val.ToString();
        UIManager.FadeTextPopUp(text, .8f);
    }
    GameObject NewObjectFromPool(Transform t, int index)
    {
        GameObject _obj = PoolManager._instance.GetFromPool(index);
        _obj.transform.position = t.position;
        _obj.SetActive(true);
        return _obj;
    }   
}
