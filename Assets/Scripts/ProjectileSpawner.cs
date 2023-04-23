using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private float _padding = 1.5f;
    [SerializeField] private Vector2 _fireRateRange;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Vector2 _projectileSpeedRange;
    
    private float _xMin;
    private float _xMax;
    private float _timeSinceLastFire;
    private float _nextFireTime;
    
    void Start()
    {
        SetUpMovementBoundaries();
        _nextFireTime = Random.Range(_fireRateRange.x, _fireRateRange.y);
    }

    
    void Update()
    {
        _timeSinceLastFire += Time.deltaTime;
        if (_timeSinceLastFire >= _nextFireTime)
        {
            Shoot();
            _timeSinceLastFire = 0;
            _nextFireTime = Random.Range(_fireRateRange.x, _fireRateRange.y);
            Move();
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

    private void Shoot()
    {
        var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        var projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            var speed = Random.Range(_projectileSpeedRange.x, _projectileSpeedRange.y);
            projectileRigidbody.velocity = new Vector2(0, -speed);
        }
    }

    private void Move()
    {
        var newXPos = Random.Range(_xMin, _xMax);
        var transform1 = transform;
        var position = transform1.position;
        position = new Vector3(newXPos, position.y, position.z);
        transform1.position = position;
    }
}
