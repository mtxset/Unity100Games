using System.Collections.Generic;
using Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Minigolf
{
    public class LevelGenerator : AddMinigameManager2 {
    public Texture2D[] Maps;
    public float ScaleDown = 10;
    public Camera Camera;
    public GameObject BallPrefab;
    public ColorToPrefab[] ColorToPrefab;
    public Color ColorDarkGreen;
    public Vector2 CurrentMapStartingPoint { get; private set; }

    // currently there is no offset compensation, so prefabs overlap
    // if lags - pregenerate maps

    private int currentMap;
    private List<GameObject> mapItems;
    private Vector2 zeroOffset;
    private List<int> playedMaps;

    private void Start() {
        mapItems = new List<GameObject>();
        playedMaps = new List<int>();

        zeroOffset = new Vector2(
            -Camera.orthographicSize * Camera.aspect,
            -Camera.orthographicSize);
            
        generateMap();
    }

    private void generateMap() { 
        for (int x = 0; x < Maps[currentMap].width; x++) {
            for (int y = 0; y < Maps[currentMap].height; y++) {
                var pixelColor = Maps[currentMap].GetPixel(x, y);
                // empty
                if (pixelColor.a == 0 || pixelColor.Equals(Color.white)) continue;

                pixelColor = approximateColor(pixelColor);
                
                var coords = new Vector2(
                    x/ScaleDown + zeroOffset.x, 
                    y/ScaleDown + zeroOffset.y + MinigameManager.transform.position.y);

                // starting point
                if (pixelColor == Color.green) {
                    BallPrefab.transform.position = coords;
                    CurrentMapStartingPoint = coords;
                }

                // area effector            DARK GREEN
                // does not work because can't find gray color
                /*if (pixelColor.Equals(ColorDarkGreen)) {
                    // find direction 
                    for (var subX = -1; subX <= 1; subX++) {
                        for (var subY = -1; subY <= 1; subY++) {
                            if (x + subX > Maps[currentMap].width 
                             || y + subY > Maps[currentMap].height) {
                                 continue;
                            } else {
                                var checkColor = approximateColor(Maps[currentMap].GetPixel(x + subX, y + subY));
                                Debug.Log($"{checkColor.r}, {checkColor.g}, {checkColor.b}, {checkColor.a}");
                                if (checkColor.Equals(Color.gray)) {
                                    var angleToRotate = 0.0f;
                                    if (subX == 1 && subY == 1) angleToRotate = -45;
                                    else if (subX == 0 && subY == 1) angleToRotate = -90;
                                    else if (subX == -1 && subY == 1) angleToRotate = -135;
                                    else if (subX == -1 && subY == 0) angleToRotate = -180;
                                    else if (subX == -1 && subY == -1) angleToRotate = -225;
                                    else if (subX == 0 && subY == -1) angleToRotate = -270;
                                    else if (subX == 1 && subY == -1) angleToRotate = -315;

                                    mapItems.Add(
                                    Instantiate(
                                        getPrefab(pixelColor), 
                                        coords, 
                                        Quaternion.Euler(0, 0, angleToRotate),
                                        transform));
                                }
                            }
                        }
                    }
                    continue;
                } */
                var prefab = getPrefab(pixelColor);

                if (prefab)
                    mapItems.Add(
                    Instantiate(
                        getPrefab(pixelColor), 
                        coords, 
                        Quaternion.identity,
                        transform));
            }
        }
    }

    private Color approximateColor(Color color, int precision = 2) {
        var multiPlier = Mathf.Pow(10.0f, precision);
        return new Color(
            Mathf.Round(color.r * multiPlier) * 1 / multiPlier,
            Mathf.Round(color.g * multiPlier) * 1 / multiPlier,
            Mathf.Round(color.b * multiPlier) * 1 / multiPlier,
            color.a);
    }

    private GameObject getPrefab(Color color) {
         foreach (var item in ColorToPrefab) {
            if (item.Color.Equals(color))
                return item.Prefab;
        }

        return null;
    }

    private void cleanCurrentMap() {
        foreach (var item in mapItems) {
            Destroy(item);
        }
        mapItems.Clear();
    }

    public void NextMap() {

        do {
            currentMap = Random.Range(0, Maps.Length);
        } while (playedMaps.Contains(currentMap));

        playedMaps.Add(currentMap);

        if (playedMaps.Count == Maps.Length) {
            playedMaps.Clear();
        }
        
        cleanCurrentMap();
        generateMap();
    }    
}    
}