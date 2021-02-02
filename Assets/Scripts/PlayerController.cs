using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField] GameController _gameController;
    [SerializeField] private float speed;
    [SerializeField] private float force;
    [SerializeField] private float gravScale;
    private bool _upwardsForce;
    public static float _speed;

    public static PlayerController _instance;
    private void Awake()
    {
        _instance = this;
        _speed = speed;
    }
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravScale;
        //StartCoroutine(FirstPause());
    }

    void FixedUpdate()
    {
        _upwardsForce = Input.GetButton("Fire1");
        if (_upwardsForce)
        {
            rb2D.AddForce(new Vector2(0, force));           
        }
        rb2D.position = Vector2.Lerp(transform.position, transform.position + Vector3.right, Time.fixedDeltaTime * speed);
    }

    IEnumerator FirstPause()
    {
        rb2D.gravityScale = 0;
        float timer = 2f;
        while(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            yield return null;
        }
        rb2D.gravityScale = 12;
    }

    public Rigidbody2D Rigidbody() { return rb2D; }
    public void OnDeath()
    {
        gameObject.SetActive(false);
        _gameController.PlayerIsDead();
    }
}
