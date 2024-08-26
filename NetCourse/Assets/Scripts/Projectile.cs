using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    //private Player player;
    //private PlayerShoot _playerShoot;
    
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float LIFE_TIME = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * bulletSpeed));
        if (IsServer)
        {

            LIFE_TIME -= Time.deltaTime;
            if (LIFE_TIME <= 0)
            {
             DestroyProjectileRpc();
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void DestroyProjectileRpc()
    {
        NetworkObject.Despawn(gameObject);
        
    }
    
   private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && IsServer)
        {
            NetworkObject.Despawn(gameObject);
        }
        
    }
   
}
