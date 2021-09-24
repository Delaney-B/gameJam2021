using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Interaction;
using System.IO;

public static class MidiParser
{
    public static short ppq;
    public static float bpm;

    static IEnumerable<Note> notes;

    public static bool ReadMidiFile(string relativePath)
    {
        string filepath = Path.Combine(Application.dataPath, relativePath);
        if (!File.Exists(filepath))
        {
            Debug.LogError($"Could not find file {filepath}");
            return false;
        }

        MidiFile midiFile = MidiFile.Read(filepath);
        notes = midiFile.GetNotes();
        ppq = ((TicksPerQuarterNoteTimeDivision)midiFile.TimeDivision).TicksPerQuarterNote;

        bpm = (float)midiFile.GetTempoMap()
            .GetTempoAtTime(new MetricTimeSpan(0))
            .BeatsPerMinute;
        if (bpm == 0)
        {
            bpm = 120;
        }
        retrun true;
    }

    public static NoteInfo[] GetTrackInfo()
    {
        List<NoteInfo> trackInfo = new List<NoteInfo>();
        long lastNoteTime = -1L;
        NoteInfo noteInfo = new NoteInfo();
        bool noteAdded;
        foreach (Note note in notes)
        {
            noteAdded = false;

            // Convert the note number to the appropriate input direction
            NoteInput noteInput = new NoteInput();
            switch (note.NoteNumber)
            {
                case 60:
                    noteInput = NoteInput.Left;
                    break;
                case 61:
                    noteInput = NoteInput.Up;
                    break;
                case 62:
                    noteInput = NoteInput.Down;
                    break;
                case 63:
                    noteInput = NoteInput.Right;
                    break;
                default:
                    break;
            }

            // Note is part of a chord
            if (lastNoteTime == note.Time)
            {
                noteInfo = trackInfo[trackInfo.Count - 1];
                noteInfo.input |= noteInput;
                noteInfo.chord = true;
            }
            // New note
            else if (lastNoteTime < note.Time)
            {
                noteInfo = new NoteInfo
                {
                    input = noteInput,
                    time = note.Time,
                    chord = false
                };
                lastNoteTime = note.Time;
                noteAdded = true;
            }

            if (noteAdded)
                trackInfo.Add(noteInfo);
        }

        return trackInfo.ToArray();
    }
}
