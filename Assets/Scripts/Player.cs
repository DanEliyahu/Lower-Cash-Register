using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _padding = 1f;
    [SerializeField] private float _spriteChangeTime = 1f;
    [SerializeField] private Sprite _collectSprite;
    [SerializeField] private Sprite[] _winSprites;

    public event Action RewardCollected;
    public event Action WinAnimationEnded;
    public event Action LoseAnimationEnded;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _changeSpriteCoroutine;

    private float _xMin;
    private float _xMax;
    private float _movePosition = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();                                                                                                                                                                                                                                                                                                                                                           
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _waitForSeconds = new WaitForSeconds(_spriteChangeTime);
        var rnd = new System.Random();
        _winSprites = _winSprites.OrderBy(x => rnd.Next()).ToArray();
    }


    private void Start()
    {
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
        if (_changeSpriteCoroutine != null)
        {
            StopCoroutine(_changeSpriteCoroutine);
        }
        _changeSpriteCoroutine = StartCoroutine(ChangeSprite());
        RewardCollected?.Invoke();
    }

    private IEnumerator ChangeSprite()
    {
        _spriteRenderer.sprite = _collectSprite;
        yield return _waitForSeconds;
        _spriteRenderer.sprite = _defaultSprite;
    }

    public void SetWinSprite(int level)
    {
        if (_changeSpriteCoroutine != null)
        {
            StopCoroutine(_changeSpriteCoroutine);
        }
        _spriteRenderer.sprite = _winSprites[level];
    }

    public void BackToDefaultSprite()
    {
        _spriteRenderer.sprite = _defaultSprite;
        _animator.Play("Idle");
    }

    public void PlayWinAnimation()
    {
        _animator.enabled = true;
        _animator.Play("Win");
    }

    public void PlayLoseAnimation()
    {
        _animator.enabled = true;
        _animator.Play("Lose");
    }

    public void HandleWinAnimationEnded()
    {
        WinAnimationEnded?.Invoke();
    }

    public void HandleLoseAnimationEnded()
    {
        LoseAnimationEnded?.Invoke();
    }
    
    public void DisableAnimator()
    {
        _animator.enabled = false;
    }
}