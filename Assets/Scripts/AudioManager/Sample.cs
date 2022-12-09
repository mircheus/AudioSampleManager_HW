using System;
using UnityEngine;

[Serializable]
public class Sample
{
    // public string ID;
    public AudioClip SampleFile;
    public SampleName Name;
    public float Volume = 1;
    public float Pitch = 1;

    public Sample(SampleName sampleName)
    {
        Name = sampleName;
        // Volume = 1;
        // Pitch = 0;
    }
    
    // public string SampleFileName;
    // public SampleSettings PV_Settings; // Pitch & Volume settings --- workaround field name ---
}
