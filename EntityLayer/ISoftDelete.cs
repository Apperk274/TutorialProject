using System;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedAt { get; set; }

    public void Undo()
    {
        DeletedAt = null;
    }
}