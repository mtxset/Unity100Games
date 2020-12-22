using System;

namespace Minigames.Tetris {
public class TetrisEvents {
    public event Action OnTetrisBlockDropped;

    public void TetrisBlockDroppedEvent() {
        OnTetrisBlockDropped?.Invoke();
    }
}
}