using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 0.18f;
    [SerializeField] private float bobbingAmount = 0.2f;
    [SerializeField] private float recoverSpeed = 2f;

    private Vector3 savedPos;
    private Vector3 midpoint;

    void Awake() {
        midpoint = transform.localPosition;
    }

    void Update()
    {
        float mul = Mathf.Sin( Time.timeSinceLevelLoad * bobbingSpeed );
        savedPos = Vector3.Lerp( savedPos, transform.position, Time.deltaTime * recoverSpeed );
        float offset = ( transform.position - savedPos ).magnitude;
        transform.localPosition = midpoint + new Vector3( 0, offset * bobbingAmount * mul );
    }
}