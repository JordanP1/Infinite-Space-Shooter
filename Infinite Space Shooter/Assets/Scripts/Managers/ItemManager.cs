using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A manager to reference pickup items to get their prefab and drop rate values.
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private Item _recovery; //Pickup item used to recover health.

    public Item Recovery { get { return _recovery; } }

    private void Awake()
    {
        Instance = this;
    }
}

[System.Serializable]
public class Item
{
    [SerializeField] private Pickup _prefab;
    [SerializeField] private float _dropRate = 0.1f;
    [SerializeField] private AudioSource _pickupSound;

    public Pickup Prefab { get { return _prefab; } }
    public float DropRate { get { return _dropRate; } }
    public AudioSource PickupSound { get { return _pickupSound; } }
}
