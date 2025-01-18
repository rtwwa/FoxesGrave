using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnFlipAction;
    public event EventHandler OnFlipAngelsState;
    public event EventHandler OnFlipGunslingerCurse;
    public event EventHandler OnFlipLoversBlessing;
    public event EventHandler OnFlipSwordsMan;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.BodyFlip.performed += Flip_performed;
        playerInputActions.Player.AngelsState.performed += AngelsState_performed;
        playerInputActions.Player.GunslingerCurse.performed += GunslingerCurse_performed;
        playerInputActions.Player.LoversBlessing.performed += LoversBlessing_performed;
        playerInputActions.Player.SwordsMan.performed += SwordsMan_performed;
    }

    private void AngelsState_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFlipAngelsState?.Invoke(this, EventArgs.Empty);
    }

    private void GunslingerCurse_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFlipGunslingerCurse?.Invoke(this, EventArgs.Empty);
    }
    private void LoversBlessing_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFlipLoversBlessing?.Invoke(this, EventArgs.Empty);
    }
    private void SwordsMan_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFlipSwordsMan?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void Flip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFlipAction?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Movements.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
