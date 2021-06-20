using System.Collections.Generic;
using Components;
using UnityEngine;
using Utils;

namespace Minigames.DoubleTrouble {

    public class Guitar: AddMinigameManager2 {
        // Order is Sounds important (low E, A, D, G, B, high E)
        public AudioClip[] Sounds;
        public GameObject[] Strings;
        public Camera CurrentCamera;
        public GameObject Pick;
        public GameObject SoundAttacker;

        public float AmplitudeOffset = 0.2f;
        public float SpeedOffset = 0.1f;
        public float PickJumpRate = 0.1f;
        public float AttackerPushBack = 0.5f;
        public Vector2 AttackerSpeedMinMax;
        public Vector2 PickJumprateMinMax;
        public float AttackerCurrentSpeed = 0;

        private GString[] strings;
        private AudioSource audioSource;

        private float jumpNextString;
        private float offset;
        private int pickPositionIndex = 0;
        private int attackerPositionIndex = 0;

        private void Start() {

            AttackerCurrentSpeed = AttackerSpeedMinMax.x;
            audioSource = GetComponent<AudioSource>();

            strings = new GString[Strings.Length];
            var h = CurrentCamera.orthographicSize;

            offset = h / (Strings.Length + 1);
            var current_offset = offset;
            for (var i = 0; i < Strings.Length; i++) {
                Strings[i].transform.position = new Vector2(Strings[i].transform.position.x, -current_offset);
                current_offset += offset;

                strings[i] = Strings[i].GetComponent<GString>();
                strings[i].Amplitude += AmplitudeOffset * i;
                strings[i].Speed -= SpeedOffset * i;
            }

            attackerJump();

            MinigameManager.ButtonEvents.OnActionButtonPressed += HandleActionButton;
        }

        private void OnDisable() {
            MinigameManager.ButtonEvents.OnActionButtonPressed -= HandleActionButton;
        }

        private void HandleActionButton() => PlayStringWherePick();

        private void FixedUpdate() {
            if ((jumpNextString += Time.fixedDeltaTime) > PickJumpRate) {
                jumpNextString = 0f;
                if (++pickPositionIndex == Strings.Length)
                    pickPositionIndex = 0;

                Pick.transform.position = new Vector3(Pick.transform.position.x, strings[pickPositionIndex].transform.position.y, 0);
            }

            var vectorList = new List<Vector2> {
                // reverse because it's time and we need it to speed up so it should go lower and lower
                new Vector2(PickJumprateMinMax.y, PickJumprateMinMax.x)
            };

            PickJumpRate = DifficultyAdjuster.SpreadDifficulty(MinigameManager.DiffCurrent, vectorList)[0];

            SoundAttacker.transform.position -= new Vector3(0.1f * AttackerCurrentSpeed, 0, 0);
        }

        public void PlayStringWherePick() {
            checkPickVsAttacker();
            var randomClipIndex = Random.Range(0, Sounds.Length - 1);
            audioSource.PlayOneShot(Sounds[randomClipIndex]);
            strings[pickPositionIndex].Play();
        }

        public void PlayString(int index) {
            checkPickVsAttacker();
            var randomClipIndex = Random.Range(0, Sounds.Length - 1);
            audioSource.PlayOneShot(Sounds[randomClipIndex]);
            strings[index].Play();
        }

        private void checkPickVsAttacker() {
            if (attackerPositionIndex == pickPositionIndex) {
                SoundAttacker.transform.position += new Vector3(AttackerPushBack, 0, 0);
                MinigameManager.Events.EventScored();
            } else {
                MinigameManager.Events.EventHit();
            }

            attackerJump();
        }

        private void attackerJump() {
            var oldPos = attackerPositionIndex;

            do {
                attackerPositionIndex = Random.Range(0, Strings.Length);
            } while (attackerPositionIndex == oldPos);

            SoundAttacker.transform.position = new Vector3(SoundAttacker.transform.position.x, Strings[attackerPositionIndex].transform.position.y, 0);
        }
    }
}
