using Components;
using UnityEngine;

namespace Minigames.Dungeon
{
    public class Player : AddMinigameManager2
    {
        public float ActionCooldown = 1f;
        public Sounds Sounds;
        
        private static readonly int SwordDies = Animator.StringToHash("swordDies");
        
        private Animator animator;

        private bool canDoWeSomething = true;

        private bool shielded;
        private static readonly int PlayerDied = Animator.StringToHash("playerDied");
        private static readonly int PlayerShield = Animator.StringToHash("playerShield");
        private static readonly int PlayerHit = Animator.StringToHash("playerHit");
        private static readonly int PlayerJump = Animator.StringToHash("playerJump");
        private static readonly int PlayerRoll = Animator.StringToHash("playerRoll");

        private void Start()
        {
            animator = GetComponent<Animator>();
            MinigameManager.Events.OnDeath += HandleDeath;
            
            MinigameManager.ButtonEvents.OnRightButtonPressed += HandleAction;
            MinigameManager.ButtonEvents.OnUpButtonPressed += HandleJump;
            MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleRoll;
        }

        private void OnDisable()
        {
            MinigameManager.Events.OnDeath -= HandleDeath;

            MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleAction;
            MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleJump;
            MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleRoll;
        }

        private void HandleRoll()
        {
            if (MinigameManager.GameOver || !canDoWeSomething)
            {
                return;
            }
            
            Sounds.SoundPlayerRoll.Play();
            animator.SetTrigger(PlayerRoll);
            startCooldown();
        }

        private void HandleJump()
        {
            if (MinigameManager.GameOver || !canDoWeSomething)
            {
                return;
            }
            
            Sounds.SoundPlayerJump.Play();
            animator.SetTrigger(PlayerJump);
            startCooldown();
        }

        private void HandleDeath()
        {
            animator.SetBool(PlayerDied, true);
        }

        private void HandleAction()
        {
            if (MinigameManager.GameOver || !canDoWeSomething)
            {
                return;
            }
            raiseShield();
            startCooldown();
        }

        private void startCooldown()
        {
            canDoWeSomething = true;
            StartCoroutine(Delay.StartDelay(
                    ActionCooldown, resetCooldown, null));

        }

        private void resetCooldown()
        {
            canDoWeSomething = true;
            lowerShield();
        }

        private void lowerShield()
        {
            shielded = false;
            animator.SetBool(PlayerShield, shielded);
        }
        private void raiseShield()
        {
            shielded = true;
            
            animator.SetBool(PlayerShield, shielded);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (MinigameManager.GameOver)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("hit"))
            {
                // Knight
                if (!shielded)
                {
                    playerGotHit();
                }
                else
                {
                    MinigameManager.Events.EventDodged();
                }
            }
            else if (other.gameObject.CompareTag("deadzone"))
            {
                // Skeleton
                playerGotHit();
                
                other.gameObject.GetComponent<Animator>().SetTrigger(SwordDies);
                Destroy(other.gameObject, 0.3f);
            }
            else if (other.gameObject.CompareTag("Barrel"))
            {
                // Mage
                Sounds.SoundFireBallHit.Play();
                
                playerGotHit();
                
                Destroy(other.gameObject);
            }
        }

        private void playerGotHit()
        {
            MinigameManager.Events.EventHit();
            animator.SetTrigger(PlayerHit);
        }
    }
}