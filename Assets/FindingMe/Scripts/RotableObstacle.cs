using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableObstacle : MonoBehaviour
{

    [SerializeField]
    private float degreesRotation = 90f;

    [SerializeField]
    private bool clockwiseRotation = true;

    [SerializeField]
    private float delayToNextPhase = 1.0f;

    private bool rotate = true;

    private void Start()
    {
        StartCoroutine(WaitTime(delayToNextPhase));
    }

    private IEnumerator WaitTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!clockwiseRotation)
        {
            transform.Rotate(Vector3.forward, degreesRotation);
        }
        else
        {
            transform.Rotate(Vector3.forward, -degreesRotation);
        }

        StartCoroutine(WaitTime(delayToNextPhase));
    }
}
