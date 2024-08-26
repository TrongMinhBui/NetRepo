using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform firePoint;
    
    //private Projectile projectile;
    // Start is called before the first frame update
    void Start()
    {
        if (_inputReader != null && IsLocalPlayer)
        {
            
            _inputReader.shootEvent += SpawnRPC;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    [Rpc(SendTo.Server)]
    private void SpawnRPC()
    {
        Debug.Log(OwnerClientId + " Hej");
        NetworkObject ob = Instantiate(objectToSpawn, firePoint.position, firePoint.rotation).GetComponent<NetworkObject>();
        
        ob.Spawn();
    
    }

    [Rpc(SendTo.Server)]
    public void FireRPC()
    {
        
        //Destroy(objectToSpawn.gameObject);
    }
}
