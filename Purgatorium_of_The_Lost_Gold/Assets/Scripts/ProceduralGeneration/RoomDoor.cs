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
        if (other.tag == "Player" && DungeonGenerator.s.teletransportadorSeguro == true)
        {
            Debug.Log("Contacto");
            Collider [] hits = Physics.OverlapSphere(transform.position, 15);
            foreach (Collider c in hits)
            {
                if (c.GetComponent<RoomDoor>() != null && c != this.GetComponent<Collider>() && c.GetComponent<Collider>().gameObject != this.gameObject)
                {
                    DungeonGenerator.s.teletransportadorSeguro = false;
                    Vector3 posTeletrans = c.transform.position;
                    other.transform.position = posTeletrans;   
                    Debug.Log("Teletransp a " + c.GetComponentInParent<Room>().name + "en posició " + posTeletrans);
                    StartCoroutine(DungeonGenerator.s.fadeInfadeOut());
                    StartCoroutine(teletranspDelay());
                    c.GetComponentInParent<Room>().OnEnterRoom();
                }
            }
           
        }
    }
    IEnumerator teletranspDelay()
    {
        
        yield return new WaitForSecondsRealtime(3f);
        DungeonGenerator.s.teletransportadorSeguro = true; 
    }

}
