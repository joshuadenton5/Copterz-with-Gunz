using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [SerializeField] private float speed;
    [SerializeField] private float force;
    [SerializeField] private float gravScale = 12f;
    private bool upwardsForce;

    public static PlayerController _instance;
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravScale;
    }

    public float PlayerSpeed() { float _speed = speed; return _speed; }  //only getting     

    void FixedUpdate()
    {
        upwardsForce = Input.GetButton("Fire1");
        if (upwardsForce)
        {
            rb2D.AddForce(new Vector2(0, force));           
        }
        rb2D.position = Vector2.Lerp(transform.position, transform.position + Vector3.right, Time.fixedDeltaTime * speed);
    }   

    public void OnDeath()
    {
        Debug.Log("Dead");
        gameObject.SetActive(false);
    }
}
