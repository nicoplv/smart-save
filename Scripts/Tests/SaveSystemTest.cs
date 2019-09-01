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
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Text", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: false));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Text", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: false));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileBinary()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Binary", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: true));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Binary", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: true));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileChecksum()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Checksum", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(checksum: true));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_Checksum", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(checksum: true));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileShuffledRandom()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_ShuffledRandom", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.Random));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_ShuffledRandom", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.Random));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileShuffledDeviceId()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_ShuffledDeviceId", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.DeviceId));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_ShuffledDeviceId", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.DeviceId));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }

        [Test]
        public void PersistentDataPathFileAll()
        {
            float randomNumber = Random.value;
            DataTest dataTestSave = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_All", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: true, checksum: true, shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.DeviceId));
            dataTestSave.VarTest = randomNumber;
            dataTestSave.Save();

            DataTest dataTestLoad = DataTest.Create<SmartSaves.SaveSystems.PersistentDataPathFile<DataTest>>("DataTest_All", new SmartSaves.SaveSystems.PersistentDataPathFileConfig(binary: true, checksum: true, shuffle: SmartSaves.SaveSystems.PersistentDataPathFileConfig.ShuffleTypes.DeviceId));
            dataTestLoad.Load();

            Assert.AreEqual(dataTestLoad.VarTest, randomNumber);
        }
    }
}
