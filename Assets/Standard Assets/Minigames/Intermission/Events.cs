using System;

namespace Minigames.Intermission {
public class Events {
    public event Action OnIntermissionDone;

    public void IntermissionDone() => OnIntermissionDone?.Invoke();
}
}