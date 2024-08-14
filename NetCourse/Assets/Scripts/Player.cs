using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private InputReader _inputReader;

    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private Camera _camera;
    
    private Vector2 mousePos;
    
    [SerializeField] private Rigidbody2D rb;
    
    private NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

    [SerializeField] private Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
            _inputReader.shootEvent += SpawnRPC;
            _inputReader.mouseEvent += MousePointRPC;
        }
    }

    private void OnMove(Vector2 input)
    {
        HelloRpc(input);
        //_moveInput.Value += input;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
           
            transform.position += (Vector3)_moveInput.Value * Time.deltaTime;
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SpawnRPC()
    {
        Debug.Log(OwnerClientId + " Hej");
        NetworkObject ob = Instantiate(objectToSpawn, firePoint.position, firePoint.rotation).GetComponent<NetworkObject>();
        Rigidbody2D projectile = ob.GetComponent<Rigidbody2D>();
        projectile.AddForce(firePoint.up * 10f, ForceMode2D.Impulse);
        ob.Spawn();
        
    }

    //Authority with the client and host so both sides that do some actions
    [Rpc(SendTo.ClientsAndHost)]
    private void HelloRpc(Vector2 data)
    {
        _moveInput.Value = data;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void MousePointRPC(Vector2 value)
    {
        mousePos = _camera.ScreenToWorldPoint(value);
        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
