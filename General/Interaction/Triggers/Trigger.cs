using System;
using UnityEngine;

namespace Devdog.General
{
    public partial class Trigger : TriggerBase
    {
        [Header("Animations & Audio")]
        public MotionInfo useAnimation = new MotionInfo();
        public MotionInfo unUseAnimation = new MotionInfo();

        public AudioClipInfo useAudioClip = new AudioClipInfo();
        public AudioClipInfo unUseAudioClip = new AudioClipInfo();

        protected Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected virtual void WindowOnHide()
        {
            Server_UnUse(PlayerManager.instance.currentPlayer);
        }

        protected virtual void WindowOnShow()
        {

        }

        public override void DoVisuals()
        {
            if (useAnimation.motion != null && animator != null)
            {
                animator.Play(useAnimation);
            }

            AudioManager.AudioPlayOneShot(useAudioClip);
        }

        public override void UndoVisuals()
        {
            if (unUseAnimation.motion != null && animator != null)
            {
                animator.Play(unUseAnimation);
            }

            AudioManager.AudioPlayOneShot(unUseAudioClip);
        }

        public override bool Use(Character character)
        {
            if (CanUse(character) == false)
            {
                return false;
            }

            if (this.Equals(character.currentTrigger))
            {
                return true;
            }

            Server_Use(character);
            return true;
        }

        public override void Server_Use(Character character)
        {
            DoVisuals();
            NotifyTriggerUsed(character);
        }

        /// <summary>
        /// Force use this trigger (ignores range and other conditions), and will not set a state (it won't set this as the active trigger),
        /// also UI elements won't be shown (windows)
        /// 
        /// <remarks>This method can be useful when you want to let a NPC or something other than the player use a trigger.</remarks>
        /// </summary>
        /// <param name="character">The object that used this trigger, null if not used by an object.</param>
        public virtual void ForceUseWithoutStateAndUI(Character character)
        {
            DoVisuals();
            NotifyTriggerUsed(character);
        }

        public override bool UnUse(Character character)
        {
            if (CanUnUse(character) == false)
            {
                return false;
            }

            Server_UnUse(character);
            return true;
        }

        public override void Server_UnUse(Character character)
        {
            UndoVisuals();
            NotifyTriggerUnUsed(character);
        }

        public virtual void ForceUnUseWithoutStateAndUI(Character character)
        {
            UndoVisuals();
            NotifyTriggerUnUsed(character);
        }
    }
}