using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Jagerwil.EventServiceTT {
    public class ServerWrapperPlaceholder : BaseServerWrapper {
        //Should sent the request to check the events, but there is no server, so...
        //Also we cannot reliably verify whether event was sent or not cuz we need something akin to event ID
        public override void VerifyEvents(string url, List<EventData> events, Action<List<EventVerificationData>> callback) {
            var list = new List<EventVerificationData>(events.Count);
            foreach (var evt in events) {
                list.Add(new EventVerificationData(evt, true));
            }
            callback?.Invoke(list);
        }

        //Should sent events to the server, but there is no server, so...
        public override void SendEvents(string url, List<EventData> events, Action<bool> callback) {
            var jsonString = JsonUtility.ToJson(new EventServerData(events));
            StartCoroutine(SendEventsCoro(url, jsonString, callback));
        }

        private IEnumerator SendEventsCoro(string url, string content, Action<bool> callback) {
            var request = UnityWebRequest.Post(url, content);
            yield return request.SendWebRequest();
            callback?.Invoke(request.result == UnityWebRequest.Result.Success);
        }

        private class EventServerData {
            public List<EventData> events;

            public EventServerData(List<EventData> events) {
                this.events = events;
            }
        }
    }
}
