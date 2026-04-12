using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowDanteBase : MonoBehaviour
{
    public Transform PlayerDante;
    public CinemachineVirtualCamera cam;
    public float posX;
    public Vector3 cameraPosition;

    public void Update()
    {
        posX = PlayerDante.transform.position.x;
        cameraPosition = cam.transform.position;
        cameraPosition.x = posX;
        transform.position = cameraPosition;
    }
}
