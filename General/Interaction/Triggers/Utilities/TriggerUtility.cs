﻿using System;
using UnityEngine;

namespace Devdog.General
{
    public static class TriggerUtility
    {
        public static ITrigger mouseOnTrigger { get; set; }
        public static bool isMouseOnTrigger
        {
            get { return mouseOnTrigger != null; }
        }
    }
}
