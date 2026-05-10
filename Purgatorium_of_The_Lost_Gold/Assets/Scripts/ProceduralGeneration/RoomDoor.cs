using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomDoor : MonoBehaviour
{
    [SerializeField] Room room;
    [SerializeField] 
    private void Reset()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Reset");
        room = GetComponentInParent<Room>();
#endif
    }
    private void Update()
    {
            DungeonGenerator.s.enemigosNulosEnSala = DungeonGenerator.s.CheckEnemiesInRoom(room);
            DungeonGenerator.s.enemigosRestantesEnSala = DungeonGenerator.s.ReturnNumberOfEnemies(room);
            if (DungeonGenerator.s.enemigosNulosEnSala == true )
        {
            
            GetComponent<Collider>().isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && DungeonGenerator.s.teletransportadorSeguro == true && DungeonGenerator.s.enemigosNulosEnSala == true)
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
                    DungeonGenerator.s.cartaInstanciadaEnSala = true;
                   
                }
            }
           
        }
        else if (DungeonGenerator.s.enemigosNulosEnSala == false)
        {
            GetComponent<Collider>().isTrigger = false;
        }
        
    }
    IEnumerator teletranspDelay()
    {
        //Si el jugador pausa a mitad de FadeIn/FadeOut, este WaitForSecondsRealtime probablemente dará problemas (ojo)
        yield return new WaitForSecondsRealtime(3f);
        DungeonGenerator.s.teletransportadorSeguro = true; 
    }

}
