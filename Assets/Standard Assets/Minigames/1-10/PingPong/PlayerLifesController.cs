using Components;
using UnityEngine;

namespace Minigames.PingPong
{
  public class PlayerLifesController : MonoBehaviour
  {
    public GameObject[] Lifes;

    private Lifes lifes;
    private MinigameManager gameManager;
    private void Start()
    {
      gameManager = GetComponentInParent<MinigameManager>();
      lifes = new Lifes(Lifes);

      subscribeToEvents();
    }

    private void OnDisable()
    {
      unsubscribeToEvents();
    }

    private void subscribeToEvents()
    {
      gameManager.Events.OnHit += HandleHit;
    }

    private void unsubscribeToEvents()
    {
      gameManager.Events.OnHit -= HandleHit;
    }

    private void HandleHit()
    {
      if (lifes.LoseLife())
      {
        gameManager.Events.EventDeath();
      }
    }
  }
}
