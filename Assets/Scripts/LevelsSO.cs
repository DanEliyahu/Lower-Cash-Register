using UnityEngine;

[CreateAssetMenu(menuName = "Levels", fileName = "Levels")]
public class LevelsSO : ScriptableObject
{
    public Level[] _levels;
}

[System.Serializable]
public class Level
{
    public int _targetScore;
    public int _numOfProjectilesToSpawn;
    public Sprite _image;
    public Vector2 _projectilesFireRateRange;
    public Vector2 _projectilesSpeedRange;
}
