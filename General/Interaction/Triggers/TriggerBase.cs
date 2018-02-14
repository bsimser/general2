using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General2
{
    [DisallowMultipleComponent]
    public abstract class TriggerBase : MonoBehaviour, ITrigger // , IMouseCallbacks
    {
        public bool useWhenPlayerComesInRange = false;
        public bool blockPlayerInput = false;

        private ITriggerRangeHandler _rangeHandler;
        public ITriggerRangeHandler rangeHandler
        {
            get
            {
                if (_rangeHandler == null || _rangeHandler.Equals(null))
                {
                    _rangeHandler = GetComponentInChildren<ITriggerRangeHandler>();
                }

                return _rangeHandler;
            }
            set { _rangeHandler = value; }
        }

        private static List<ITriggerCallbacks> _callbacks = new List<ITriggerCallbacks>();
        protected virtual void Awake()
        {

        }

        protected virtual void OnValidate()
        {
            if (rangeHandler == null)
            {
                DevdogLogger.LogWarning("Trigger needs an ITriggerRangeHandler");
            }
        }

        protected virtual void OnDestroy()
        {
            UnUse();

            Destroy(GetComponent<ITriggerInputHandler>() as UnityEngine.Component);
            Destroy(GetComponent<ITriggerRangeHandler>() as UnityEngine.Component);
        }

        public virtual bool Toggle()
        {
            return Toggle(PlayerManager.currentPlayer);
        }

        public virtual bool Toggle(Character character)
        {
            if (this.Equals(character.currentTrigger))
            {
                return UnUse(character);
            }

            return Use(character);
        }

        public bool CanUse()
        {
            return CanUse(PlayerManager.currentPlayer);
        }

        public virtual bool CanUse(Character character)
        {
            if (enabled == false || rangeHandler.IsCharacterInRange(character) == false)
                return false;

            if (this.Equals(character.currentTrigger))
                return true;

            if (InputManager.CanReceiveUIInput(gameObject) == false)
                return false;

            return true;
        }

        public bool CanUnUse()
        {
            return CanUnUse(PlayerManager.currentPlayer);
        }

        public abstract void Server_Use(Character character);

        public virtual bool CanUnUse(Character character)
        {
            if (enabled == false || rangeHandler.IsCharacterInRange(character) == false)
                return false;

            if (this.Equals(character.currentTrigger) == false)
                return false;

            if (InputManager.CanReceiveUIInput(gameObject) == false)
                return false;

            return true;
        }


        public bool Use()
        {
            return Use(PlayerManager.currentPlayer);
        }

        public abstract bool Use(Character character);

        public bool UnUse()
        {
            return UnUse(PlayerManager.currentPlayer);
        }

        public abstract bool UnUse(Character character);
        public abstract void Server_UnUse(Character character);


        public abstract void DoVisuals();
        public abstract void UndoVisuals();

        private void UpdateTriggerCallbacks()
        {
            GetComponents<ITriggerCallbacks>(_callbacks);
        }

        /// <summary>
        /// Only the first active and enabled component gets the callbacks. In order for the next component to receive the callbacks the first one has to be disabled or removed.
        /// </summary>
        protected virtual void NotifyTriggerUsed(Character character)
        {
            if (blockPlayerInput)
            {
                InputManager.LimitPlayerInputTo(gameObject);
            }

            UpdateTriggerCallbacks();
            var data = new TriggerEventData();
            for (int i = 0; i < _callbacks.Count; i++)
            {
                var callback = _callbacks[i];
                var callbackBehaviour = callback as Behaviour;
                if (callbackBehaviour != null)
                {
                    if (callbackBehaviour.isActiveAndEnabled == false)
                    {
                        continue;
                    }
                }

                callback.OnTriggerUsed(character, data);
                if (data.used)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Only the first active and enabled component gets the callbacks. In order for the next component to receive the callbacks the first one has to be disabled or removed.
        /// </summary>
        protected virtual void NotifyTriggerUnUsed(Character character)
        {
            if (blockPlayerInput)
            {
                InputManager.RemovePlayerLimitInput(gameObject);
            }

            UpdateTriggerCallbacks();
            var data = new TriggerEventData();
            for (int i = 0; i < _callbacks.Count; i++)
            {
                var callback = _callbacks[i];
                var callbackBehaviour = callback as Behaviour;
                if (callbackBehaviour != null)
                {
                    if (callbackBehaviour.isActiveAndEnabled == false)
                    {
                        continue;
                    }
                }

                callback.OnTriggerUnUsed(character, data);
                if (data.used)
                {
                    break;
                }
            }
        }
    }
}
