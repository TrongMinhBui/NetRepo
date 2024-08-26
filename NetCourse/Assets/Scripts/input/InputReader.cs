using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{

    GameInput gameInput;

    public event UnityAction<Vector2> MoveEvent = delegate { };

    public event UnityAction shootEvent = delegate { };

    public event UnityAction<Vector2> mouseEvent = delegate { };
    
    public event UnityAction emoteEvent = delegate { };
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
        throw new System.NotImplementedException();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shootEvent.Invoke();
        }
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
       mouseEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnEmote(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            emoteEvent.Invoke();
        }
    }

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.Gameplay.Enable();
        }
    }
}
