using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General
{
    public abstract class BestTriggerSelectorBase : ScriptableObject
    {
        public abstract ITrigger GetBestTrigger(Character character, IEnumerable<ITrigger> triggersInRange);
    }
}
