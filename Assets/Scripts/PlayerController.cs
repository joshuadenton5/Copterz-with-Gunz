using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Turret turret;
    [SerializeField] private int speed;
    [SerializeField] private float force;
    [SerializeField] private float gravScale = 12f;
    private bool upwardsForce, fire;
    public static bool dead;

    void Start()
    {

        turret = GetComponentInChildren<Turret>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravScale;
    }

    public int PlayerSpeed() { return speed; }       

    void FixedUpdate()
    {
        upwardsForce = Input.GetButton("Fire1");
        if (upwardsForce)
        {
            rb2D.AddForce(new Vector2(0, force));           
        }
        
        /*Vector2 newVel = rb2D.velocity;
        newVel.x = speed;
        rb2D.velocity = newVel;*/

        rb2D.position = Vector2.Lerp(transform.position, transform.position + Vector3.right, Time.fixedDeltaTime * speed);
    }   

    void OnDeath()
    {
        Debug.Log("Dead");
        dead = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cave"))
        {
            OnDeath();
        }
        else if (collision.CompareTag("Block"))
        {
            OnDeath();
        }
    }
}
