using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Bingo {

    public struct Ticket {
        public bool Active;
        public GameObject Text;
        public GameObject Background;
    } 

    public class BingoTable : MonoBehaviour {
        public GameObject TicketText;
        public GameObject TicketBackground;
        public Transform SpawnNewTicketPosition;
        public float TheTicketIsFallingSpeed = 0.1f;

        public Color ActiveColor;
        public int Rows;
        public int Columns;
        public Canvas Canvas;
        public Camera CurrentCamera;

        private Ticket[][] board;
        private List<Ticket> liveTickets = new List<Ticket>();
        private float spaceRows;
        private float spaceColumns;
        private Vector2 offset = new Vector2();
        private Vector2 currentSelection = new Vector2();

        private Ticket fallingTicket;

        private void Start() {
            // last column for falling numbers
            Columns--;
            Rows--;
            board = new Ticket[Rows][];
            for (var i = 0; i < Rows; i++)
                board[i] = new Ticket[Columns];

            currentSelection.x = 0;
            currentSelection.y = 0;

            spaceRows = CurrentCamera.orthographicSize * 2 / (Rows + 1); 
            spaceColumns = CurrentCamera.orthographicSize * 2 * CurrentCamera.aspect / (Columns + 1);

            offset.x = -CurrentCamera.orthographicSize * CurrentCamera.aspect + (TicketBackground.transform.localScale.x * 2); 
            offset.y = -CurrentCamera.orthographicSize + (TicketBackground.transform.localScale.x * 2); // gamemanager: + y MinigameManagerOffset
            createTable();
            board[(int)currentSelection.y][(int)currentSelection.x].Background.GetComponent<SpriteRenderer>().color = ActiveColor;
        }

        private void FixedUpdate() {
            if (fallingTicket.Active) {
                var newPos = fallingTicket.Background.transform.position - new Vector3(0, TheTicketIsFallingSpeed * Time.fixedDeltaTime, 0); // gamemanager: + y MinigameManagerOffset

                if (newPos.y < offset.y)
                {
                    resetFallingTicket();

                    return;
                }

                fallingTicket.Background.transform.position = newPos; 
                fallingTicket.Text.transform.position = newPos;
            }            
        }

        private void resetFallingTicket(){
            // gamemanager: lose life
            Destroy(fallingTicket.Background);
            Destroy(fallingTicket.Text);
            fallingTicket.Active = false;

            SpawnNewFallingNumber();
        }
        
        private void findRandomActiveBoardCell(out int row, out int column) {
            do {
                row = Random.Range(0, board.Length);
                column = Random.Range(0, board[0].Length);
            } while (!board[row][column].Active);
        }

        public void SpawnNewFallingNumber() {
            int randomColumn, randomRow;

            findRandomActiveBoardCell(out randomRow, out randomColumn);

            fallingTicket = new Ticket();
            GameObject ticketText, ticketBackground;

            spawnNewTicket(SpawnNewTicketPosition.position, out ticketText, out ticketBackground);

            fallingTicket.Text = ticketText;
            fallingTicket.Background = ticketBackground;
            fallingTicket.Text.GetComponent<Text>().text = board[randomRow][randomColumn].Text.GetComponent<Text>().text;
            fallingTicket.Active = true;
        }   

        public void Select() {
            if (fallingTicket.Text.GetComponent<Text>().text == board[(int)currentSelection.y][(int)currentSelection.x].Text.GetComponent<Text>().text) {
                resetFallingTicket();
                board[(int)currentSelection.y][(int)currentSelection.x].Active = false;
                Destroy(board[(int)currentSelection.y][(int)currentSelection.x].Background);
                Destroy(board[(int)currentSelection.y][(int)currentSelection.x].Text);
            }
        }

        public void Traverse(int row, int col) {

            board[(int)currentSelection.y][(int)currentSelection.x].Background.GetComponent<SpriteRenderer>().color = Color.white;

            if (currentSelection.x + col < 0)
                currentSelection.x = board[0].Length - 1;
            else if (currentSelection.x + col > board[0].Length - 1)
                currentSelection.x = 0;
            else
                currentSelection.x += col;

            if (currentSelection.y + row < 0)
                currentSelection.y = board.Length - 1;
            else if (currentSelection.y + row > board.Length - 1)
                currentSelection.y = 0;
            else
                currentSelection.y += row;

            board[(int)currentSelection.y][(int)currentSelection.x].Background.GetComponent<SpriteRenderer>().color = ActiveColor;
        }

        private void spawnNewTicket(in Vector2 pos, out GameObject textObject, out GameObject backgroundObject) {
            textObject = Instantiate(TicketText, pos, Quaternion.identity, Canvas.transform);
            textObject.transform.localScale = Vector3.one;
            backgroundObject = Instantiate(TicketBackground, pos, Quaternion.identity, transform);
        }

        private void createTable() {
            var uniqueBalls = new Queue<int>();

            for (var i = 1; i < Rows*Columns + 1; i++)
                uniqueBalls.Enqueue(i);

            // gamemanager: shuffle uniqueBalls 

            for (var row = 0; row < Rows; ++row)
                for (var col = 0; col < Columns; ++col) {  
                    var newTicket = new Ticket();

                    var pos = new Vector2(spaceColumns * col + offset.x, spaceRows * row + offset.y); // gamemanager: offset y
                    GameObject ticketText, ticketBackground;

                    spawnNewTicket(pos, out ticketText, out ticketBackground);

                    newTicket.Text = ticketText;
                    newTicket.Background = ticketBackground;

                    newTicket.Text.GetComponent<Text>().text = uniqueBalls.Dequeue().ToString();
                    newTicket.Active = true;
                    board[row][col] = newTicket;
                }
        }
    }
} 