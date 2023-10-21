using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectionTrigger : MonoBehaviour
{
    [SerializeField] private string roomID;
    [SerializeField] private Vector3 playerSpawnPosition;
    [SerializeField] private Vector3 playerRotation;

    public string RoomID { get { return roomID; } }
    public Vector3 PlayerSpawnPosition { get { return playerSpawnPosition; } }
    public Vector3 PlayerRotation { get { return PlayerRotation; } }
}