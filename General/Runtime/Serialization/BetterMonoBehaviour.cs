using System;
using System.Collections.Generic;
using Devdog.General2.Serialization;
using UnityEngine;

namespace Devdog.General2
{
    public class BetterMonoBehaviour : SerializedMonoBehaviour
    {
        /// <summary>
        /// Used to store the state of this scriptable object. 
        /// Fetched through reflection to avoid people messing with it.
        /// </summary>
        [SerializeField]
        [IgnoreCustomSerialization] // Ignore in custom serializer - Let unity seriarlize this.
        private string _serializedJsonString = "{}";

        [SerializeField]
        [IgnoreCustomSerialization] // Ignore in custom serializer - Let unity seriarlize this.
        private List<UnityEngine.Object> _objectReferences;

        [NonSerialized]
        private readonly BetterSerializationModel _serializer = new BetterSerializationModel();

        public virtual void Load()
        {
            _serializer.Load(ref _objectReferences, ref _serializedJsonString, this);
        }
        protected override void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(_serializedJsonString) == false && _serializedJsonString.Replace(" ", "") != "{}")
            {
                Load();
                _serializedJsonString = "";
                _objectReferences = new List<UnityEngine.Object>();
            }
        }
    }
}
