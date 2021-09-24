using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Melanchall.DryWetMidi.Interaction;

public class Minigame : MonoBehaviour
{
    public event System.Action<NoteInfo> OnLookAhead;
    public event System.Action<NoteInfo, HitType> OnHit;

    [Tooltip("The path to the MIDI file relative to Assets")]
    public string midiPath;

    private NoteInfo[] trackInfo;

    private CalmciergeControls controls;
    private CalmciergeControls.MinigameActions actions;
    private InputAction inputUp;
    private InputAction inputDown;
    private InputAction inputLeft;
    private InputAction inputRight;
    private InputActionMap actionMap;

    [SerializeField, Tooltip("Offset in number of beats"), Range(0, 8)]
    private int lookAheadBeatOffset = 4;
    private int lookAheadOffset;
    private long lookAheadTick;
    private long currentTick;
    private double realtime;

    private int currentIndex;
    private int lookAheadIndex;

    private short ppq;
    private float bpm;
    private float timePerTick;

    private bool isMinigameActive;
    private bool isInitialized;

    private List<NoteInfo> hitList;


    protected virtual void Awake()
    {
        controls = new CalmciergeControls();
        inputUp = controls.Minigame.Up;
        inputDown = controls.Minigame.Down;
        inputLeft = controls.Minigame.Left;
        inputRight = controls.Minigame.Right;
        actions = controls.Minigame;
        actionMap = actions.Get();
        hitList = new List<NoteInfo>();
    }

    protected virtual void OnEnable()
    {
        actionMap.Enable();
        inputUp.Enable();
        inputDown.Enable();
        inputLeft.Enable();
        inputRight.Enable();
        inputUp.started += InputUp_started;
        inputDown.started += InputDown_started;
        inputLeft.started += InputLeft_started;
        inputRight.started += InputRight_started;
    }

    protected virtual void OnDisable()
    {
        actionMap.Disable();
        inputUp.Disable();
        inputDown.Disable();
        inputLeft.Disable();
        inputRight.Disable();
        inputUp.started -= InputUp_started;
        inputDown.started -= InputDown_started;
        inputLeft.started -= InputLeft_started;
        inputRight.started -= InputRight_started;
    }

    protected virtual void Start()
    {
        if (!MidiParser.ReadMidiFile(midiPath))
        {
            Debug.LogError($"MIDI file {midiPath} could not be loaded");
            isInitialized = false;
            return;
        }
        trackInfo = MidiParser.GetTrackInfo();
        bpm = MidiParser.bpm;
        ppq = MidiParser.ppq;
        timePerTick = 60000 / (bpm * ppq);
        lookAheadOffset *= ppq;
        currentTick = 0;
        realtime = 0;
        isMinigameActive = false;
        isInitialized = true;
    }

    protected void BeginMinigame()
    {
        isMinigameActive = true;
    }

    protected virtual void Update()
    {
        if (!isInitialized)
        {
            return; // Minigame is broken
        }

        if (currentIndex == trackInfo.Length)
        {
            if (hitList.Count == 0)
                isMinigameActive = false;
        }
        if (isMinigameActive && currentIndex != trackInfo.Length)
        {
            if (lookAheadIndex < trackInfo.Length && trackInfo[lookAheadIndex].time <= lookAheadTick)
            {
                hitList.Add(trackInfo[lookAheadIndex]);
                OnLookAhead?.Invoke(trackInfo[lookAheadIndex]);

                lookAheadIndex++;
            }
            if (trackInfo[currentIndex].time <= currentTick)
            {
                currentIndex++;
            }

            realtime += Time.deltaTime;
            currentTick = (long)(realtime / timePerTick * 1000);
            lookAheadTick = (long)(realtime / timePerTick * 1000) + lookAheadOffset;
        }

        CheckSuccess();
    }

    private void InputRight_started(InputAction.CallbackContext obj)
    {
        CheckSuccess(NoteInput.Right);
    }

    private void InputLeft_started(InputAction.CallbackContext obj)
    {
        CheckSuccess(NoteInput.Left);
    }

    private void InputDown_started(InputAction.CallbackContext obj)
    {
        CheckSuccess(NoteInput.Down);
    }

    private void InputUp_started(InputAction.CallbackContext obj)
    {
        CheckSuccess(NoteInput.Up);
    }

    private void CheckSuccess(NoteInput playerInput = NoteInput.None)
    {
        HitType result = HitType.None;
        if (hitList.Count > 0)
        {
            Debug.Log(hitList.Count);
            // Check timing
            for (int i = 0; i < hitList.Count; i++)
            {
                NoteInfo currentNote = hitList[i];
                float timeDifference = (currentTick - currentNote.time) * timePerTick * 1000;
                float absTimeDifference = Mathf.Abs(timeDifference);
                bool isCorrectHit = playerInput == currentNote.input;

                // Determine if input timing was good
                if (absTimeDifference <= TimingWindows.Perfect)
                {
                    result = HitType.Perfect;
                }
                else if (absTimeDifference <= TimingWindows.Great)
                {
                    result = HitType.Great;
                }
                else if (absTimeDifference <= TimingWindows.Good)
                {
                    result = HitType.Good;
                }
                else if (timeDifference > TimingWindows.Good)
                {
                    result = HitType.Miss;
                }

                // If good timing, check for correct input
                if (result != HitType.Miss)
                {
                    if (!isCorrectHit)
                    {
                        result = HitType.Miss;
                    }
                }

                // If a note was determined to be hit or missed, remove all notes ahead
                // of it in the hit list. Mark all skipped notes as missed, then report
                // resolved note as current HitType.
                // Ex.
                // [ Miss, Miss, Great, None, None ]
                // Becomes
                // [ None, None ]
                if (result != HitType.None)
                {
                    // Remove previous missed notes
                    for (int j = 0; j < i; j++)
                    {
                        OnHit?.Invoke(hitList[j], HitType.Miss);
                        hitList.RemoveAt(j);
                    }
                    // Remove current note
                    OnHit?.Invoke(hitList[i], result);
                    hitList.RemoveAt(i);

                    // We're done here
                    break;
                }
            }
        }
    }
}
