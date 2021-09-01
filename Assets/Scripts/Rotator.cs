using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 5;

    private void Start()
    {
        speed *= 60;
    }

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}