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
            DataTest dataTestSave = DataTest.Create("DataTest_BinaryAndChecksum", SmartSaves.SaveTypes.BinaryChecksum);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_BinaryAndChecksum", SmartSaves.SaveTypes.BinaryChecksum);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinaryShuffled()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_Shuffled", SmartSaves.SaveTypes.BinaryShuffled);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Shuffled", SmartSaves.SaveTypes.BinaryShuffled);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinaryShuffledChecksum()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_ShuffledChecksum", SmartSaves.SaveTypes.BinaryShuffledChecksum);
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_ShuffledChecksum", SmartSaves.SaveTypes.BinaryShuffledChecksum);
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }
    }
}
