using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jagerwil.EventServiceTT {
    public class EventService : MonoBehaviour {
        [SerializeField] private BaseServerWrapper _serverWrapper;
        [SerializeField] private string _fileName;
        [SerializeField] private string _serverUrl; //It would be better to move Url into ServerWrapper
        [SerializeField] private float _cooldownBeforeSend = 1f;

        private EventServiceData _data;

        private WaitForSeconds _delayYield;
        private Coroutine _sendEventsCoro;

        private void Awake() {
            _delayYield = new WaitForSeconds(_cooldownBeforeSend);
        }

        private void Start() {
            //TODO: Replace Load with LoadAsync to make game start faster 
            var saveLoadSystem = SaveLoadSystem.Instance;
            if (saveLoadSystem)
                _data = saveLoadSystem.Load<EventServiceData>(_fileName);

            if (_data == null)
                return;

            _serverWrapper.VerifyEvents(_serverUrl, _data.events, (result) => {
                foreach (var eventInfo in result) {
                    _data.pendingEvents.Remove(eventInfo.evt);
                    if (!eventInfo.wasReceived) {
                        _data.events.Add(eventInfo.evt);
                    }
                }
                SendEvents();
            });
        }

        public void TrackEvent(string type, string data) {
            if (_data == null)
                return;

            _data.events.Add(new EventData(type, data));
            SaveData();
            
            if (_sendEventsCoro == null) {
                _sendEventsCoro = StartCoroutine(SendEventsAfterDelay());
            }
        }

        IEnumerator SendEventsAfterDelay() {
            yield return _delayYield;
            SendEvents();
            _sendEventsCoro = null;
        }

        private void SendEvents() {
            var events = new List<EventData>(_data.events);
            _data.events.Clear();
            _data.pendingEvents.AddRange(events);
            SaveData();

            _serverWrapper.SendEvents(_serverUrl, events, (success) => {
                if (!success) {
                    _data.events.AddRange(events);
                    SaveData();
                }
            });
        }

        private void SaveData() {
            var saveLoadSystem = SaveLoadSystem.Instance;
            if (saveLoadSystem)
                saveLoadSystem.Save(_data, _fileName);
        }
    }

    [Serializable]
    public class EventServiceData {
        public List<EventData> events = new();
        public List<EventData> pendingEvents = new();
    }

    [Serializable]
    public class EventData {
        public string type;
        public string data;

        public EventData(string type, string data) {
            this.type = type;
            this.data = data;
        }
    }
}

