using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageLabel : MonoBehaviour
{
    [Header("DamageLabels")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Color normalFontColouur = Color.white;
    [SerializeField] private float startClourFadePercent = 0.8f;

    [Header("Animation curve")]
    [SerializeField] private AnimationCurve easeCurve;
    private float displayDuration;

    [Header("Bezier curve Settings")]
    [SerializeField] private Vector2 highPointOfsset;
    [SerializeField] private Vector2 lowPointOffset;
    [SerializeField] private float heigthVariationMax = 150;
    [SerializeField] private float heigthVariationMin = 50;

    private Vector3 _highPointOffsetBasedOnDirection = Vector3.zero;
    private Vector3 _dropPointOffsetBasedOnDirection = Vector3.zero;
    private bool _direction = true;

    [Header("Visualize")]
    [SerializeField] private bool displayGizmos;
    [SerializeField, Range(1, 30)] private int gizmoResolution = 20;
    private Vector3 startPoinForVisualization = Vector3.zero;

    //private SpawnsDamagePopUps _poolManager;
    private Coroutine _moveCoroutine;

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
        {
            return;
        }
        //OrientCurveBasedOnDirections();
        Vector3 start = transform.position;

        if (Application.isPlaying)
        {
            start = startPoinForVisualization;
        }

        var heigthVariation = heigthVariationMax - heigthVariationMin;
        Vector3 highPoint = start + _highPointOffsetBasedOnDirection + new Vector3(0, heigthVariation, 0);
        Vector3 dropPoint = highPoint + _dropPointOffsetBasedOnDirection;

        int colourChangeIndex = (int)(startClourFadePercent * gizmoResolution);
        Gizmos.color = Color.red;

        Vector3 prevPoint = start;

        for(int i = 0;  i <= gizmoResolution; i++)
        {

        }
    }

}
