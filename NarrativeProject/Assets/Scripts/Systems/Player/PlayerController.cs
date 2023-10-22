using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private LayerMask roomTriggerLayer;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerLookSpeed;

    private PlayerInputControls playerInputControls;
    private InputAction WASD;
    private InputAction mouseDelta;
    private InputAction mousePosition;

    private RoomSelectionTrigger selectedRoom;
    private bool mapMode = true;
    public bool journalMode = false;
    public bool dialogueMode = false;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

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
        playerInputControls.PlayerControls.Interact.Disable();
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
        if (journalMode)
        {
            return;
        }

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
        if (journalMode)
        {
            return;
        }

        Debug.Log("Interacted");
    }

    public Vector2 GetMovementInput()
    {
        if (mapMode || journalMode || dialogueMode)
        {
            return Vector2.zero;
        }

        return WASD.ReadValue<Vector2>();
    }

    public float GetRotationInput()
    {
        if (mapMode || journalMode || dialogueMode)
        {
            return 0;
        }
        
        return mouseDelta.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        Vector2 currentInput = GetMovementInput();
        float rotationInput = GetRotationInput();

        characterController.Move((transform.right * currentInput.x + transform.forward * currentInput.y) * playerSpeed * Time.deltaTime);
        transform.eulerAngles += new Vector3(0, rotationInput * playerLookSpeed * Time.deltaTime, 0);
    }
}