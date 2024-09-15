using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    // [SerializeField] private int amount = 10;
    private Transform _player;
    private Rigidbody2D _rb;
    private CircleCollider2D _physicalCollider;
    [SerializeField] private CircleCollider2D _triggerCollider;
    private bool _isFollowing = false;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
        StartCoroutine(EnableTriggerCollider());
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _physicalCollider = GetComponent<CircleCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isFollowing = true;
        }
    }
    IEnumerator EnableTriggerCollider()
    {
        float timer = 2f;
        yield return new WaitForSeconds(timer);
        _triggerCollider.enabled = true;
    }
    private void Update()
    {
        if (_player != null && _isFollowing)
        {
            FollowPlayer();
        }
    }
    private void FollowPlayer()
    {
        _rb.gravityScale = 0;
        _physicalCollider.enabled = false;
        transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        Destroy(gameObject, .5f);
    }
}
