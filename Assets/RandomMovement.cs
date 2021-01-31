using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float x = .2f;
    public float y = .2f;

    public float rotZ = 30;

    void Update()
    {

        float sinY = Mathf.Cos(y* Time.time);
        transform.Translate(new Vector3(0,sinY, 0) * Time.deltaTime);
    }
}
