using System.Collections.Generic;
using UnityEngine;

namespace Bingo {

    public struct Ticket {
        public GameObject Text;
        public GameObject Background;
    } 

    public class BingoTable : MonoBehaviour {
        public GameObject TicketText;
        public GameObject TicketBackground;

        public Color ActiveColor;
        public int Rows;
        public int Columns;
        public Canvas Canvas;
        public Camera CurrentCamera;

        private int[][] board;
        private List<Ticket> liveTickets = new List<Ticket>();
        private float spaceRows;
        private float spaceColumns;
        private Vector2 offset = new Vector2();

        private void Start() {
            // last column for falling numbers
            Columns--;
            board = new int[Rows][];
            for (var i = 0; i < Rows; i++)
                board[i] = new int[Columns];

            spaceRows = CurrentCamera.orthographicSize * 2 / Rows; 
            spaceColumns = CurrentCamera.orthographicSize * 2 * CurrentCamera.aspect / Columns;

            offset.x = -CurrentCamera.orthographicSize * CurrentCamera.aspect + (TicketBackground.transform.localScale.x * 2); 
            offset.y = -CurrentCamera.orthographicSize + (TicketBackground.transform.localScale.x * 2); // + y MinigameManagerOffset
            createTable();
        }

        private void createTable() {
            for (var row = 0; row < Rows; ++row)
                for (var col = 0; col < Columns; ++col) {  
                    var newTicket = new Ticket();

                    var pos = new Vector2(spaceColumns * col + offset.x, spaceRows * row + offset.y);
                    var ticketText = Instantiate(
                        TicketText, 
                        pos, 
                        Quaternion.identity, 
                        Canvas.transform);

                    ticketText.transform.localScale = Vector3.one; // it's 11:00 pm I don't know

                    var ticketBackground = Instantiate(
                        TicketBackground,
                        pos,
                        Quaternion.identity,
                        transform
                    );

                    newTicket.Text = ticketText;
                    newTicket.Background = ticketBackground;
                    
                }
        }
    }
} 