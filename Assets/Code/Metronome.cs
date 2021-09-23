using System;
using UnityEngine;
using UnityEngine.Events;

public class Metronome : MonoBehaviour {
    private static Metronome _instance;

    public static Metronome GetInstance() {
        if (!_instance) {
            _instance = FindObjectOfType<Metronome>();
            if (!_instance) {
                throw new Exception("Unable to find Metronome instance! Please add one to the scene.");
            }
        }

        return _instance;
    }

    public float BeatTime => 60f / beatsPerMinute;

    public float BeatProximity {
        get {
            float nextDiff = _nextTick - Time.fixedTime;
            float nextProximity = Mathf.Clamp01(Mathf.Abs(nextDiff / BeatTime));
            float prevDiff = _prevTick - Time.fixedTime;
            float prevProximity = Mathf.Clamp01(Mathf.Abs(prevDiff / BeatTime));

            return Math.Min(nextProximity, prevProximity);
        }
    }

    public float BeatRemaining {
        get {
            float diff = _nextTick - Time.fixedTime;
            return Mathf.Clamp01(diff / BeatTime);
        }
    }

    public float BeatProgress => 1f - BeatRemaining;

    [SerializeField] private int beatsPerMinute = 60;
    public UnityEvent tickEvent;

    private float _nextTick = 0f;
    private float _prevTick = 0f;

    private void FixedUpdate() {
        if (Time.fixedTime >= _nextTick) {
            tickEvent?.Invoke();

            _prevTick = _nextTick;
            _nextTick = Time.fixedTime + 60f / beatsPerMinute;
        }
    }
}
