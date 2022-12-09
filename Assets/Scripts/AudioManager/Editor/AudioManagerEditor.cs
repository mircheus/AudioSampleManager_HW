using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private const string _enumFile = "SampleName";
    
    private AudioManager _audioManager;
    private string _pathToEnumFile;
    private string _sampleName = "New Sample Name";

    private void OnEnable()
    {
        _audioManager = (AudioManager) target;
        _pathToEnumFile = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(_enumFile)[0]);
    }

    public override void OnInspectorGUI()
    {
        _audioManager.Samples = RefreshSamples(_audioManager.Samples);
        
        foreach (var sample in _audioManager.Samples)
        {
            DrawSampleInfo(sample);
        }

        _sampleName = EditorGUILayout.TextField("Name: ", _sampleName);
        DrawAddSampleButton();
    }
    
    private void DrawSampleInfo(Sample sample)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(sample.Name.ToString());
        
        sample.SampleFile = (AudioClip)EditorGUILayout.ObjectField("Sample File: ", sample.SampleFile, typeof(AudioClip), false);
        sample.Volume = EditorGUILayout.FloatField("Volume: ", sample.Volume);
        sample.Pitch = EditorGUILayout.FloatField("Pitch: ", sample.Pitch);
        
        DrawPlayButton(sample);
        DrawRemoveSampleButton(sample);
        
        EditorGUILayout.EndVertical();
    }

    private List<Sample> RefreshSamples(List<Sample> oldSamples)
    {
        int samplesQuantity = Enum.GetNames(typeof(SampleName)).Length;
        List<Sample> samples = new List<Sample>(samplesQuantity);

        for (int i = 0; i < samplesQuantity; i++)
        {
            SampleName sampleName = (SampleName) i;
            Sample sample = TryRestoreSample(oldSamples, sampleName.ToString());

            if (sample == null)
            {
                sample = CreateNewSample(sampleName);
            }

            samples.Add(sample);
        }

        return samples;
    }

    private Sample TryRestoreSample(List<Sample> oldSamples, string name)
    {
        return oldSamples.FirstOrDefault(s => s.Name.ToString() == name);
    }

    private Sample CreateNewSample(SampleName sampleName)
    {
        Sample sample = new Sample(sampleName);
        return sample;
    }

    private void DrawPlayButton(Sample sample)
    {
        if (GUILayout.Button("Play", GUILayout.Width(40), GUILayout.Height(40)))
        {
            Debug.Log($"{sample.Name.ToString()} played");
            PlaySample(sample);
        }
    }

    private void PlaySample(Sample sampleToPlay)
    {
        AudioSource audioSource = _audioManager.GetComponent<AudioSource>();
        AudioClip audioClip = sampleToPlay.SampleFile;
        audioSource.clip = audioClip;
        audioSource.pitch = sampleToPlay.Pitch;
        audioSource.volume = sampleToPlay.Volume;
        audioSource.Play();
        Debug.Log("End of play sample method");
    }

    private void DrawAddSampleButton()
    {
        if (GUILayout.Button("Add new sample"))
        {
            AddSample();
        }
    }

    private void DrawRemoveSampleButton(Sample sample)
    {
        if (GUILayout.Button("Delete", GUILayout.Width(20), GUILayout.Height(20)))
        {
            RemoveSample(sample);
        }
    }

    private void AddSample()
    {
        if (_sampleName == string.Empty)
            return;

        if (!Regex.IsMatch(_sampleName, @"^[a-zA-Z][a-zA-Z0-9]*$"))
            return;

        Array array = Enum.GetValues(typeof(SampleName));

        if (array.Length != 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (_sampleName == array.GetValue(i).ToString())
                {
                    Debug.LogError("A path with the same name has already been added");
                    return;
                }
            }
        }
        
        EnumEditor.WriteToFile(_sampleName, _pathToEnumFile);
        Refresh();

        _sampleName = string.Empty;
    }

    private void RemoveSample(Sample sample)
    {
        if (!EnumEditor.TryRemoveFromFile(sample.Name.ToString(), _pathToEnumFile))
            return;
        
        Refresh();
    }

    private void Refresh()
    {
        Debug.Log("Please wait");
        var realivePath = _pathToEnumFile.Substring(_pathToEnumFile.IndexOf("Assets"));
        AssetDatabase.ImportAsset(realivePath);
    }
}
