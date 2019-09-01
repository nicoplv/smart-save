namespace SmartSaves
{
    public abstract class SaveSystemConfig
    {

    }

    public abstract class SaveSystem<T> where T : Data<T>
    {
        #region Variables

        protected Data<T> data;

        #endregion

        #region Constructor

        public SaveSystem(Data<T> _data, SaveSystemConfig _config)
        {
            data = _data;
        }

        #endregion

        #region Methods

        public abstract void Save();
        public abstract void Load();
        public abstract void Unload();
        public abstract void Delete();

        #endregion
    }
}