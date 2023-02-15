using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HaloController : MonoBehaviour
{
    public float size = 1f;

    public float maxSize = 10f;

    Light _light = null;

    Blink _blink = null;

    private void Start()
    {
        _light = GetComponent<Light>();

        _blink = GetComponent<Blink>();
    }

    private void Update()
    {
        Camera camera = Camera.main;

        float cameraDistance = (transform.position - camera.transform.position).magnitude;

        float _size = 0f;

        if (_light != null)
        {
            _size = (_light.intensity / _blink.maxIntensity) * size * (cameraDistance / 100f);
        }
        else
        {
            _size = size * (cameraDistance / 100f);
        }

        if (_size > maxSize) _size = maxSize;

        if (_size < size) _size = size;

        _light.range = _size;


    }
}