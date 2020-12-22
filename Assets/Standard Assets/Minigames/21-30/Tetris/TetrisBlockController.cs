using UnityEngine;

namespace Minigames.Tetris {
public class TetrisBlockController : MonoBehaviour {
    
    public Vector3 RotationPoint; 
    public float FallRate = 1;

    private float fallTimer;
    private float moveOffset;
    
    private MinigameManager minigameManager;
    private float spawnPointY;

    // if true means it's piece which is left after row destruction

    private void Start() {

        spawnPointY = transform.position.y;
        moveOffset = 1.0f;
        minigameManager = GetComponentInParent<MinigameManager>();

        minigameManager.ButtonEvents.OnLeftButtonPressed += HandleLeftButton;
        minigameManager.ButtonEvents.OnRightButtonPressed += HandleRightButton;
        minigameManager.ButtonEvents.OnUpButtonPressed += HandleRotation;
        minigameManager.ButtonEvents.OnDownButtonPressed += HandleDown;

        if (!valid_move()) {
            // TODO: manage loss
        }
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
        if (minigameManager.GameOver) return;

        transform.position += new Vector3(leftRight * moveOffset, 0, 0);
        
        if (!valid_move()) {
            transform.position -= new Vector3(leftRight * moveOffset, 0, 0);
        }
    }

    public void DropBlock() {
        FallRate /= 10;
    }

    public void Rotate() {
        if (minigameManager.GameOver) return;

        transform.RotateAround(
            transform.TransformPoint(RotationPoint), Vector3.forward, 90);

        if (!valid_move()) {
            transform.RotateAround(
                transform.TransformPoint(RotationPoint), Vector3.forward, -90);
        }
    }

    private void FixedUpdate() {
        if (minigameManager.GameOver) return;



        move_tetromino_down();
    }

    public Vector2 round_vec2(Vector2 v) {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    private void move_tetromino_down() {

        if ((fallTimer += Time.fixedDeltaTime) > FallRate) {
            fallTimer = 0;
            transform.position += new Vector3(0, -1 * moveOffset, 0);
            if (!valid_move()) {                
                transform.position -= new Vector3(0, -1 * moveOffset, 0);
                minigameManager.TetrisEvents.TetrisBlockDroppedEvent();
                update_grid();
                delete_full_rows();
                this.enabled = false;
            }
        }
    }

    void update_grid() {
        for (var row = 0; row < minigameManager.MaxBlocksHeight; ++row)
            for (var col = 0; col < minigameManager.MaxBlocksWidth; ++col)
                if (minigameManager.Grid[col, row] != null)
                    if (minigameManager.Grid[col, row].parent == transform)
                        minigameManager.Grid[col, row] = null;

        // Add new children to grid
        foreach (Transform child in transform) {
            int x, y;
            vec2_to_index(child.position, out x, out y);
            minigameManager.Grid[x, y] = child;
        }
    }

    private void vec2_to_index(Vector2 position, out int x, out int y) {
        x = Mathf.RoundToInt(position.x);
        y = Mathf.RoundToInt(position.y - minigameManager.transform.position.y);
    }

    private bool has_full_row(int row) {
        for (int col = 0; col < minigameManager.MaxBlocksWidth; ++col) {
            if (minigameManager.Grid[col, row] == null)
                return false;
        }

        return true;
    }

    private void delete_full_rows() {
        for (int row = 0; row < minigameManager.MaxBlocksHeight; ++row) {
            if (has_full_row(row)) {
                delete_row(row);
                drop_rows_above(row+1);
                --row;
            }
        }
    }

    private void delete_row(int row) {
        for (int col = 0; col < minigameManager.MaxBlocksWidth; ++col) {
            Destroy(minigameManager.Grid[col, row].gameObject);
            minigameManager.Grid[col, row] = null;
            minigameManager.Events.EventScored();
        }
    }

    private void drop_rows_above(int row) {
        for (int i = row; i < minigameManager.MaxBlocksHeight; ++i)
            drop_row(i);
    }

    private void drop_row(int row) {
        for (var col = 0; col < minigameManager.MaxBlocksWidth; ++col) {
            if (minigameManager.Grid[col, row] == null)
                continue;
            
            minigameManager.Grid[col, row - 1] = minigameManager.Grid[col, row];    
            minigameManager.Grid[col, row] = null;
            minigameManager.Grid[col, row - 1].position += Vector3.down * moveOffset; 
        }
    }

    private bool valid_move() {
        foreach (Transform item in transform) {
            int x, y;
            vec2_to_index(item.position, out x, out y);

            if ((int)item.transform.position.x < 0 || 
                (int)item.transform.position.x >= minigameManager.MaxBlocksWidth ||
                (int)item.transform.position.y <= minigameManager.transform.position.y)
                return false;

            if (minigameManager.Grid[x, y] != null 
                && minigameManager.Grid[x, y].parent != transform)
                return false;
        }

        return true;
    }
}
}

