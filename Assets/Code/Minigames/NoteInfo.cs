using System.Collections.Generic;

public struct NoteInfo
{
    public NoteInput input;
    public long time;
    public bool chord;

    public static bool operator ==(NoteInfo lhs, NoteInfo rhs)
    {
        return lhs.input == rhs.input && lhs.time == rhs.time;
    }

    public static bool operator !=(NoteInfo lhs, NoteInfo rhs)
    {
        return lhs.time != rhs.time && lhs.input != rhs.input;
    }
}
