using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelsSO _levels;
    [SerializeField] private Image _levelImage;
    [SerializeField] private Image _levelUpImage;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _targetScoreText;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private Player _player;
    [SerializeField] private ProjectileDestroyer _projectileDestroyer;

    private int _currentLevel;
    private int _score = 0;
    private int _targetScore;
    private int _remainingProjectiles;
    
    private void Start()
    {
        _currentLevel = 0;
        SetUpLevel(_levels._levels[_currentLevel]);
        _player.RewardCollected += IncreaseScore;
        _player.WinAnimationEnded += HandleWinAnimationEnded;
        _player.LoseAnimationEnded += Restart;
        _projectileDestroyer.ProjectileDestroyed += MissedProjectile;
    }

    private void OnDisable()
    {
        _player.RewardCollected -= IncreaseScore;
        _player.WinAnimationEnded -= HandleWinAnimationEnded;
        _player.LoseAnimationEnded -= Restart;
        _projectileDestroyer.ProjectileDestroyed -= MissedProjectile;

    }

    public void StartGame()
    {
        _projectileSpawner.SetIsSpawning(true);
    }

    private void IncreaseScore()
    {
        _score++;
        _remainingProjectiles--;
        _scoreText.text = _score.ToString();
        if (_score >= _targetScore)
        {
            _projectileSpawner.DestroyAllProjectiles();
            _projectileSpawner.SetIsSpawning(false);
            _player.SetWinSprite(_currentLevel);
            _player.PlayWinAnimation();
        }
        else if (_remainingProjectiles <= 0)
        {
            Lose();
        }
    }

    private void MissedProjectile()
    {
        _remainingProjectiles--;
        if (_remainingProjectiles <= 0 )
        {
            Lose();
        }
    }

    private void Lose()
    {
        _player.PlayLoseAnimation();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    private void HandleWinAnimationEnded()
    {
        _currentLevel++;
        if (_currentLevel >= _levels._levels.Length)
        {
            return;
        }
        StartCoroutine(DisplayMovingToNextLevelText());
    }

    private IEnumerator DisplayMovingToNextLevelText()
    {
        _levelUpImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _levelUpImage.gameObject.SetActive(false);
        LevelUp();
    }
    
    private void LevelUp()
    {
        _player.BackToDefaultSprite();
        SetUpLevel(_levels._levels[_currentLevel]);
        _projectileSpawner.SetIsSpawning(true);
    }

    private void SetUpLevel(Level level)
    {
        _levelImage.sprite = level._image;
        _targetScore = level._targetScore;
        _targetScoreText.text = _targetScore.ToString();
        _projectileSpawner.FireRateRange = level._projectilesFireRateRange;
        _projectileSpawner.ProjectileSpeedRange = level._projectilesSpeedRange;
        _projectileSpawner.NumOfProjectilesToSpawn = level._numOfProjectilesToSpawn;
        _remainingProjectiles = level._numOfProjectilesToSpawn;
    }
}
