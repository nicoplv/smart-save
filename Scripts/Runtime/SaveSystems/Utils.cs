using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartSaves.SaveSystems
{
    public static class Utils
    {
        #region Methods

        public static string Shuffle(string _value, int _seed)
        {
            System.Random rng = new System.Random(_seed);
            char[] valueArray = _value.ToCharArray();
            for (int n = valueArray.Length - 1; n > 0; n--)
            {
                int k = rng.Next(n + 1);
                char value = valueArray[k];
                valueArray[k] = valueArray[n];
                valueArray[n] = value;
            }
            return new string(valueArray);
        }

        public static string Unshuffle(string _value, int _seed)
        {
            System.Random rng = new System.Random(_seed);
            char[] valueArray = _value.ToCharArray();

            int[] k = new int[valueArray.Length];
            for (int n = valueArray.Length - 1; n > 0; n--)
                k[n] = rng.Next(n + 1);

            for (int n = 1; n < valueArray.Length; n++)
            {
                char value = valueArray[n];
                valueArray[n] = valueArray[k[n]];
                valueArray[k[n]] = value;
            }

            return new string(valueArray);
        }

        public static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        #endregion
    }
}