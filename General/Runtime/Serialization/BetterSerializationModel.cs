using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Devdog.General2
{
    public class BetterSerializationModel
    {
        [NonSerialized]
        private bool _isSerializing = false;

        public void Save(ref List<UnityEngine.Object> objectReferences, ref byte[] json, UnityEngine.Object obj)
        {
            if (_isSerializing || Application.isPlaying)
                return;

            _isSerializing = true;
            objectReferences = new List<UnityEngine.Object>(); // Has to be new list, ref type -> Clear will clear it inside the serializer
            json = JsonSerializer.SerializeObject(obj, objectReferences);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
#endif

            _isSerializing = false;
        }

        public void Load(ref List<UnityEngine.Object> objectReferences, ref byte[] json, UnityEngine.Object obj)
        {
            if (_isSerializing)
                return;

            _isSerializing = true;
            JsonSerializer.DeserializeObject(obj, json, objectReferences);
            _isSerializing = false;
        }
    }
}
