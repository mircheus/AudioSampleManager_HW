using System;
using UnityEngine;

[Serializable]
public class Sample
{
    public AudioClip SampleFile;
    public SampleName Name;
    public float Volume = 1;
    public float Pitch = 1;

    public Sample(SampleName sampleName)
    {
        Name = sampleName;
    }
}
