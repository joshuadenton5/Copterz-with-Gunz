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
        bullet.gameObject.SetActive(false);
        _gameController.UpdateScoreVal(ValueFromKill);//adding to score
        ProcessTextElement(transform, ValueFromKill);

        GetPostBlocker(transform);
        transform.parent.gameObject.SetActive(false);
    }

    public void ProcessTextElement(Transform t, float val)
    {
        GameObject _scoreCanvas = GetCanvasFromPool(t);
        UnityEngine.UI.Text text = _scoreCanvas?.GetComponentInChildren<UnityEngine.UI.Text>();
        text.text = val.ToString();
        UIManager.FadeTextPopUp(text, .5f);
    }

    GameObject GetCanvasFromPool(Transform t)
    {
        GameObject scoreCanvas = PoolManager._instance.GetFromPool(3);
        scoreCanvas.transform.position = t.position;
        scoreCanvas.SetActive(true);
        return scoreCanvas;
    }

    void GetPostBlocker(Transform t)
    {
        GameObject postBlocker = PoolManager._instance.GetFromPool(4);
        postBlocker.transform.position = t.position;
        postBlocker.SetActive(true);
        postBlocker.GetComponent<Post_Blocker>().Explode();
    }
}
