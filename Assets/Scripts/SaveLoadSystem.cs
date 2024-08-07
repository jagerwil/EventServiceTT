using Jagerwil.Core;
using JetBrains.Annotations;

namespace Jagerwil.EventServiceTT {
    public class SaveLoadSystem : Singleton<SaveLoadSystem> {
        public void Save<T>(T obj, string filename) {
            //Saving logic
        }

        [CanBeNull]
        public T Load<T>(string filename) {
            //Loading logic
            return default;
        }
    }
}
