using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private LayerMask roomTriggerLayer;
    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerLookSpeed;
    [SerializeField] private GameObject cameraPivot;

    [SerializeField] private GameObject crosshair;

    private PlayerInputControls playerInputControls;
    private InputAction WASD;
    private InputAction mouseDelta;
    private InputAction mousePosition;

    private RoomSelectionTrigger selectedRoom;
    public bool mapMode = false;
    private bool controlsDisabled = true;
    public bool journalMode = false;
    public bool dialogueMode = false;
    public bool interactableMode = false;

    public bool transitionMode = false;

    private float yRotation = 0;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInputControls ??= new PlayerInputControls();

        //EnableMapControls();

        // Convert this to UI Controls
        //playerInputControls.PlayerControls.EnterRoomSelection.performed += OnMapModeSwitch;
        //playerInputControls.PlayerControls.EnterRoomSelection.Enable();

        mousePosition = playerInputControls.RoomSelectionControls.Mouse;
        mousePosition.Enable();
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

        mousePosition = playerInputControls.RoomSelectionControls.Mouse;
        mousePosition.Disable();

        // Convert this to UI Controls
        //playerInputControls.PlayerControls.EnterRoomSelection.performed -= OnMapModeSwitch;
        //playerInputControls.PlayerControls.EnterRoomSelection.Disable();
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
        if (controlsDisabled)
        {
            return;
        }

        controlsDisabled = true;

        Cursor.lockState = CursorLockMode.None;

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }

        WASD.Disable();
        mouseDelta.Disable();

        playerInputControls.PlayerControls.Interact.performed -= OnInteract;
        playerInputControls.PlayerControls.Interact.Disable();
    }

    private void EnablePlayerControls()
    {
        controlsDisabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);

        WASD = playerInputControls.PlayerControls.Movement;
        WASD.Enable();

        mouseDelta = playerInputControls.PlayerControls.Look;
        mouseDelta.Enable();

        playerInputControls.PlayerControls.Interact.performed += OnInteract;
        playerInputControls.PlayerControls.Interact.Enable();
    }

    public void OnRoomSelect(InputAction.CallbackContext obj)
    {
        if (journalMode || interactableMode || dialogueMode)
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
        if (journalMode || interactableMode || dialogueMode || transitionMode)
        {
            return;
        }

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition.ReadValue<Vector2>());

        if (Physics.Raycast(mouseRay, out RaycastHit hit, 7f, interactableLayer, QueryTriggerInteraction.Ignore))
        {
            InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();

            interactableObject.OnInteract();
        }
    }

    public Vector2 GetMovementInput()
    {
        if (mapMode || journalMode || dialogueMode || interactableMode || controlsDisabled)
        {
            return Vector2.zero;
        }

        return WASD.ReadValue<Vector2>();
    }

    public Vector2 GetRotationInput()
    {
        if (mapMode || journalMode || dialogueMode || interactableMode || controlsDisabled)
        {
            return Vector2.zero;
        }
        
        return mouseDelta.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector2 currentInput = GetMovementInput();
        Vector2 rotationInput = GetRotationInput();

        characterController.Move((transform.right * currentInput.x + transform.forward * currentInput.y) * playerSpeed * Time.deltaTime);
        transform.eulerAngles += new Vector3(0, rotationInput.x * playerLookSpeed * Time.deltaTime, 0);

        yRotation = Mathf.Clamp(yRotation + -rotationInput.y * (playerLookSpeed * 0.8f) * Time.deltaTime, -80, 80);
        cameraPivot.transform.localEulerAngles = new Vector3(yRotation, 0, 0);
    }

    private void FixedUpdate()
    {
        if (!controlsDisabled && (mapMode || journalMode || dialogueMode || interactableMode))
        {
            DisablePlayerControls();
        } 
        else if (controlsDisabled && !(mapMode || journalMode || dialogueMode || interactableMode))
        {
            EnablePlayerControls();
        }
    }
}