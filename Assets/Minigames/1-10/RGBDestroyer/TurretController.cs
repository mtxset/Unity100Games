using Assets.Shaders;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Minigames.RGBDestroyer
{
    class TurretController : MonoBehaviour
    {
        public GameObject TurretLeft = null;
        public GameObject TurretCenter = null;
        public GameObject TurretRight = null;

        public GameObject LaserPrefab = null;

        public float LaserSpeed = 10f;
        public int OutlineWidth = 9;
        public Color OutlineColor = Color.green;
        public float TurretRotationSpeed = 10f;

        public enum TurretControl
        {
            Left = 0,
            Center = 1,
            Right = 2,
        }

        private TurretControl turretControl = TurretControl.Center;

        private MinigameManager gameManager;

        private SpriteOutline turretLeftOutline;
        private SpriteOutline turretCenterOutline;
        private SpriteOutline turretRightOutline;

        private List<SpriteOutline> turretOutlines;
        private List<GameObject> turrets;
        private int currentColorIndex = 0;

        // can set this to public and add more colors
        private List<Color> colorList;

        private void Start()
        {
            this.colorList = new List<Color>
            {
                Color.red, Color.green, Color.blue
            };

            this.turretLeftOutline = this.TurretLeft.GetComponent<SpriteOutline>();
            this.turretCenterOutline = this.TurretCenter.GetComponent<SpriteOutline>();
            this.turretRightOutline = this.TurretRight.GetComponent<SpriteOutline>();
            this.turretOutlines = new List<SpriteOutline>
            {
                this.turretLeftOutline, this.turretCenterOutline, this.turretRightOutline 
            };

            foreach (var item in turretOutlines)
            {
                item.color = this.OutlineColor;
                item.outlineSize = 0;
            }

            turretOutlines[(int)TurretControl.Center].outlineSize = 9;

            this.turrets = new List<GameObject>
            {
                this.TurretLeft, this.TurretCenter, this.TurretRight
            };

            foreach (var item in turrets)
            {
                item.GetComponent<SpriteRenderer>().color = this.colorList[this.currentColorIndex];
            }

            this.gameManager = this.GetComponentInParent<MinigameManager>();
            this.subscribeToEvents();
        }

        private void Update()
        {
            if (this.gameManager.GameOver)
            {
                return;
            }

            this.turrets[(int)this.turretControl].transform.Rotate(
                0, 0, 1 * this.TurretRotationSpeed * Time.deltaTime);
        }

        private void outlineTurret(TurretControl turretControl)
        {
            for (int i = 0; i < this.turretOutlines.Count; i++)
            {
                if (i == (int)turretControl)
                {
                    turretOutlines[i].outlineSize = this.OutlineWidth;
                }
                else
                {
                    turretOutlines[i].outlineSize = 0;
                }
            }

        }

        private void OnDisable()
        {
            this.unsubscribeToEvents();
        }
        private void subscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
            this.gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
        }
        private void unsubscribeToEvents()
        {
            this.gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
            this.gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
            this.gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
            this.gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
        }

        private void HandleUpButtonPressed()
        {
            this.changeCurrentColor(1);
        }

        private void HandleDownButtonPressed()
        {
            this.changeCurrentColor(-1);
        }

        private void HandleActionButtonPressed()
        {
            var laser = Instantiate(this.LaserPrefab, this.gameManager.transform);
            laser.transform.position = this.turrets[(int)this.turretControl].transform.position;
            laser.GetComponent<Rigidbody2D>().AddForce(
                Vector2.up * this.LaserSpeed * Time.deltaTime);

            laser.GetComponent<LineRenderer>().startColor = this.colorList[this.currentColorIndex];
            laser.GetComponent<LineRenderer>().endColor = this.colorList[this.currentColorIndex];

            Destroy(laser, 3.0f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">1 forward, -1 back<param>
        private void changeCurrentColor(int direction)
        {
            if (direction == 1)
            {
                var nextColorIndex = this.currentColorIndex + 1;
                if (nextColorIndex == this.colorList.Count)
                {
                    this.currentColorIndex = 0;
                }
                else
                {
                    this.currentColorIndex = nextColorIndex;
                }
            }
            else if (direction == -1)
            {
                var previousColorIndex = this.currentColorIndex - 1;
                if (previousColorIndex < 0)
                {
                    this.currentColorIndex = this.colorList.Count - 1;
                }
                else
                {
                    this.currentColorIndex = previousColorIndex;
                }
            }

            foreach (var item in turrets)
            {
                item.GetComponent<SpriteRenderer>().color = this.colorList[this.currentColorIndex];
            }
        }

        private void HandleHorizontalPressed(InputValue inputValue)
        {
            switch (inputValue.Get<float>())
            {
                case -1:
                    this.turretControl = TurretControl.Left;
                    break;
                case 0:
                    this.turretControl = TurretControl.Center;
                    break;
                case 1:
                    this.turretControl = TurretControl.Right;
                    break;
            }

            this.outlineTurret(this.turretControl);
        }
    }
}
