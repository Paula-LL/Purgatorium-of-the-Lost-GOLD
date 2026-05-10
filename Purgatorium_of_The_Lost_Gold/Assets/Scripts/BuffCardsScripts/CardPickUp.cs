using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickUp : MonoBehaviour
{
    public Animation anim;
    private Camera camera;
 
    private void Update()
    {
        camera = Camera.main;   
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.LookAt(camera.transform.position);
            transform.position = camera.transform.position - new Vector3(0,0, 2);
            transform.position += new Vector3(0, -2, 0);
            transform.rotation = Quaternion.Euler(camera.transform.rotation.x + 90, camera.transform.rotation.y + 180, camera.transform.rotation.z);
            
            
            Debug.Log("La carta se ha tocado");
            anim.Play();
           
        }
    }
}