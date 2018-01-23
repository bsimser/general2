using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General
{
    public interface ITriggerRangeHandler
    {
        float useRange { get; }
        
        IEnumerable<Character> GetCharactersInRange();
        bool IsCharacterInRange(Character character);
    }
}
