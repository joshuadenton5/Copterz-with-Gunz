using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private GameController _gameController;

    [SerializeField] private float speed = 45f;
    [SerializeField] private float force = 1000f;
    [SerializeField] private float gravScale = 12f;

    private bool _upwardsForce;
    public static float _speed;
    public bool isDead { get; set; }

    public static PlayerController _instance;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravScale;
        _instance = this;
        _speed = speed;
    }
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        rb2D.velocity = new Vector2(speed, 0);
    }
    public IEnumerator MovePlayer()
    {
        Vector2 toPos = rb2D.position + Vector2.right * 3;
        Debug.Log(toPos.x);
        while(rb2D.position.x < toPos.x)
        {
            rb2D.position += Vector2.right/10f;
            yield return null;
        }
        rb2D.position = new Vector2(toPos.x, rb2D.position.y);
    }

    void FixedUpdate()
    {
        _upwardsForce = Input.GetButton("Fire1");
        if (_upwardsForce)
        {
            rb2D.AddForce(new Vector2(0, force));           
        }
    }
    public void OnDeath()
    {
        gameObject.SetActive(false);
        _gameController.PlayerIsDead();
        isDead = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PixelSpawn"))
        {
            //call appropiate function
        }
    }
}
