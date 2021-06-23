using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Pause _pause;

    private void Update()
    {
        //Pause the game when pressing the cancel button, but only when the game isn't already paused.
        if (Input.GetButtonDown("Cancel") && Time.timeScale != 0)
        {
            _pause.gameObject.SetActive(true);
        }
    }
}
