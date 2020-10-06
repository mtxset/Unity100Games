using UnityEngine;

namespace Minigames.Intermission
{
    public class Intermission : MonoBehaviour {
    /* 
    Requirments:
    A. Save/Load system for each minigame
    B. Menu 

    0. After each game Minigame Intermission, before Intermission
    1. Create menu? with 1. Replay, 2. Adjust params, 3. Next game (Default)
    2. Replay sets same game
    3. Adjust params somehow x amount of minigames's parameters are chosen (I need to ensure that all parameters are traversed)
    3.1. Players vote which parameter to change
    3.2. They choose to increase it or decrease by x percent
    3.3. Game is replayed
    */

    /*
    Saving:
    Path/Game.json
    */

    [System.Serializable]
    public struct Save {
        public int hits;
        public int shots;
    }

    private void Start() {
        var save = new Save {
            hits = 123,
            shots = 1231
        };
        Components.SaveLoad.SaveCurrentGameParams(save);
    }
}
}