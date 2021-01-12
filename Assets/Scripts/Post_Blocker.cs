using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post_Blocker : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Transform[] _transforms;
    private Vector3[] _positions;
    private BoxCollider _collider;

    private float gravityAcceleration = -9.81f;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _transforms = GetComponentsInChildren<Transform>();
        _positions = new Vector3[_transforms.Length];

        for (int i = 0; i < _transforms.Length; i++)
        {
            _positions[i] = _transforms[i].localPosition;
        }
    }
    public void Explode()
    {
        _collider.isTrigger = false;
        for(int i = 0; i < _rigidbodies.Length; i++)
        {
            StartCoroutine(ExplosionSequence(_rigidbodies[i], i));
        }
    }
    private IEnumerator GravitySimulation(Rigidbody rb, float gravityScale)
    {
        float timer = 0;
        while (timer < 2f) //garvity will be active for 2 seconds
        {
            Vector3 gravity = gravityAcceleration * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
            timer += Time.fixedDeltaTime;
            yield return null;
        }
    }
    void StartExplosion(Rigidbody rb)
    {
        int randomForce = Random.Range(100, 400);
        int randomExplosionForce = Random.Range(50, 150);
        int randomRadius = Random.Range(30, 70);
        rb.AddForce(Vector3.right * randomForce, ForceMode.VelocityChange);
        rb.AddExplosionForce(randomExplosionForce, transform.position, randomRadius, 0, ForceMode.VelocityChange);
    }

    IEnumerator ExplosionSequence(Rigidbody rb, int positionIndex)
    {
        float gravityScale = Random.Range(2, 12);
        StartExplosion(rb);
        yield return StartCoroutine(GravitySimulation(rb, gravityScale));
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.localPosition = _positions[positionIndex];
        yield return null;
        _collider.isTrigger = true;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cave"))
        {
            Debug.Log("run");
        }
    }
}
