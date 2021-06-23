using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Activates with the game over UI.
public class GameOver : MonoBehaviour
{
    private float _originalTimeScale = 1f;

    private void OnEnable()
    {
        //Stop the game when game over screen is displayed.
        _originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        //Continue the game when game over screen is disabled.
        Time.timeScale = _originalTimeScale;
    }

    /// <summary>
    /// Restart the level.
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
