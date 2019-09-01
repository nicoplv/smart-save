using System;
using UnityEngine;

namespace SmartSaves
{
    public class Data<T> : ScriptableObject where T : Data<T>
    {
        #region Variables

        private SaveSystem<T> saveSystem;

        #endregion

        #region Methods

        public static T Create(string _name, SaveSystemConfig _config = null)
        {
            return Create<SaveSystems.PersistentDataPathFile<T>>(_name, _config);
        }

        public static T Create<S>(string _name, SaveSystemConfig _config = null) where S : SaveSystem<T>
        {
            return Create(typeof(S), _name, _config);
        }

        public static T Create(Type _type, string _name, SaveSystemConfig _config = null)
        {
            T b_data = CreateInstance<T>();
            b_data.name = _name;
            b_data.saveSystem = (SaveSystem<T>)(_type.GetConstructor(new[] { typeof(T), typeof(SaveSystemConfig) }).Invoke(new object[] { b_data, _config }));
            return b_data;
        }

        public static T Duplicate(T _original, string _name)
        {
            return Duplicate(_original.saveSystem.GetType(), _original, _name);
        }

        public static T Duplicate<S>(T _original, string _name) where S : SaveSystem<T>
        {
            return Duplicate(typeof(S), _original, _name);
        }

        public static T Duplicate(Type _type, T _original, string _name)
        {
            _original.name = _name;
            _original.saveSystem = (SaveSystem<T>)(_type.GetConstructor(new[] { typeof(T) }).Invoke(new object[] { _original }));
            return _original;
        }

        public void Save()
        {
            OnBeforeSave();
            if(saveSystem != null)
                saveSystem.Save();
            OnAfterSave();
        }
        
        public void Load()
        {
            OnBeforeLoad();
            if (saveSystem != null)
                saveSystem.Load();
            OnAfterLoad();
        }

        public void Unload()
        {
            OnBeforeUnload();
            if (saveSystem != null)
                saveSystem.Unload();
            OnAfterUnload();
        }

        public void Delete()
        {
            OnBeforeDelete();
            if (saveSystem != null)
                saveSystem.Delete();
            OnAfterDelete();
        }

        protected virtual void OnBeforeSave() { }
        protected virtual void OnAfterSave() { }
        protected virtual void OnBeforeLoad() { }
        protected virtual void OnAfterLoad() { }
        protected virtual void OnBeforeUnload() { }
        protected virtual void OnAfterUnload() { }
        protected virtual void OnBeforeDelete() { }
        protected virtual void OnAfterDelete() { }

        #endregion
    }
}