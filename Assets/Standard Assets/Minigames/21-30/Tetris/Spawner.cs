using UnityEngine;

namespace Minigames.Tetris {
public class Spawner : MonoBehaviour {

    private float debug_grid_timer = 0;
    
    public GameObject[] TetrisParts;

    private MinigameManager minigameManager;
    
    private void Start() {
        minigameManager = GetComponentInParent<MinigameManager>();
        minigameManager.TetrisEvents.OnTetrisBlockDropped += HandleTetrisDropped;
        SpawnNewPiece();
    }

    private void OnDisable() {
        minigameManager.TetrisEvents.OnTetrisBlockDropped -= HandleTetrisDropped;
    }

    private void FixedUpdate() {
        if ((debug_grid_timer += Time.fixedDeltaTime) > 0.5f) {
            draw_debug_grid(0.5f);
            debug_grid_timer = 0;
        }    
    }

    private void HandleTetrisDropped() => SpawnNewPiece();

    public void SpawnNewPiece() {

        if (minigameManager.GameOver) return;

        Instantiate(
            TetrisParts[Random.Range(0, TetrisParts.Length)], 
            transform.position,
            Quaternion.identity,
            transform);
    }

    private void draw_debug_grid(float line_duration) {
        for (var row = 0; row < minigameManager.MaxBlocksHeight; ++row) {
            for (var col = 0; col < minigameManager.MaxBlocksWidth; ++col) {
                if (minigameManager.Grid[col, row] == null)
                    continue;
                
                var offset = minigameManager.Grid[col, row].transform.localScale.x/2 + 0.1f;

                Vector2 quad_pos = minigameManager.Grid[col, row].transform.position;
                
                var start = new Vector2();
                var end = new Vector2();

                start = quad_pos + new Vector2(-offset, offset);
                end = quad_pos + new Vector2(offset, offset);
                Debug.DrawLine(start, end, Color.red, line_duration);

                start = end;
                end = quad_pos + new Vector2(offset, -offset);
                Debug.DrawLine(start, end, Color.red, line_duration);

                start = end;
                end = quad_pos + new Vector2(-offset, -offset);
                Debug.DrawLine(start, end, Color.red, line_duration);

                start = end;
                end = quad_pos + new Vector2(-offset, offset);
                Debug.DrawLine(start, end, Color.red, line_duration);
            }
        }
    }
}
}