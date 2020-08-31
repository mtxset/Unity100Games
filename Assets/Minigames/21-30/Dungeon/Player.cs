using Components;
using UnityEngine;

namespace Minigames.Dungeon
{
    public class Player : AddMinigameManager
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
            this.animator = this.GetComponent<Animator>();
            this.MinigameManager.Events.OnDeath += HandleDeath;
            
            this.MinigameManager.ButtonEvents.OnRightButtonPressed += HandleAction;
            this.MinigameManager.ButtonEvents.OnUpButtonPressed += HandleJump;
            this.MinigameManager.ButtonEvents.OnLeftButtonPressed += HandleRoll;
        }

        private void OnDisable()
        {
            this.MinigameManager.Events.OnDeath -= HandleDeath;

            this.MinigameManager.ButtonEvents.OnRightButtonPressed -= HandleAction;
            this.MinigameManager.ButtonEvents.OnUpButtonPressed -= HandleJump;
            this.MinigameManager.ButtonEvents.OnLeftButtonPressed -= HandleRoll;
        }

        private void HandleRoll()
        {
            if (this.MinigameManager.GameOver || !this.canDoWeSomething)
            {
                return;
            }
            
            this.Sounds.SoundPlayerRoll.Play();
            this.animator.SetTrigger(PlayerRoll);
            this.startCooldown();
        }

        private void HandleJump()
        {
            if (this.MinigameManager.GameOver || !this.canDoWeSomething)
            {
                return;
            }
            
            this.Sounds.SoundPlayerJump.Play();
            this.animator.SetTrigger(PlayerJump);
            this.startCooldown();
        }

        private void HandleDeath()
        {
            this.animator.SetBool(PlayerDied, true);
        }

        private void HandleAction()
        {
            if (this.MinigameManager.GameOver || !this.canDoWeSomething)
            {
                return;
            }
            this.raiseShield();
            this.startCooldown();
        }

        private void startCooldown()
        {
            this.canDoWeSomething = true;
            StartCoroutine(Delay.StartDelay(
                    this.ActionCooldown, this.resetCooldown, null));

        }

        private void resetCooldown()
        {
            this.canDoWeSomething = true;
            this.lowerShield();
        }

        private void lowerShield()
        {
            this.shielded = false;
            this.animator.SetBool(PlayerShield, shielded);
        }
        private void raiseShield()
        {
            this.shielded = true;
            
            this.animator.SetBool(PlayerShield, shielded);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (this.MinigameManager.GameOver)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("hit"))
            {
                // Knight
                if (!this.shielded)
                {
                    playerGotHit();
                }
                else
                {
                    this.MinigameManager.Events.EventDodged();
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
                this.Sounds.SoundFireBallHit.Play();
                
                playerGotHit();
                
                Destroy(other.gameObject);
            }
        }

        private void playerGotHit()
        {
            this.MinigameManager.Events.EventHit();
            this.animator.SetTrigger(PlayerHit);
        }
    }
}