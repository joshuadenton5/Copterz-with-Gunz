using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float power;
    private bool fire;

    void Start()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (!PlayerController.dead)
        {
            fire = Input.GetButtonDown("Fire2");
            if (fire)
            {
                FireBullet();
            }
            yield return new WaitForSeconds(.01f);
        }
    }

    public void FireBullet()
    {
        GameObject bullet = PoolManager.SharedInstance.GetFromPool(2);
        if (!(bullet is null))
        {
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            AddForce(bullet);
        }
    }

    void AddForce(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right *10000);
    }
}
