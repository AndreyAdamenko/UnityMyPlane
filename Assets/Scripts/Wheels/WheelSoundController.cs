using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSoundController : MonoBehaviour
{
    WheelsController wheelsController = null;
    
    AudioSource wheelAudio = null;

    Rigidbody _rb = null;
    
    // Start is called before the first frame update
    void Start()
    {
        wheelsController = GetComponent<WheelsController>();

        wheelAudio = GetComponent<AudioSource>();

        _rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wheelAudio == null) return;

        if (!wheelsController.wheelRolling)
        {
            if (wheelAudio.isPlaying) wheelAudio.Stop();
        }
        else
        {
            if (!wheelAudio.isPlaying) wheelAudio.Play();
        }

        float highSpeed = 5f;

        wheelAudio.pitch = (_rb.velocity.magnitude / highSpeed) * 0.5f;

        wheelAudio.volume = _rb.velocity.magnitude / highSpeed;
    }
}
