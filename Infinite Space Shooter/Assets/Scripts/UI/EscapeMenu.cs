using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to menu to close with pressing the cancel button.
public class EscapeMenu : MonoBehaviour
{
    private void Update()
    {
        //Close menu when pressing the cancel button.
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
        }
    }
}
