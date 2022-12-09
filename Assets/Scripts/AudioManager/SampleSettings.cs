using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SampleSettings : MonoBehaviour
{
    public float Volume;
    public float Pitch;

    public SampleSettings()
    {
        Volume = 1;
        Pitch = 1;
    }
}
