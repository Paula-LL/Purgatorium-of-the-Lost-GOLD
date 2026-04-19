using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] GameObject panelshowMovementControls;

    private bool isCloseningPanelMovementControls = false;  
   
    
    private void Start()
    {
        StartCoroutine(showControls());
    }

    public IEnumerator showControls()
    {
        yield return new WaitForSeconds(5f);
        panelshowMovementControls.SetActive(true);
    }
    public void desactivarPanelMovementControls()
    {
        panelshowMovementControls.SetActive(false);
        isCloseningPanelMovementControls = true;

    }
}
