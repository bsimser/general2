using System;
using UnityEngine;

namespace Devdog.General
{
    [CreateAssetMenu(menuName = "Devdog/General Settings")]
    public class GeneralSettings : ScriptableObject
    {
        [Header("Cursor")]
        [Category("Cursor")]
        public CursorIcon defaultCursor;

        [Header("Logging")]
        [Category("Logging")]
        public DevdogLogger.LogType minimalLogType;

        [Category("Editor & Testing")]
        [Header("Testing")]
        public bool useExceptionsForAssertions = false;
    }
}
