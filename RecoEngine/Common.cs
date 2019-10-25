using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Reflection;
using System.Drawing;
using System.Drawing.Printing;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;

using Telerik.WinControls;
using Telerik.WinControls.UI;


namespace RecoEngine
{
   public static class Common
    {
      

        public static string strIPAddress = "";
        public static string strWindowsUser = "";
        public static string sUserName;
        public static int iUserID = 0;
        public static int iUserType = 1;
        public static string strTableName = "";
        public static string strKeyName = "";
        public static string strFormula = "";
        public static string sOpportunityName = "";
        public static string strfiltertxt = "";
        public static string sUserFullName;
        public static string strPtnlFilter="";
        private static string sLogFilePath = ""; //
        public static string sEXEVersion = "13.01.30";
        public static int iProjectID = 0;
        public static bool bIsConnectionStringEstablish = false;
        public static bool bIsTableMapped = false;

        public static string strProductName = ConfigurationSettings.AppSettings["ProductName"].ToString();


        public static frmOriginal fMain = null;

        public static Control TopMostParent(this Control control)
        {
            var parent = control.Parent;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
            }
            return parent;
        } 
        public static string strDropper = "-0.25";
        public static string strGrower = "0.25";
        public static string strStopper = "-1.0";
        public static class timePeriods
        {
            public static string[] strtp1;
            public static string[] strtp2;

        }

        public static void SetLoginDetailsToRegistry()
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper());
                sk1.SetValue("USERNAME", Common.sUserName.ToString());
                sk1.SetValue("PROJECT", Common.iProjectID.ToString());
                sk1.SetValue("TABLENAME", Common.strTableName.ToString());
                sk1.SetValue("KEYNAME", Common.strKeyName.ToString());
            }
            catch (Exception ex)
            {

                Common.LogMessage(ex);
            }
        }
        public static void GetLastLoginDetailsFromRegistry(ref string sUserName)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\" + Application.ProductName.ToUpper());
                if (sk1 == null)
                {
                    sUserName = "";
                    strTableName = "";
                }
                else
                {
                    if (sk1.GetValue("USERNAME") != null)
                        sUserName = (string)sk1.GetValue("USERNAME");

                    if (sk1.GetValue("PROJECT") != null)
                        iProjectID = int.Parse((string)sk1.GetValue("PROJECT"));

                    if (sk1.GetValue("TABLENAME") != null)
                        strTableName = (string)sk1.GetValue("TABLENAME");

                    if (sk1.GetValue("KEYNAME") != null)
                        strKeyName = (string)sk1.GetValue("KEYNAME");


                    if (sUserName == null)
                        sUserName = "";


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogMessage(ex);
            }
        }

        public static void SetRunningStatus(bool bFromClosing)
        {
         
            try
            {
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper());
                if (bFromClosing)
                    sk1.SetValue("TREIsRunning", 0, RegistryValueKind.String);
                else
                    sk1.SetValue("TREIsRunning", 1, RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                LogMessage(ex);
            }
        }

        public static string GetTREErrorLOfgFilePath()
        {
            try
            {

                string sMosTempPath = GetTRETempPath(); //AppPath();
                if (sMosTempPath.EndsWith(@"\") == false)
                { sMosTempPath += @"\"; }
                sMosTempPath += @"ErrorLog\";
                if (Directory.Exists(sMosTempPath) == false)
                {
                    Directory.CreateDirectory(sMosTempPath);
                }
                return sMosTempPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string AppPath()
        {
            try
            {
                string sLocation = "";
                sLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return sLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetTRETempPath()
        {
            try
            {
                string sMosTempPath = Path.GetTempPath();
                if (sMosTempPath.EndsWith(@"\") == false)
                { sMosTempPath += @"\"; }
                sMosTempPath += @"TRE\";
                if (Directory.Exists(sMosTempPath) == false)
                {
                    Directory.CreateDirectory(sMosTempPath);
                }
                return sMosTempPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void CheckLogFile()
        {
            try
            {
                sLogFilePath = GetTREErrorLOfgFilePath();
                if (sLogFilePath.EndsWith(@"\") == false)
                { sLogFilePath += @"\"; }
                sLogFilePath = sLogFilePath + "TREErrorLog.txt";
                if (File.Exists(sLogFilePath) == false)
                {
                    File.Create(sLogFilePath);
                }
            }
            catch (Exception ex)
            { ex = null; }
        }


        public static void LogMessage(Exception ex)
        {
            try
            {
                if (ex != null)
                {
                    CheckLogFile();
                    if (File.Exists(sLogFilePath))
                    {
                        StreamWriter stream = new StreamWriter(sLogFilePath, true);
                        stream.AutoFlush = true;

                        string sMessage = "";
                        string sMessage1 = "";
                        if (ex.InnerException != null)
                            sMessage = ex.InnerException.Message.ToString();

                        sMessage += "\n";
                        sMessage += ex.StackTrace.ToString();

                        sMessage += "\n" + ex.Message.ToString();

                        sMessage1 = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString() + "\t" + sMessage;
                        stream.WriteLine(sMessage1);

                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }
                }
            }
            catch (Exception Ex)
            {
                Ex = null;
            }
        }

        public static void LogMessage(string sMessage)
        {
            try
            {
                CheckLogFile();
                if (File.Exists(sLogFilePath))
                {
                    StreamWriter stream = new StreamWriter(sLogFilePath, true);
                    stream.AutoFlush = true;

                    sMessage = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString() + "\t" + sMessage;
                    stream.WriteLine(sMessage);

                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }
            }
            catch (Exception ex)
            {
                ex = null;
            }
        }

        public static Telerik.WinControls.UI.RadGroupBox GetfrmDummy()
        {
            Telerik.WinControls.UI.RadGroupBox gbDummy = new Telerik.WinControls.UI.RadGroupBox();
            //gbDummy.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.Panel;
            gbDummy.GroupBoxStyle = RadGroupBoxStyle.Office;
            gbDummy.Visible = true;
            gbDummy.Dock = DockStyle.Fill;
            gbDummy.BringToFront();
            return gbDummy;

        }

        public static Dictionary<string, Type> GetDict(DataTable dt)
        {
            try
            {
                //Dictionary<string, Type> de = new Dictionary<string, Type>();

                //foreach (DataColumn dc in dt.Columns)
                //{
                //    de.Add("Field." + dc.ColumnName.ToString(), dc.DataType);
                //    if (dc.ColumnName.IndexOf("_") > -1)
                //        de.Add("Field." + dc.ColumnName.ToString().Replace("_", ""), dc.DataType);
                //}

                Dictionary<string, Type> de = new Dictionary<string, Type>();

                foreach (DataColumn dc in dt.Columns)
                {
                    de.Add(dc.ColumnName.ToString(), dc.DataType);                    
                }


                return de;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Encrypt/Decrypt"

        public static string GetEncriptedValue(string strOriginalValue)
        {
            try
            {
                // First we need to turn the input string into a byte array. 
                byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(strOriginalValue);

                // Then, we need to turn the password into Key and IV 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("TRE",
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                // Now get the key/IV and do the encryption using the
                // function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first getting
                // 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by default
                // 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off
                // the algorithm to find out the sizes. 
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

                // Now we need to turn the resulting byte array into a string. 
                // A common mistake would be to use an Encoding class for that.
                //It does not work because not all byte values can be
                // represented by characters. 
                // We are going to be using Base64 encoding that is designed
                //exactly for what we are trying to do. 
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception e)
            {
                e = null;
                return "";
            }
        }


        public static string GetDecryptedValue(string strEncryptedValue)
        {
            try
            {
                // First we need to turn the input string into a byte array. 
                // We presume that Base64 encoding was used 
                byte[] cipherBytes = Convert.FromBase64String(strEncryptedValue);

                // Then, we need to turn the password into Key and IV 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("TRE",
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                // Now get the key/IV and do the decryption using
                // the function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first
                // getting 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by
                // default 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off
                // the algorithm to find out the sizes. 
                byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

                // Now we need to turn the resulting byte array into a string. 
                // A common mistake would be to use an Encoding class for that.
                // It does not work 
                // because not all byte values can be represented by characters. 
                // We are going to be using Base64 encoding that is 
                // designed exactly for what we are trying to do. 
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception e)
            {
                e = null;
                return "";
            }
        }

        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the
            // next line with something like 
            //      TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }



        #endregion "Encrypt/Decrypt"

        #region Text Log

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "C:\\Logs\\";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        } 
        #endregion
    }
}
