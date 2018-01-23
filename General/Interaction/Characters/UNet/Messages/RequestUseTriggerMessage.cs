using UnityEngine.Networking;

namespace Devdog.General
{
    public sealed class RequestUseTriggerMessage : MessageBase
    {
//        public byte[] triggerGuidBytes;
        public NetworkIdentity triggerIdentity;
    }
}