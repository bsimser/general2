using System;
using System.Collections.Generic;
using Devdog.General2.Serialization;

namespace Devdog.General2
{
    public static class JsonSerializer
    {
        private static SerializationConfig _config;

        static JsonSerializer()
        {
            _config = new SerializationConfig()
            {
                SerializationPolicy = new CustomSerializationPolicy("Devdog_serialization_policy", true, info =>
                {
                    foreach (var attr in info.CustomAttributes)
                    {
                        if (attr.AttributeType == typeof(CustomSerializationAttribute))
                        {
                            return true;
                        }
                            
                        if (attr.AttributeType == typeof(IgnoreCustomSerializationAttribute))
                        {
                            return false;
                        }
                    }
                        
                    return false;
                })
            };
        }

        public static byte[] SerializeObject<T>(T value, List<UnityEngine.Object> objectReferences)
            where T: UnityEngine.Object
        {
            SerializationData data = new SerializationData()
            {
                SerializedFormat = DataFormat.JSON,
                ReferencedUnityObjects = objectReferences,
            };
            UnitySerializationUtility.SerializeUnityObject(value, ref data, false, new SerializationContext()
            {
                Config = _config,
            });

            return data.SerializedBytes;
        }
        
        public static void DeserializeObject<T>(T value, byte[] json, List<UnityEngine.Object> objectReferences)
            where T: UnityEngine.Object
        {
            if (json == null || json.Length == 0)
            {
                return;
            }
            
            UnitySerializationUtility.DeserializeUnityObject(value, ref json, ref objectReferences, DataFormat.JSON, new DeserializationContext()
            {
                Config = _config
            });
        }
        
        public static byte[] Serialize<T>(T value, List<UnityEngine.Object> objectReferences)
        {
            return SerializationUtility.SerializeValue(value, DataFormat.JSON, out objectReferences, new SerializationContext()
            {
                Config = _config
            });
        }
        
        public static byte[] Serialize(object value, List<UnityEngine.Object> objectReferences)
        {
            return SerializationUtility.SerializeValue(value, DataFormat.JSON, out objectReferences, new SerializationContext()
            {
                Config = _config
            });
        }

        public static T Deserialize<T>(byte[] json, List<UnityEngine.Object> objectReferences)
        {
            if (json == null || json.Length == 0)
            {
                return default(T);
            }
            
            return SerializationUtility.DeserializeValue<T>(json, DataFormat.JSON, objectReferences, new DeserializationContext()
            {
                Config = _config
            });
        }
        
        public static object Deserialize(byte[] json, List<UnityEngine.Object> objectReferences)
        {
            if (json == null || json.Length == 0)
            {
                return null;
            }
            
            return SerializationUtility.DeserializeValueWeak(json, DataFormat.JSON, objectReferences);
        }
    }
}