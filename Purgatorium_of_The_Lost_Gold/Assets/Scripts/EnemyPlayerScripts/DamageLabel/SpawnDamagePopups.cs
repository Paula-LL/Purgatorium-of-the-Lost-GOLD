using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SpawnDamagePopups : MonoBehaviour
{
    public static SpawnDamagePopups Instance { get; private set; }

    private ObjectPool<DamageLabel> _damageLabelPopupPool;

    [Header("Damage Label Popup")]
    [SerializeField] private DamageLabel damageLabelPrefab;

    [Header("Display setUp")]
    [Range(0.8f, 1.5f), SerializeField] public float displayLength = 1f;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);

            
        }
        _damageLabelPopupPool = new ObjectPool<DamageLabel>(
                () =>
                {
                    DamageLabel damageLabel = Instantiate(damageLabelPrefab, transform);
                    damageLabel.Inicialize(displayLength, this);
                    return damageLabel;
                },
                    damageLabel => damageLabel.gameObject.SetActive(true),
                    damageLabel => damageLabel.gameObject.SetActive(false)
                 );
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        mainCamera = Camera.main;
    }
    public void DamageDone(float damage, Vector3 postion, bool isCrit)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(postion);
        screenPosition.z = 0;
        bool direction = screenPosition.x < Screen.width * 0.5f;

        SpawnDamagePopup(damage, screenPosition, direction, isCrit);

    }
    public void ContactDone(string text, Vector3 position, bool isCrit)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(position);
        screenPosition.z = 0;
        bool direction = screenPosition.x < Screen.width * 0.5f;

        SpawnTextPopUp(text, screenPosition, direction, isCrit);
    }
    private void SpawnDamagePopup(float damage, Vector3 position, bool direction, bool isCrit)
    {
        DamageLabel damageLabel = _damageLabelPopupPool.Get();
        damageLabel.Display(damage, position,direction, isCrit);  
    }
    public void ReturnDamageLabelToPool(DamageLabel damageLabelId)
    {
        _damageLabelPopupPool.Release(damageLabelId);
    }

    private void SpawnTextPopUp (string text, Vector3 position, bool direction, bool isCrit)
    {
        DamageLabel damageLabel = _damageLabelPopupPool.Get();
        damageLabel.DisplayText(text, position, direction, isCrit);
    }


}
