using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private InputReader _inputReader;

    //[SerializeField] private GameObject objectToSpawn;

    [SerializeField] private Camera _camera;
    
    private Vector2 mousePos;
    
    [SerializeField] private Rigidbody2D rb;
    
    private NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> _health = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone);
    //[SerializeField] private Transform firePoint;

    //[SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float playerSpeed = 10f;
    
    [SerializeField] public int maxHealth = 10;
    // Start is called before the first frame update
    void Start()
    {
        if(IsServer)
            _health.Value = maxHealth;
            
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
            //_inputReader.shootEvent += SpawnRPC;
            _inputReader.mouseEvent += MousePointRPC;
        }
    }

    private void OnMove(Vector2 input)
    {
        MovementRpc(input);
        //_moveInput.Value += input;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
           
            transform.position += (Vector3)_moveInput.Value * (playerSpeed * Time.deltaTime);
        }
    }

  

    //Authority with the server
    [Rpc(SendTo.ClientsAndHost)]
    private void MovementRpc(Vector2 data)
    {
        _moveInput.Value = data;
    }

    [Rpc(SendTo.Server)]
    private void MousePointRPC(Vector2 value)
    {
        mousePos = _camera.ScreenToWorldPoint(value);
        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        
        rb.rotation = angle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsServer && other.gameObject.CompareTag("Bullet"))
        {
            _health.Value -= 2f;
            if (_health.Value <= 0)
            {
                NetworkObject.Despawn(gameObject);
            }
        }
        throw new NotImplementedException();
    }
}
