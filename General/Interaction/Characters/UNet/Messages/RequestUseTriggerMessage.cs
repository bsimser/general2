using UnityEngine.Networking;

namespace Devdog.General2
{
    public sealed class RequestUseTriggerMessage : MessageBase
    {
//        public byte[] triggerGuidBytes;
        public NetworkIdentity triggerIdentity;
    }
}