using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DataTest : SmartSaves.Data<DataTest>
    {
        [SerializeField]
        public float VarTest;
    }

    public class SaveSystemTest
    {
        [Test]
        public void PersistentDataPathFileText()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_Text", SmartSaves.SaveTypes.Text);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Text", SmartSaves.SaveTypes.Text);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinary()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_Binary", SmartSaves.SaveTypes.Binary);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Binary", SmartSaves.SaveTypes.Binary);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinaryChecksum()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_BinaryAndChecksum", SmartSaves.SaveTypes.BinaryAndChecksum);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_BinaryAndChecksum", SmartSaves.SaveTypes.BinaryAndChecksum);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }
    }
}
