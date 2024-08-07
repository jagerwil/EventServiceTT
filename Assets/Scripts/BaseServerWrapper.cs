using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jagerwil.EventServiceTT {
    public abstract class BaseServerWrapper : MonoBehaviour {
        public abstract void VerifyEvents(string url, List<EventData> events, Action<List<EventVerificationData>> callback);
        public abstract void SendEvents(string url, List<EventData> events, Action<bool> callback);

        public class EventVerificationData {
            public EventData evt;
            public bool wasReceived;

            public EventVerificationData(EventData evt, bool wasReceived) {
                this.evt = evt;
                this.wasReceived = wasReceived;
            }
        }
    }
}
