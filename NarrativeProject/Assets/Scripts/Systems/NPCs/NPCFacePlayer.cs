using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    [SerializeField] PlayerController player;
    Transform trans;
    public float faceDirection,accuracy;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        var aimDirection = new Vector3(player.transform.position.x - trans.position.x,0f, player.transform.position.z - trans.position.z).normalized;
        faceDirection = Vector3.Angle(new Vector3(0, 0, 1), aimDirection);
        if (aimDirection.x < 0)
        {
            faceDirection = faceDirection * -1;
        }
        trans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(trans.rotation.eulerAngles.y, faceDirection, accuracy), 0);
        
    }
}
