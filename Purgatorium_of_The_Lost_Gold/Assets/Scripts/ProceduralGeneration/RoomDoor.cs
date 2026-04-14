using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomDoor : MonoBehaviour
{
    [SerializeField] Room room;
    private bool teletransPortSeguro = true;
    private void Reset()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Reset");
        room = GetComponentInParent<Room>();
#endif
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && teletransPortSeguro == true)
        {
            Debug.Log("Contacto");
            Collider [] hits = Physics.OverlapSphere(transform.position, 15);
            foreach (Collider c in hits)
            {
                if (c.GetComponent<RoomDoor>() != null && c != this.GetComponent<Collider>() && c.GetComponent<Collider>().gameObject != this.gameObject)
                {
                    Vector3 posTeletrans = c.transform.position;
                    posTeletrans.y = other.transform.position.y;
                    other.GetComponent<CharacterController>().Move(posTeletrans);
                    other.transform.position = posTeletrans;
                    Debug.Log("Teletransp a " + c.GetComponentInParent<Room>().name + "en posició " + posTeletrans);
                  
                }
            }
            StartCoroutine(teletranspDelay());
            room.OnEnterRoom();
            StopAllCoroutines();
        }
    }

    IEnumerator teletranspDelay()
    {
        teletransPortSeguro = false;
        yield return new WaitForSecondsRealtime(1f);
        teletransPortSeguro = true; 
    }

}
