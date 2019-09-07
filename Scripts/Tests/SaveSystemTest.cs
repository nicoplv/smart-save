using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SmartSaves.SaveSystems;

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
            DataTest dataTestSave = DataTest.Create("DataTest_Text", Config.ForPersistentDataPathFile<DataTest>(binary: false));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Text", Config.ForPersistentDataPathFile<DataTest>(binary: false));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinary()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_Binary", Config.ForPersistentDataPathFile<DataTest>(binary: true));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Binary", Config.ForPersistentDataPathFile<DataTest>(binary: true));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileChecksum()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_Checksum", Config.ForPersistentDataPathFile<DataTest>(checksum: true));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_Checksum", Config.ForPersistentDataPathFile<DataTest>(checksum: true));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileShuffledRandom()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_ShuffledRandom", Config.ForPersistentDataPathFile<DataTest>(shuffle: Config.PersistentDataPathFileShuffleTypes.Random));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_ShuffledRandom", Config.ForPersistentDataPathFile<DataTest>(shuffle: Config.PersistentDataPathFileShuffleTypes.Random));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileShuffledDeviceId()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_ShuffledDeviceId", Config.ForPersistentDataPathFile<DataTest>(shuffle: Config.PersistentDataPathFileShuffleTypes.DeviceId));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_ShuffledDeviceId", Config.ForPersistentDataPathFile<DataTest>(shuffle: Config.PersistentDataPathFileShuffleTypes.DeviceId));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileAll()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create("DataTest_All", Config.ForPersistentDataPathFile<DataTest>(binary: true, checksum: true, shuffle: Config.PersistentDataPathFileShuffleTypes.DeviceId));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create("DataTest_All", Config.ForPersistentDataPathFile<DataTest>(binary: true, checksum: true, shuffle: Config.PersistentDataPathFileShuffleTypes.DeviceId));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }
    }
}
