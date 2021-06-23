using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Recovery pickup to restore health.
public class Recovery : Pickup
{
    public override void OnPickup()
    {
        //Restores health by 1 when picked up.
        GameManager.Instance.Player.Health += 1;
        //Play recovery pickup sound.
        ItemManager.Instance.Recovery.PickupSound.Play();
    }
}
