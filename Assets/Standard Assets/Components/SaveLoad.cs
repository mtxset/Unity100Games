using System.IO;
using UnityEngine;

namespace Components {
public class SaveLoad {

    public struct OpenFile {
        public bool Failed;
        public object Result;
    }

    public readonly static string Path = Application.persistentDataPath;

    public static void SaveCurrentGameParams(object objectToSave) {
        string json = JsonUtility.ToJson(objectToSave);

        var file = File.Create($"{Path}/{GameManager.GameManager.Instance.GetCurrentGameName()}.json");
        var byteArray = new byte[json.Length];

        for (int i = 0; i < json.Length; i++) {
            byteArray[i] = (byte)json[i];
        }

        file.Write(byteArray, 0, byteArray.Length);
        file.Close();
    }

    public static OpenFile LoadCurrentGameParams<T>() {
        var result = new OpenFile();
        var path = $"{Path}/{GameManager.GameManager.Instance.GetCurrentGameName()}.json";

        if (!File.Exists(path)) {
            result.Failed = true;
            return result;
        }

        var file = File.Open(path, FileMode.Open);

        var byteArrayRead = new byte[file.Length];
        file.Read(byteArrayRead, 0, (int)file.Length);
        file.Close();

        var charArray = new char[byteArrayRead.Length];
        
        for (int i = 0; i < byteArrayRead.Length; i++) {
            charArray[i] = (char)byteArrayRead[i];
        }

        var readString = string.Concat(charArray);

        result.Result = JsonUtility.FromJson<T>(readString);
        
        return result;
    }

}
}