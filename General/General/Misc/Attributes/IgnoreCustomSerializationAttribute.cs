using System;
using System.Collections.Generic;

namespace Devdog.General
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreCustomSerializationAttribute : Attribute
    {

    }
}
