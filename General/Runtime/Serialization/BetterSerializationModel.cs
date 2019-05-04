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

        public void Save<T>(ref List<UnityEngine.Object> objectReferences, ref byte[] json, T obj)
            where T: UnityEngine.Object
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

        public void Load<T>(ref List<UnityEngine.Object> objectReferences, ref byte[] json, T deserializeTo)
            where T: UnityEngine.Object
        {
            if (_isSerializing)
                return;

            _isSerializing = true;
            JsonSerializer.DeserializeObject(deserializeTo, json, objectReferences);
            _isSerializing = false;
        }
    }
}
