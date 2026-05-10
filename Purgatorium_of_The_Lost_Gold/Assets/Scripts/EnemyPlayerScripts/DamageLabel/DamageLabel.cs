using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageLabel : MonoBehaviour
{
    [Header("DamageLabels")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float normalFontSize = 42;
    [SerializeField] private float critFontSize = 52;
    [SerializeField] private Color normalFontColouur = Color.white;
    [SerializeField] private float startClourFadePercent = 0.8f;

    [Header("Animation curve")]
    [SerializeField] private AnimationCurve easeCurve;
    private float _displayDuration;

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
    private Vector3 _startPoinForVisualization = Vector3.zero;

    private SpawnDamagePopups _poolManager;
    private Coroutine _moveCoroutine;

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
        {
            return;
        }
        OrientCurveBasedOnDirections();
        Vector3 start = transform.position;

        if (Application.isPlaying)
        {
            start = _startPoinForVisualization;
        }

        var heigthVariation = heigthVariationMax - heigthVariationMin;
        Vector3 highPoint = start + _highPointOffsetBasedOnDirection + new Vector3(0, heigthVariation, 0);
        Vector3 dropPoint = highPoint + _dropPointOffsetBasedOnDirection;

        int colourChangeIndex = (int)(startClourFadePercent * gizmoResolution);
        Gizmos.color = Color.red;

        Vector3 prevPoint = start;

        for(int i = 1;  i <= gizmoResolution; i++)
        {
            float time = i / (float) gizmoResolution;
            Vector3 nextPoint = CalculateBezierPoint(time, start, highPoint, dropPoint);

            if (i<= colourChangeIndex)
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;  
        }
    }
    private void OrientCurveBasedOnDirections()
    {
        _highPointOffsetBasedOnDirection = highPointOfsset;
        _dropPointOffsetBasedOnDirection = lowPointOffset;
        if (_direction)
        {
            return;
        }
        _highPointOffsetBasedOnDirection.x = -_highPointOffsetBasedOnDirection.x;
        _dropPointOffsetBasedOnDirection.x = -_dropPointOffsetBasedOnDirection.x;
    }
    private Vector3 CalculateBezierPoint(float progress, Vector3 Start, Vector3 control, Vector3 end)
    {
        float remainingPath = 1-progress;
        Vector3 currentLocation = remainingPath * remainingPath * Start;
        currentLocation += 2 * remainingPath * progress * control;
        currentLocation += progress * progress * end;

        return currentLocation;
    }
    public void Inicialize(float displayDuration, SpawnDamagePopups poolMeneger)
    {
        _poolManager = poolMeneger;
        _displayDuration = displayDuration;

        OrientCurveBasedOnDirections();
    }
    public void Display(float damage, Vector3 objectPosition, bool direction, bool isCrit)
    {
        transform.position = objectPosition;
        _startPoinForVisualization = objectPosition;
        _direction = direction;

        damageText.SetText(damage.ToSafeString());

        damageText.color = normalFontColouur;
        damageText.enableVertexGradient = isCrit;
        damageText.fontSize = isCrit ? critFontSize : normalFontSize;

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(Move());
        StartCoroutine(ReturnDamageLabelToPool(_displayDuration));
    }
    public void DisplayText(string text, Vector3 objectPosition, bool direction, bool isCrit)
    {
        transform.position = objectPosition;
        _startPoinForVisualization = objectPosition;
        _direction = direction;
        
        damageText.SetText(text);

        damageText.color = normalFontColouur;
        damageText.enableVertexGradient = isCrit;
        damageText.fontSize = isCrit ? critFontSize : normalFontSize;

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(Move());
        StartCoroutine(ReturnDamageLabelToPool(_displayDuration));
    }
    private IEnumerator Move()
    {
        float time = 0;
        float fadeStartTime = startClourFadePercent * _displayDuration;

        OrientCurveBasedOnDirections();

        Vector3 start = transform.position;
        var heigthVariation = Random.Range(heigthVariationMin, heigthVariationMax);
        Vector3 variation = new Vector3 (0, heigthVariation, 0);

        Vector3 highPoint = (start + _highPointOffsetBasedOnDirection + variation);
        Vector3 droppoint = highPoint + _dropPointOffsetBasedOnDirection;

        while (time < _displayDuration)
        {
            time += Time.deltaTime;

            float progress = time / _displayDuration;
            float easedTime = easeCurve.Evaluate(progress);

            if (time > fadeStartTime)
            {
                Color color = damageText.color;
                float newAlpha = Mathf.Lerp(1,0, (time - fadeStartTime) / (_displayDuration -fadeStartTime));
                color.a *= newAlpha;    
                damageText.color = color;
            }
            transform.position = CalculateBezierPoint(easedTime, start, highPoint, droppoint);
            yield return null;
        }
    }
    private IEnumerator ReturnDamageLabelToPool(float displayLegth)
    {
        yield return new WaitForSeconds(displayLegth);
        _poolManager.ReturnDamageLabelToPool(this);
    }
}
