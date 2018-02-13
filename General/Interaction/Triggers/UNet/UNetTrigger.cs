﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Devdog.General
{
    public class UNetTrigger : NetworkBehaviour, ITrigger
    {
        private List<Character> _usingCharacters = new List<Character>();
        private static readonly List<ITriggerCallbacks> _callbacks = new List<ITriggerCallbacks>();

        private NetworkIdentity _identity;
        public NetworkIdentity identity
        {
            get
            {
                if (_identity == null)
                {
                    _identity = GetComponent<NetworkIdentity>();
                }

                return _identity;
            }
        }

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

        protected virtual void OnDestroy()
        {
            for (int i = _usingCharacters.Count - 1; i >= 0; i--)
            {
                // Try to un-use the collection. If not the server will have to clean up on disconnect.
                UnUse(_usingCharacters[i]);

                Destroy(GetComponent<ITriggerInputHandler>() as UnityEngine.Component);
                Destroy(GetComponent<ITriggerRangeHandler>() as UnityEngine.Component);
            }
        }
        
        public bool Toggle(Character character)
        {
            if (_usingCharacters.Contains(character))
            {
                return UnUse(character);
            }

            return Use(character);
        }

        public bool CanUse(Character character)
        {
            if (rangeHandler.IsCharacterInRange(character) == false)
            {
                return false;
            }
            
            // Avoid double using...
            if (_usingCharacters.Contains(character))
            {
                return false;
            }

            return true;
        }

        public virtual bool Use(Character character)
        {
            if (CanUse(character) == false)
            {
                return false;
            }

            if (isClient)
            {
                var bridge = character.GetComponent<UNetGeneralActionsBridge>();
                if (bridge != null)
                {
                    bridge.Cmd_RequestUseTrigger(new RequestUseTriggerMessage()
                    {
                        triggerIdentity = identity
                    });
                }
            }
            else if (isServer)
            {
                Server_Use(character);
            }

            return true;
        }

        public bool CanUnUse(Character character)
        {
            return _usingCharacters.Contains(character);
        }

        public bool UnUse(Character character)
        {
            if (CanUnUse(character) == false)
            {
                return false;
            }

            if (isClient)
            {
                var bridge = character.GetComponent<UNetGeneralActionsBridge>();
                if (bridge != null)
                {
                    bridge.Cmd_RequestUnUseTrigger(new RequestUseTriggerMessage()
                    {
                        triggerIdentity = identity
                    });
                }
            }
            else if (isServer)
            {
                Server_UnUse(character);
            }

            return true;
        }

        [Server]
        public virtual void Server_Use(Character character)
        {
            if (character.currentTrigger != null)
            {
                // If this player is already using a collection unuse it and use this one.
                // NOTE: ForceUnUse could unuse a trigger that can't be un-used... But calling UnUse() could fail
                character.currentTrigger.Server_UnUse(character);
            }

            NotifyTriggerUsed(character);
        }

        [Server]
        public virtual void Server_UnUse(Character character)
        {
            NotifyTriggerUnUsed(character);
        }

        private IEnumerable<ITriggerCallbacks> GetCallbacks()
        {
            GetComponents<ITriggerCallbacks>(_callbacks);
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

                yield return callback;
            }
        }

        /// <summary>
        /// Only the first active and enabled component gets the callbacks. In order for the next component to receive the callbacks the first one has to be disabled or removed.
        /// </summary>
        public virtual void NotifyTriggerUsed(Character character)
        {
            _usingCharacters.Add(character);
            character.currentTrigger = this;
            var data = new TriggerEventData();
            foreach (var callback in GetCallbacks())
            {
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
        public virtual void NotifyTriggerUnUsed(Character character)
        {
            _usingCharacters.Remove(character);
            character.currentTrigger = null;
            var data = new TriggerEventData();
            foreach (var callback in GetCallbacks())
            {
                callback.OnTriggerUnUsed(character, data);
                if (data.used)
                {
                    break;
                }
            }
        }
    }
}