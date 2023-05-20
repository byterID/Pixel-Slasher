using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float Hp;
    Warrior war;
    public GameObject Player;
    void Start()
    {
        Player = GameObject.Find("Warrior");
        war = Player.GetComponent<Warrior>();
    }

    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            war.RecountHp(-10);
        }
    }
}
