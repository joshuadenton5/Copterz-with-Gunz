using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float power;

    private GameController _gameController;

    public int AmmoTotal { get; set; }
    private bool fire;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        StartCoroutine(Fire());
    }

    void DecrementAmmo()
    {
        _gameController.UpdateAmmoAmount(1);
    }

    public IEnumerator Fire()
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            fire = Input.GetButtonDown("Fire2");
            if (fire && _gameController.GetCurrentAmmo()>0)
            {
                FireBullet();
                DecrementAmmo();
            }
            yield return new WaitForSeconds(.001f); //slight delay so player can't spam fire button
        }
    }

    public void FireBullet()
    {
        GameObject bullet = PoolManager._instance.GetFromPool(2);
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
        rb.AddForce(Vector2.right * power);
    }
}
