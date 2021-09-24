using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMinigame : Minigame
{
    public GameObject left;
    public GameObject up;
    public GameObject down;
    public GameObject right;

    protected override void Start()
    {
        base.Start();
        OnLookAhead += TestMinigame_OnLookAhead;
        OnHit += TestMinigame_OnHit;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void TestMinigame_OnHit(NoteInfo noteInfo, HitType hitType)
    {
        switch (noteInfo.input)
        {
            case NoteInput.Left:
                left.SetActive(true);
                break;
            case NoteInput.Up:
                up.SetActive(true);
                break;
            case NoteInput.Down:
                down.SetActive(true);
                break;
            case NoteInput.Right:
                right.SetActive(true);
                break;
        }
    }

    private void TestMinigame_OnLookAhead(NoteInfo noteInfo)
    {
        switch (noteInfo.input)
        {
            case NoteInput.Left:
                left.SetActive(false);
                break;
            case NoteInput.Up:
                up.SetActive(false);
                break;
            case NoteInput.Down:
                down.SetActive(false);
                break;
            case NoteInput.Right:
                right.SetActive(false);
                break;
        }
    }
}
