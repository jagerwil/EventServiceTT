using JetBrains.Annotations;
using UnityEngine;

namespace Jagerwil.Core {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static T _instance;

        [CanBeNull]
        public static T Instance {
            get {
                if (!_instance) {
                    _instance = FindObjectOfType<T>();
                }
                if (!_instance) {
                    Debug.LogError($"{nameof(T)}: {nameof(Instance)} is not found!");
                }
                return _instance;
            }
        }

        private void Awake() {
            if (_instance && _instance != this) {
                Destroy(this);
                return;
            }

            _instance = (T)this;
        }
    }
}

