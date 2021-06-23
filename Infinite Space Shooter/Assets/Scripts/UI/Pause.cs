using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private float _originalTimeScale = 1f;

    private void Update()
    {
        //Close pause menu when pressing cancel button.
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //Save the original time scale when being enabled and set to 0 to pause the game.
        _originalTimeScale = Time.timeScale;
        Time.timeScale = 0;

        //Also pause background music.
        GameManager.Instance.PauseBackgroundMusic();
    }

    private void OnDisable()
    {
        //Restore original time scale.
        Time.timeScale = _originalTimeScale;

        //Resume background music.
        GameManager.Instance.UnPauseBackgroundMusic();
    }
}
