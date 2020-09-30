using System;
using System.IO;
using UnityEngine;

namespace Minigames.Intermission {
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

    [System.Serializable]
    public struct Save {
        public short hits;
        public short shots;
    }

    private void Start() {
        var save = new Save();
        save.hits = 123;
        save.shots = 1123;
        string json = JsonUtility.ToJson(save);

        var file = File.Create(Application.persistentDataPath + "/gamesave.json");
        byte[] byteArray = new byte[json.Length];

        for (int i = 0; i < json.Length; i++) {
            byteArray[i] = (byte)json[i];
        }

        file.Write(byteArray, 0, byteArray.Length);
        file.Close();

        file = File.Open(Application.persistentDataPath + "/gamesave.json", FileMode.Open);
        var byteArrayRead = new byte[file.Length];
        file.Read(byteArrayRead, 0, (int)file.Length);
        var charArray = new char[byteArrayRead.Length];
        for (int i = 0; i < byteArrayRead.Length; i++) {
            charArray[i] = (char)byteArrayRead[i];
        }
        var readString = string.Concat(charArray);
        file.Close();

        var saveString = File.ReadAllText(Application.persistentDataPath + "/gamesave.json");

        var loadedJson = JsonUtility.FromJson<Save>(saveString);
        file.Close();

        Debug.Log(readString);
    }
}
}