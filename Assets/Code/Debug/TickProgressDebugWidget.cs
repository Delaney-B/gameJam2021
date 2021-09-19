using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TickProgressDebugWidget : MonoBehaviour {
    [SerializeField] private Slider debugSlider;
    [SerializeField] private GameObject tickIndicator;

    private void Awake() {
        var metronome = Metronome.GetInstance();

        metronome.tickEvent.AddListener(TickCallback);

        if (tickIndicator) {
            tickIndicator.SetActive(false);
        }
    }

    private void Update() {
        var metronome = Metronome.GetInstance();

        debugSlider.value = metronome.TickRemainingPercentage;
    }

    private void TickCallback() {
        StartCoroutine(Tick_CR());
    }

    private IEnumerator Tick_CR() {
        if (!tickIndicator) {
            yield break;
        }

        tickIndicator.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        tickIndicator.SetActive(false);
    }
}
