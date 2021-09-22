using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

public class MinigameTest : MonoBehaviour
{
    public GameObject left;
    public GameObject up;
    public GameObject down;
    public GameObject right;

    private NoteInfo[] trackInfo;

    private long lookAheadTime;
    private long currentTime;

    private long trackPosition;
    private short ppq;
    private float bpm;
    private float timePerTick;

    private bool isMinigameActive;

    void Start()
    {
        MidiParser.ReadMidiFile();
        trackInfo = MidiParser.GetTrackInfo();
        bpm = MidiParser.bpm;
        ppq = MidiParser.ppq;
        timePerTick = 60000 / (bpm * ppq);
        currentTime = 0;
        isMinigameActive = false;
        BeginMinigame();
    }

    public void BeginMinigame()
    {
        isMinigameActive = true;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            currentTime += (int)(Time.deltaTime / timePerTick);
            Debug.Log(currentTime);
        }
    }
}
