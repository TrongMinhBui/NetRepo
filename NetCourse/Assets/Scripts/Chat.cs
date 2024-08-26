using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Chat : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;


    [SerializeField] private TextMeshProUGUI text;
    //[SerializeField] private float LIFE_TIME = 1f;
    // Start is called before the first frame update
    void Start()
    {
        text.text = string.Empty;
        if (inputReader != null)
        {
            inputReader.emoteEvent += onEmote;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /*
     
    [Rpc(SendTo.Server)]
    private void DestroyEmoteRpc()
    {
        NetworkObject.Despawn(gameObject);
    }
     */

    private void onEmote()
    {
        string message = "Hello World!";
        SendEmoteRpc(message);
        /*
         
         if (IsServer)
         {
             LIFE_TIME -= Time.deltaTime; 
             if (LIFE_TIME <= 0) 
             { 
                 DestroyEmoteRpc();
             }      
         } 
         */
    }

    [Rpc(SendTo.Server)]
    public void SendEmoteRpc(string message)
    {
        updateEmoteRpc(message);
        Debug.Log("Success sent");
    }

    [Rpc(SendTo.Everyone)]
    public void updateEmoteRpc(string message)
    {
        text.text = message.ToString() + " Tehe";
        Debug.Log("Success recieved");
    }
}
