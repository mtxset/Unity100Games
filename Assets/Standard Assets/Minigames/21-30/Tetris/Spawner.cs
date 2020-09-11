using UnityEngine;

namespace Minigames.Tetris {
    public class Spawner : MonoBehaviour {
        public GameObject[] TetrisParts;

        private MinigameManager gameManager;
        
        private void Start() {
            gameManager = GetComponentInParent<MinigameManager>();
            gameManager.TetrisEvents.OnTetrisBlockDropped += HandleTetrisDropped;
            SpawnNewPiece();
        }

        private void OnDisable() {
            gameManager.TetrisEvents.OnTetrisBlockDropped -= HandleTetrisDropped;
        }

        private void HandleTetrisDropped() => SpawnNewPiece();

        public void SpawnNewPiece() {

            if (gameManager.GameOver) return;

            Instantiate(
                TetrisParts[Random.Range(0, TetrisParts.Length)], 
                transform.position,
                Quaternion.identity,
                transform);
        }
    }
}