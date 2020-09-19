using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Minigolf {
public class LevelGenerator : MonoBehaviour {
    public Texture2D[] Maps;
    public float ScaleDown = 10;
    public Camera Camera;
    public GameObject BallPrefab;
    public ColorToPrefab[] ColorToPrefab;

    // currently there is no offset compensation, so prefabs overlap
    // if lags - pregenerate maps

    private int currentMap;
    private List<GameObject> mapItems;
    private Vector2 zeroOffset;

    private void Start() {
        mapItems = new List<GameObject>();

        zeroOffset = new Vector2(
            -Camera.orthographicSize * Camera.aspect,
            -Camera.orthographicSize);
            
        generateMap();
    }

    private void generateMap() { 
        for (int x = 0; x < Maps[currentMap].width; x++) {
            for (int y = 0; y < Maps[currentMap].height; y++) {
                var pixelColor = Maps[currentMap].GetPixel(x, y);

                if (pixelColor.a == 0 || pixelColor == Color.white) continue;

                var coords = new Vector2(x/ScaleDown + zeroOffset.x, y/ScaleDown + zeroOffset.y);

                if (pixelColor == Color.green) {
                    BallPrefab.transform.position = coords;
                }

                foreach (var item in ColorToPrefab) {
                    if (item.Color.Equals(pixelColor)) {
                        mapItems.Add(
                        Instantiate(
                            item.Prefab, 
                            coords, 
                            Quaternion.identity,
                            transform));
                    }
                }
            }
        }
    }

    private void cleanCurrentMap() {
        foreach (var item in mapItems) {
            Destroy(item);
        }
        mapItems.Clear();
    }

    public void NextMap() {
        if (++currentMap == Maps.Length) currentMap = 0;
        
        cleanCurrentMap();
        generateMap();
    }
    
}    
}