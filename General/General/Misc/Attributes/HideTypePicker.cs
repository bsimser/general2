using System;
using UnityEngine;

namespace Devdog.General
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HideTypePicker : Attribute
    {

        public HideTypePicker()
        {
        }
    }
}
