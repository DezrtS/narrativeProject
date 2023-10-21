using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private LayerMask roomTriggerLayer;

    private PlayerInputControls playerInputControls;
    private InputAction WASD;
    private InputAction mouseDelta;
    private InputAction mousePosition;

    private RoomSelectionTrigger selectedRoom;
    private bool mapMode = true;

    private void OnEnable()
    {
        playerInputControls ??= new PlayerInputControls();

        EnableMapControls();

        // Convert this to UI Controls
        playerInputControls.PlayerControls.EnterRoomSelection.performed += OnMapModeSwitch;
        playerInputControls.PlayerControls.EnterRoomSelection.Enable();
    }

    private void OnDisable()
    {
        if (mapMode)
        {
            DisableMapControls();
        } 
        else
        {
            DisablePlayerControls();
        }

        // Convert this to UI Controls
        playerInputControls.PlayerControls.EnterRoomSelection.performed -= OnMapModeSwitch;
        playerInputControls.PlayerControls.EnterRoomSelection.Disable();
    }

    private void DisableMapControls()
    {
        mousePosition = playerInputControls.RoomSelectionControls.Mouse;
        mousePosition.Disable();

        playerInputControls.RoomSelectionControls.Select.performed -= OnRoomSelect;
        playerInputControls.RoomSelectionControls.Select.Disable();
    }

    private void EnableMapControls()
    {
        mousePosition = playerInputControls.RoomSelectionControls.Mouse;
        mousePosition.Enable();

        playerInputControls.RoomSelectionControls.Select.performed += OnRoomSelect;
        playerInputControls.RoomSelectionControls.Select.Enable();
    }

    private void DisablePlayerControls()
    {
        WASD.Disable();
        mouseDelta.Disable();

        playerInputControls.PlayerControls.Interact.performed -= OnInteract;
        playerInputControls.PlayerControls.EnterRoomSelection.Disable();
    }

    private void EnablePlayerControls()
    {
        WASD = playerInputControls.PlayerControls.Movement;
        WASD.Enable();

        mouseDelta = playerInputControls.PlayerControls.Look;
        mouseDelta.Enable();

        playerInputControls.PlayerControls.Interact.performed += OnInteract;
        playerInputControls.PlayerControls.Interact.Enable();
    }

    public void OnRoomSelect(InputAction.CallbackContext obj)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition.ReadValue<Vector2>());

        if (Physics.Raycast(mouseRay, out RaycastHit hit, 50f, roomTriggerLayer, QueryTriggerInteraction.Ignore))
        {
            selectedRoom = hit.transform.GetComponent<RoomSelectionTrigger>();

            Debug.Log($"Selecting Room {selectedRoom.RoomID} and spawning player in position {selectedRoom.PlayerSpawnPosition}");
        }
    }

    public void OnMapModeSwitch(InputAction.CallbackContext obj)
    {
        Debug.Log("Switching Modes");

        if (mapMode)
        {
            mapMode = false;
            DisableMapControls();
            EnablePlayerControls();
        } 
        else
        {
            mapMode = true;
            DisablePlayerControls();
            EnableMapControls();
        }
    }

    public void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacted");
    }
}