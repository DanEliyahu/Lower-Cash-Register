using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _padding = 1f;
    [SerializeField] private float _spriteChangeTime = 1f;
    [SerializeField] private Sprite[] _collectSprites;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;
    private WaitForSeconds _waitForSeconds;
    private GameManager _gameManager;

    private float _xMin;
    private float _xMax;
    private float _movePosition = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _waitForSeconds = new WaitForSeconds(_spriteChangeTime);
    }


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        SetUpMovementBoundaries();
    }

    public void SetMovementDirection(int direction)
    {
        _movePosition = direction;
    }
    
    private void FixedUpdate()
    {
        var deltaX = _movePosition * _movementSpeed * Time.deltaTime;
        var newXPos = Mathf.Clamp(_rb.position.x + deltaX, _xMin, _xMax);
        _rb.MovePosition(new Vector2(newXPos, _rb.position.y));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Reward"))
        {
            CollectReward();
            Destroy(col.gameObject);
        }
    }

    private void SetUpMovementBoundaries()
    {
        var gameCamera = Camera.main;
        if (gameCamera == null)
            return;

        _xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _padding;
        _xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - _padding;
    }

    private void CollectReward()
    {
        _gameManager.IncreaseScore();
        StartCoroutine(ChangeSprite());
    }

    private IEnumerator ChangeSprite()
    {
        var spriteToSwitchTo = _collectSprites[Random.Range(0, _collectSprites.Length)];
        _spriteRenderer.sprite = spriteToSwitchTo;
        yield return _waitForSeconds;
        _spriteRenderer.sprite = _defaultSprite;
    }
}