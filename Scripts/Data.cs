using UnityEngine;

namespace SmartSaves
{
    public class Data<T> : ScriptableObject where T : Data<T>
    {
        #region Variables

        private SaveSystem<T> saveSystem;

        #endregion

        #region Methods

        public static T Create(string _name)
        {
            //T b_data = CreateInstance<T>();
            //b_data.name = _name;
            //b_data.saveSystem = SaveSystem<T>.Create(b_data);
            //return b_data;
            return Create(_name, Settings.Instance.SaveType);
        }

        public static T Create(string _name, SaveTypes _saveType)
        {
            T b_data = CreateInstance<T>();
            b_data.name = _name;
            b_data.saveSystem = SaveSystem<T>.Create(b_data, _saveType);
            return b_data;
        }

        public static T Duplicate(T _original, string _name)
        {
            //_original.name = _name;
            //_original.saveSystem = SaveSystem<T>.Create(_original);
            //return _original;
            return Duplicate(_original, _name, Settings.Instance.SaveType);
        }

        public static T Duplicate(T _original, string _name, SaveTypes _saveType)
        {
            _original.name = _name;
            _original.saveSystem = SaveSystem<T>.Create(_original, _saveType);
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