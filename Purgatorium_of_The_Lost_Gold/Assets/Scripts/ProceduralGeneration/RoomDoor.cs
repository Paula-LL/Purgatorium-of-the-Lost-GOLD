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
            Debug.Log("Contacto");
            Collider [] hits = Physics.OverlapSphere(transform.position, 20);
            foreach (Collider c in hits)
            {
                if (c.GetComponent<RoomDoor>() != null && c != this.gameObject.GetComponent<Collider>())
                {
                    Debug.Log("Teletransp");
                    other.transform.position = c.transform.position;
                }
            }
            room.OnEnterRoom();
        }
    }


}
