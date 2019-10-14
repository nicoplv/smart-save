using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartSaves.SaveSystems
{
    public enum Types
    {
        PersistentDataPathFile,
    }

    public static class SaveSystemTypeExtension
    {
        public static Type GetSaveSystemType<T>(this Types _saveSystemType) where T : Data<T>
        {
            switch (_saveSystemType)
            {
                case Types.PersistentDataPathFile:
                    return typeof(SaveSystems.PersistentDataPathFile<T>);
            }
            return null;
        }
    }
}