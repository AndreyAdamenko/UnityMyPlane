using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSoundController : MonoBehaviour
{
    WheelsController wheelsController = null;
    
    AudioSource audio = null;

    Rigidbody _rb = null;
    
    // Start is called before the first frame update
    void Start()
    {
        wheelsController = GetComponent<WheelsController>();

        audio = GetComponent<AudioSource>();

        _rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audio == null) return;

        if (!wheelsController.wheelRolling)
        {
            if (audio.isPlaying) audio.Stop();
        }
        else
        {
            if (!audio.isPlaying) audio.Play();
        }

        float highSpeed = 5f;

        audio.pitch = (_rb.velocity.magnitude / highSpeed) * 0.5f;

        audio.volume = _rb.velocity.magnitude / highSpeed;
    }
}
