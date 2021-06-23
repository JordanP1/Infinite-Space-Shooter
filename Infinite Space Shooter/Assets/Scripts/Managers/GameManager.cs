using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A game manager to access or handle core game elements.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int HighScore { get; private set; }

    [SerializeField] private Player _player;
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;
    private int _score = 0;
    private int _lastScore = 0;
    [SerializeField] private AudioSource _backgroundMusic;

    public Player Player { get { return _player; } }
    public Vector2 ScreenBounds { get; private set; } //The bounds of the game screen.
    
    //The score is the sum of points earned and the duration the level has been loaded.
    public int Score
    {
        get { return _score + Mathf.FloorToInt(Time.timeSinceLevelLoad * 50); }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Get the screen bounds at the start of the game.
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        PlayBackgroundMusic();
    }

    private void Update()
    {
        //Keep the score updated as the game progresses.
        int score = Score;
        if (score != _lastScore)
        {
            _scoreText.text = string.Format("Score: {0}", score);
            _lastScore = score;
        }
    }

    /// <summary>
    /// Enables the game over screen and pauses the game.
    /// </summary>
    public void TriggerGameOver()
    {
        //Update the highest score for the game session.
        int score = Score;
        if (score > HighScore)
        {
            HighScore = score;
        }

        _highScoreText.text = string.Format("High Score: {0}", HighScore);

        //Stop background music.
        StopBackgroundMusic();

        //Show game over screen.
        _gameOver.gameObject.SetActive(true);
    }

    /// <summary>
    /// Add to the score.
    /// </summary>
    /// <param name="value">Value to add to the score.</param>
    public void AddScore(int value)
    {
        _score += value;
        if (_score < 0) { _score = 0; }
    }

    /// <summary>
    /// Play the background music.
    /// </summary>
    public void PlayBackgroundMusic()
    {
        if (_backgroundMusic != null)
        {
            _backgroundMusic.Play();
        }
    }

    /// <summary>
    /// Stop the background music.
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (_backgroundMusic != null)
        {
            _backgroundMusic.Stop();
        }
    }

    /// <summary>
    /// Pauses the background music.
    /// </summary>
    public void PauseBackgroundMusic()
    {
        _backgroundMusic.Pause();
    }

    /// <summary>
    /// Unpauses background music.
    /// </summary>
    public void UnPauseBackgroundMusic()
    {
        _backgroundMusic.UnPause();
    }
}
