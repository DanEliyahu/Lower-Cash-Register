using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private float _padding = 1.5f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectilesParent;

    public Vector2 FireRateRange { get; set; }
    public Vector2 ProjectileSpeedRange { get; set; }
    public int NumOfProjectilesToSpawn { get; set; }
    
    private float _xMin;
    private float _xMax;
    private float _timeSinceLastFire;
    private float _nextFireTime;
    private bool _isSpawning = false;
    
    void Start()
    {
        SetUpMovementBoundaries();
        _nextFireTime = Random.Range(FireRateRange.x, FireRateRange.y);
    }

    
    void Update()
    {
        if (!_isSpawning || NumOfProjectilesToSpawn <= 0 )
        {
            return;
        }
        _timeSinceLastFire += Time.deltaTime;
        if (_timeSinceLastFire >= _nextFireTime)
        {
            Shoot();
            _timeSinceLastFire = 0;
            _nextFireTime = Random.Range(FireRateRange.x, FireRateRange.y);
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
        var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity, _projectilesParent);
        NumOfProjectilesToSpawn--;
        var projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            var speed = Random.Range(ProjectileSpeedRange.x, ProjectileSpeedRange.y);
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

    public void SetIsSpawning(bool value)
    {
        _isSpawning = value;
    }

    public void DestroyAllProjectiles()
    {
        foreach (Transform projectile in _projectilesParent)
        {
            Destroy(projectile.gameObject);
        }
    }
}
