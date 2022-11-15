using System.Collections.Generic;
using Shaders;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigames.RGBDestroyer
{
  internal class TurretController : MonoBehaviour
  {
    public GameObject TurretLeft;
    public GameObject TurretCenter;
    public GameObject TurretRight;

    public GameObject LaserPrefab;

    public float LaserSpeed = 10f;
    public int OutlineWidth = 9;
    public Color OutlineColor = Color.green;
    public float TurretRotationSpeed = 10f;

    private enum TurretControl
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
    private int currentColorIndex;

    // can set this to public and add more colors
    private List<Color> colorList;

    private void Start()
    {
      gameManager = GetComponentInParent<MinigameManager>();

      colorList = new List<Color>
      {
          Color.red, Color.green, Color.blue
      };

      turretLeftOutline = TurretLeft.GetComponent<SpriteOutline>();
      turretCenterOutline = TurretCenter.GetComponent<SpriteOutline>();
      turretRightOutline = TurretRight.GetComponent<SpriteOutline>();
      turretOutlines = new List<SpriteOutline>
      {
          turretLeftOutline, turretCenterOutline, turretRightOutline
      };

      foreach (var item in turretOutlines)
      {
        item.color = OutlineColor;
        item.outlineSize = 0;
      }

      turretOutlines[(int)TurretControl.Center].outlineSize = 9;

      turrets = new List<GameObject>
      {
          TurretLeft, TurretCenter, TurretRight
      };

      foreach (var item in turrets)
      {
        item.GetComponent<SpriteRenderer>().color = colorList[currentColorIndex];
      }

      subscribeToEvents();
    }

    private void Update()
    {
      if (gameManager.GameOver)
      {
        return;
      }

      outlineTurret(turretControl);

      turrets[(int)turretControl].transform.Rotate(0, 0, TurretRotationSpeed * Time.deltaTime);
    }

    private void outlineTurret(TurretControl newTurretControl)
    {
      for (var i = 0; i < turretOutlines.Count; i++)
      {
        turretOutlines[i].outlineSize = i == (int)newTurretControl ? OutlineWidth : 0;
      }
    }

    private void OnDisable()
    {
      unsubscribeToEvents();
    }
    private void subscribeToEvents()
    {
      gameManager.ButtonEvents.OnUpButtonPressed += HandleUpButtonPressed;
      gameManager.ButtonEvents.OnDownButtonPressed += HandleDownButtonPressed;
      gameManager.ButtonEvents.OnActionButtonPressed += HandleActionButtonPressed;
      gameManager.ButtonEvents.OnHorizontalPressed += HandleHorizontalPressed;
    }
    private void unsubscribeToEvents()
    {
      gameManager.ButtonEvents.OnUpButtonPressed -= HandleUpButtonPressed;
      gameManager.ButtonEvents.OnDownButtonPressed -= HandleDownButtonPressed;
      gameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButtonPressed;
      gameManager.ButtonEvents.OnHorizontalPressed -= HandleHorizontalPressed;
    }
    private void HandleUpButtonPressed()
    {
      changeCurrentColor(1);
    }

    private void HandleDownButtonPressed()
    {
      changeCurrentColor(-1);
    }

    private void HandleActionButtonPressed()
    {
      var laser = Instantiate(LaserPrefab, gameManager.transform);
      laser.transform.position = turrets[(int)turretControl].transform.position;
      laser.GetComponent<Rigidbody2D>().AddForce(
          Vector2.up * LaserSpeed * Time.deltaTime);

      laser.GetComponent<LineRenderer>().startColor = colorList[currentColorIndex];
      laser.GetComponent<LineRenderer>().endColor = colorList[currentColorIndex];

      Destroy(laser, 3.0f);
    }

    /// <summary>
    ///
    /// </summary>
    /// 1 forward, -1 back
    private void changeCurrentColor(int direction)
    {
      switch (direction)
      {
        case 1:
          {
            var nextColorIndex = currentColorIndex + 1;
            currentColorIndex = nextColorIndex == colorList.Count ? 0 : nextColorIndex;

            break;
          }
        case -1:
          {
            var previousColorIndex = currentColorIndex - 1;
            if (previousColorIndex < 0)
            {
              currentColorIndex = colorList.Count - 1;
            }
            else
            {
              currentColorIndex = previousColorIndex;
            }

            break;
          }
      }

      foreach (var item in turrets)
      {
        item.GetComponent<SpriteRenderer>().color = colorList[currentColorIndex];
      }
    }

    private void HandleHorizontalPressed(InputValue inputValue)
    {
      switch (inputValue.Get<float>())
      {
        case -1:
          turretControl = TurretControl.Left;
          break;
        case 0:
          turretControl = TurretControl.Center;
          break;
        case 1:
          turretControl = TurretControl.Right;
          break;
      }

      outlineTurret(turretControl);
    }
  }
}
