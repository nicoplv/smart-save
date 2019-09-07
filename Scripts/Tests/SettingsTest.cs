using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SettingsTest  
    {
        [Test]
        public void Settings_InstanceExist()
        {
            Assert.IsInstanceOf<SmartSaves.Settings.Setting>(SmartSaves.Settings.Setting.Instance);
        }
    }
}
