namespace SmartSaves
{
    #region Enums

    public enum SaveTypes
    {
        Text,
        Binary,
        BinaryAndChecksum
    }

    #endregion

    public abstract class SaveSystem<T> where T : Data<T>
    {
        #region Variables

        protected Data<T> data;

        #endregion

        #region Constructor

        public SaveSystem(Data<T> _data)
        {
            data = _data;
        }

        #endregion

        #region Methods

        //public static SaveSystem<T> Create(Data<T> _data)
        //{
        //    switch (Settings.Instance.SaveType)
        //    {
        //        case SaveTypes.Binary:
        //            return new SaveSystems.PersistentDataPathFileBinary<T>(_data);
        //        case SaveTypes.BinaryAndChecksum:
        //            return new SaveSystems.PersistentDataPathFileBinaryChecksum<T>(_data);
        //        default:
        //            return new SaveSystems.PersistentDataPathFileText<T>(_data);
        //    }
        //}

        public static SaveSystem<T> Create(Data<T> _data, SaveTypes _saveType)
        {
            switch (_saveType)
            {
                case SaveTypes.Binary:
                    return new SaveSystems.PersistentDataPathFileBinary<T>(_data);
                case SaveTypes.BinaryAndChecksum:
                    return new SaveSystems.PersistentDataPathFileBinaryChecksum<T>(_data);
                default:
                    return new SaveSystems.PersistentDataPathFileText<T>(_data);
            }
        }

        public abstract void Save();
        public abstract void Load();
        public abstract void Unload();
        public abstract void Delete();

#endregion
    }
}