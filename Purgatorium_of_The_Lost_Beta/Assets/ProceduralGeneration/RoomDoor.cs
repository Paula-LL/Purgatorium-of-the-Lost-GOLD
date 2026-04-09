using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomDoor : MonoBehaviour
{
    [SerializeField] Room room;
    private void Reset()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Reset");
        room = GetComponentInParent<Room>();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            room.OnEnterRoom();
        }
    }


}
