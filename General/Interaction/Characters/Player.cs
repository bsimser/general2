using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General
{
    [DisallowMultipleComponent]
    public partial class Player : Character
    {
        [SerializeField]
        private bool _initPlayerOnAwake = true;
        
        public BestTriggerSelectorBase triggerSelector;
        
        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private readonly List<IPlayerInputCallbacks> _inputCallbacksCache = new List<IPlayerInputCallbacks>(); 
        protected override void Awake()
        {
            base.Awake();
            
            if (_initPlayerOnAwake)
            {
                RegisterPlayerAsCurrentPlayer();
            }
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(SelectBestTrigger());
            InputManager.OnPlayerInputChanged += OnPlayerInputChanged;
        }

        private IEnumerator SelectBestTrigger()
        {
            yield return _waitForSeconds;
            if (triggerSelector != null)
            {
                triggerSelector.GetBestTrigger(this, triggersInRange);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            InputManager.OnPlayerInputChanged -= OnPlayerInputChanged;
        }

        public void RegisterPlayerAsCurrentPlayer()
        {
            PlayerManager.instance.currentPlayer = this;
        }
        
        protected virtual void OnPlayerInputChanged(bool isInputLimited)
        {
            GetComponentsInChildren<IPlayerInputCallbacks>(true, _inputCallbacksCache);
            foreach (var c in _inputCallbacksCache)
            {
                c.SetInputActive(!isInputLimited);
            }
        }
    }
}