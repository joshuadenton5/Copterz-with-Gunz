using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post_Blocker : MonoBehaviour
{
    private Rigidbody[] childrensRbs;

    private float gravityScale = 0;
    private float gravityAcceleration = -9.81f;

    void Awake()
    {
        childrensRbs = GetComponentsInChildren<Rigidbody>();
    }

    public void Explode()
    {
        for(int i = 0; i < childrensRbs.Length; i++)
        {
            int coinFlip = Random.Range(-1, 1);
            if (coinFlip == -1)
                Explosion_One(childrensRbs[i]);
            else
                Explosion_Two(childrensRbs[i]);
            //childrensRbs[i].transform.SetParent(null);
        }
    }
    private void FixedUpdate()
    {
        if(gravityScale > 0)
        {
            Vector3 gravity = gravityAcceleration * gravityScale * Vector3.up;
            foreach (Rigidbody rb in childrensRbs)
                rb.AddForce(gravity, ForceMode.Acceleration);
            Debug.Log("Test");
        }
    }

    void Explosion_One(Rigidbody rb)
    {
        gravityScale = Random.Range(2, 8);
        rb.AddForce(Vector3.right * 200, ForceMode.VelocityChange);
        rb.AddExplosionForce(100, transform.position, 50, 0, ForceMode.VelocityChange);
    }
    private IEnumerator _Explode()
    {

        yield return null;
    }

    void Explosion_Two(Rigidbody rb)
    {
        gravityScale = Random.Range(2, 8);
        rb.AddExplosionForce(250, transform.position, 50, 0, ForceMode.VelocityChange);
    }
}
