using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingUp : MonoBehaviour
{
    public float scaleUpDuration = 1f;
    public float scaleDownDuration = 1f;
    public Vector3 maxScale = new Vector3(1.3f, 1.3f, 1.3f);
    public Vector3 minScale = new Vector3(1f, 1f, 1f);

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        Destroy(gameObject, .5f);
    }

    private void Update()
    {
        transform.position += Vector3.up * 3 * Time.deltaTime;

        float elapsedTime = Time.time - startTime;

        // Scale up
        if (elapsedTime <= scaleUpDuration)
        {
            float t = elapsedTime / scaleUpDuration;
            transform.localScale = Vector3.Lerp(minScale, maxScale, t);
        }
        // Scale down
        else if (elapsedTime <= scaleUpDuration + scaleDownDuration)
        {
            float t = (elapsedTime - scaleUpDuration) / scaleDownDuration;
            transform.localScale = Vector3.Lerp(maxScale, minScale, t);
        }
    }
}