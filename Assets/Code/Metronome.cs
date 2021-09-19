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

    public float TickRemainingPercentage {
        get {
            float tickTime = 60f / beatsPerMinute;
            return _tickCountdown / tickTime;
        }
    }

    [SerializeField] private int beatsPerMinute = 60;
    public UnityEvent tickEvent;

    private float _tickCountdown = 0f;

    private void Update() {
        _tickCountdown -= Time.deltaTime;
        if (_tickCountdown <= 0f) {
            tickEvent?.Invoke();

            _tickCountdown = 60f / beatsPerMinute;
        }
    }
}
