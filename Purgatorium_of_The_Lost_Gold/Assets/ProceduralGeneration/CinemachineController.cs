using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    public List <GameObject> roomList = new List <GameObject> ();
    public Collider[] roomColliders; 
    public GameObject salaActual;
    public GameObject player;
    public bool playerDetectado = false;
    public CinemachineVirtualCamera virtualCameraAcutal;
    public int incrementalPriority = 12;
    public DungeonGenerator dungeonGenerator;
   

    private void Start()
    {
        foreach (GameObject room in dungeonGenerator._dungeonRoomInstances)
        {
            roomList.Add (room);
        }
        salaActual = roomList[0];
        establecerCameraActualStart();

    }
    public void establecerCameraActualStart()
    {
        virtualCameraAcutal = salaActual.GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCameraAcutal.Priority = incrementalPriority + 1;
    }
}
