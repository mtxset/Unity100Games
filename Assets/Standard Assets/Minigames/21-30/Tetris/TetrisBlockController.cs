using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Tetris {
public class TetrisBlockController : MonoBehaviour {
    public Vector3 RotationPoint; 
    public float FallRate = 1;
    public bool Remnant;

    private float fallTimer;
    private float moveOffset;

    private Vector2 maxMoveOffset;
    private MinigameManager minigameManager;

    // if true means it's piece which is left after row destruction

    private void Start() {
        moveOffset = transform.localScale.x*2;
        minigameManager = GetComponentInParent<MinigameManager>();
        maxMoveOffset.x = minigameManager.MaxBlocksWidth;
        maxMoveOffset.y = minigameManager.MaxBlocksHeight;

        minigameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButton;
        minigameManager.ButtonEvents.OnRightButtonPressed += HandleRightButton;
        minigameManager.ButtonEvents.OnUpButtonPressed += HandleRotation;
        minigameManager.ButtonEvents.OnDownButtonPressed += HandleDown;
    }

    private void OnDisable() {
        minigameManager.ButtonEvents.OnLeftButtonPressed -= HandleLeftButton;
        minigameManager.ButtonEvents.OnRightButtonPressed -= HandleRightButton;
        minigameManager.ButtonEvents.OnUpButtonPressed -= HandleRotation;
        minigameManager.ButtonEvents.OnDownButtonPressed -= HandleDown;
    }

    private void HandleDown() => DropBlock();

    private void HandleLeftButton() => MoveCurrentBlock(-1);

    private void HandleRightButton() => MoveCurrentBlock(1); 

    private void HandleRotation() => Rotate();

    public void MoveCurrentBlock(int leftRight) {
        transform.position += new Vector3(leftRight * moveOffset, 0, 0);
        if (!valideMove()) {
            transform.position -= new Vector3(leftRight * moveOffset, 0, 0);
        }
    }

    public void DropBlock() {
        FallRate /= 10;
    }

    public void Rotate() {
        transform.RotateAround(
            transform.TransformPoint(RotationPoint), Vector3.forward, 90);
        if (!valideMove()) {
            transform.RotateAround(
                transform.TransformPoint(RotationPoint), Vector3.forward, -90);
        }
    }

    private void FixedUpdate() {
        moveTetrisPiece();
    }

    private void moveTetrisPiece() {
        if ((fallTimer += Time.fixedDeltaTime) > FallRate)
        {
            transform.position += new Vector3(0, -1 * moveOffset, 0);
            if (!valideMove())
            {                
                if (!Remnant)
                    minigameManager.TetrisEvents.TetrisBlockDroppedEvent();
                transform.position -= new Vector3(0, -1 * moveOffset, 0);
                addToGrid();
                checkLines();
                this.enabled = false;
            }
            fallTimer = 0;
        }
    }

    private void addToGrid() {
        foreach (Transform item in transform) {
            
            var x = item.transform.position.x;
            var y = item.transform.position.y;

            var indexX = Mathf.RoundToInt((x + maxMoveOffset.x-1)/2);
            var indexY = Mathf.RoundToInt((y + maxMoveOffset.y-1)/2);

            minigameManager.Grid[indexX, indexY] = item;
        }
    }

    private bool hasFullLine(int line) {
        for (int i = 0; i < minigameManager.MaxBlocksWidth; i++) {
            if (minigameManager.Grid[i, line] == null)
                return false;
        }

        return true;
    }

    private void letFloatingFall(int line) {
        var parents = new HashSet<TetrisBlockController>();

        for (int y = line; y < minigameManager.MaxBlocksHeight; y++) {
            for (int x = 0; x < minigameManager.MaxBlocksWidth; x++) {
                if (minigameManager.Grid[x, y] != null) {
                    parents.Add(minigameManager.Grid[x, y].GetComponentInParent<TetrisBlockController>());
                }
            }
        }

        foreach (var item in parents) {
            item.Remnant = true;
            item.enabled = true;
        }
    }

    private void deleteLine(int line) {
        for (int i = 0; i < minigameManager.MaxBlocksWidth; i++) {
            Destroy(minigameManager.Grid[i, line].gameObject);
            minigameManager.Grid[i, line] = null;
        }

        letFloatingFall(line);
    }

    private void rowDown(int line) {
        for (var i = line; i < minigameManager.MaxBlocksHeight; i++) {
            for (var c = 0; c < minigameManager.MaxBlocksWidth; c++) {
                if (minigameManager.Grid[c, i] != null) {
                    minigameManager.Grid[c, i - 1] = minigameManager.Grid[c, i];
                    minigameManager.Grid[c, i - 1].transform.position += Vector3.down * 2;
                    minigameManager.Grid[c, i] = null;
                }
            }
        }
    }

    private void checkLines() {
        for (var i = minigameManager.MaxBlocksHeight - 1; i >=0; i--) {
            if (hasFullLine(i)) {
                deleteLine(i);
                // rowDown(i);
            }
        }
    }

    private bool valideMove() {
        foreach (Transform item in transform)
        {
            var x = item.transform.position.x;
            var y = item.transform.position.y;

            var indexX = Mathf.RoundToInt((x + maxMoveOffset.x-1)/2);
            var indexY = Mathf.RoundToInt((y + maxMoveOffset.y-1)/2);

            if (x < -maxMoveOffset.x || x >= maxMoveOffset.x 
                || y <= -maxMoveOffset.y) {
                return false;
            }

            if (minigameManager.Grid[indexX, indexY] != null)
                return false;
        }

        return true;
    }
}
}

