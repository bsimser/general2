using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Devdog.General2
{
    public partial class TriggerRangeHandler2D : MonoBehaviour, ITriggerRangeHandler
    {
        [SerializeField]
        private float _useRange = 10f;
        public float useRange
        {
            get { return _useRange; }
        }

        private readonly List<Character> _charactersInRange = new List<Character>();
        private CircleCollider2D _circleCollider;
        private Rigidbody2D _rigidbody2D;
        private TriggerBase _trigger;

        protected void Awake()
        {
            _trigger = GetComponentInParent<TriggerBase>();
            Assert.IsNotNull(_trigger, "TriggerRangeHandler used but no TriggerBase found on object.");

            _rigidbody2D = gameObject.GetOrAddComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;

            _circleCollider = gameObject.GetOrAddComponent<CircleCollider2D>();
            _circleCollider.isTrigger = true;
            _circleCollider.radius = useRange;

            gameObject.layer = 2; // Ignore raycasts
        }

        public IEnumerable<Character> GetCharactersInRange()
        {
            return _charactersInRange;
        }

        public bool IsCharacterInRange(Character character)
        {
            return _charactersInRange.Contains(character);
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player2D>();
            if (player != null)
            {
                _charactersInRange.Add(player);
//                _trigger.NotifyCameInRange(player);
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player2D>();
            if (player != null)
            {
                _charactersInRange.Remove(player);
                _trigger.UnUse(player);
            }
        }
    }
}