using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    Cinemachine.CinemachineBrain brainRef;
    const int defaultCameraOnlinePriority = 12;
    const int defaultCameraOfflinePriority = 10;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;
    [SerializeField] GameObject thisRoom;
    [SerializeField] GameObject spawnEnemyPoints;

    private void Start()
    {
        if (thisRoom.transform.position == new Vector3(0, 20, 0))
        {
            cam.Priority = defaultCameraOnlinePriority;
        }

    }

    Cinemachine.CinemachineBrain GetBrain()
    {
        if (brainRef == null)
        {
            brainRef = FindObjectOfType<Cinemachine.CinemachineBrain>();
        }
        return brainRef;
    }

    public void OnEnterRoom()
    {

        Cinemachine.CinemachineVirtualCamera currentActiveCam = (Cinemachine.CinemachineVirtualCamera)GetBrain().ActiveVirtualCamera;
        if (currentActiveCam != null && currentActiveCam != cam)
        {
            if (currentActiveCam.GetComponentInParent<Room>() != null)
            {
                currentActiveCam.Priority = defaultCameraOfflinePriority;
            }
        }
        cam.Priority = defaultCameraOnlinePriority;
        DungeonGenerator.s.onFinishFadeIn.AddListener(SpawnEnemiesInThisRoom);
        
    }

    void SpawnEnemiesInThisRoom()
    {
        DungeonGenerator.s.SpawmEnemiesInEnterRoom(this);
        DungeonGenerator.s.onFinishFadeIn.RemoveListener(SpawnEnemiesInThisRoom);


    }
    public GameObject returnSpawnEnemiesPoint()
    {
        return spawnEnemyPoints;
    }

    public GameObject GetActualRoom()
    {
        return thisRoom;
    }
    
}
