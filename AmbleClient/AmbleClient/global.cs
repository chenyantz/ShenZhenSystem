﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows;
using System.Windows.Forms;
using log4net;
using PasswordServer;
using System.Security.Cryptography;
using System.IO;

namespace AmbleClient

{
   public enum JobDescription
    { Admin=0,Boss=1,Sales=2,SalesManager=3,Purchaser=4,PurchasersManager=5,Logistics=6,LogisticsManager=7,Financial=8,FinancialManager=9
    
    }

    public enum Currency
    {
     USD=0,CNY=1,EUR=2,HK=3,JP=4,ERROR=5
    
    }
          

    public static class UserInfo
    {
        public static int UserId;
        public static string UserName;
        public static JobDescription Job;
    }

    public static class UserCombine
    { 
       public static List<int> GetUserCanBeSales()
        {
            List<int> canBeSales = new List<int>();
            canBeSales.Add((int)JobDescription.Admin);
            canBeSales.Add((int)JobDescription.Boss);
           canBeSales.Add((int)JobDescription.Sales);
           canBeSales.Add((int)JobDescription.SalesManager);
           return canBeSales;
        
        }

       public static List<int> GetUserCanBeBuyers()
       {
           List<int> canBeBuyers = new List<int>();
           canBeBuyers.Add((int)JobDescription.Admin);
           canBeBuyers.Add((int)JobDescription.Boss);
           canBeBuyers.Add((int)JobDescription.Purchaser);
           canBeBuyers.Add((int)JobDescription.PurchasersManager);
           return canBeBuyers;
       }



    
    
    }



    public class Logger
    {
        private static ILog logger = LogManager.GetLogger("Amble");

        public static void Debug(string Message)
        {
            logger.Debug(Message);
        }

        public static void Info(string Message)
        {
            logger.Info(Message);
        }

        public static void Warning(string Message)
        {
            logger.Warn(Message);
        }

        public static void Error( string Message)
        {
            logger.Error(Message);
        }

        public static void Fatal(string Message)
        {
            logger.Fatal(Message);
        }
    } 



    public static class Tool
    {
        public static string Get6DigitalNumberAccordingToId(int id)
        {
            int length = (int)Math.Log10(id) + 1;

            switch (length)
            { 
                case 1:
                    return "00000" + id;
                case 2:
                    return "0000" + id;
                case 3:
                    return "000" + id;
                case 4:
                    return "00" + id;
                case 5:
                    return "0" + id;
                default:
                    return id.ToString();
            }
        }

        public static string GetIdAccordingTo6DigitalNumber(string DigitalId)
        {
         int number=Convert.ToInt32(DigitalId);
         return number.ToString();
        }

    }


    public static class ItemsCheck
    {

        public static bool CheckTextBoxEmpty(TextBox tb)
        {
            if (string.IsNullOrWhiteSpace(tb.Text.Trim()))
            {
                tb.Focus();
                 return false;
            }
            return true;
        }

        public static bool CheckPhoneNumber(string phoneNumber)
        { 
        
        return false;
        }

        public static bool CheckEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");   

        }

        public static bool CheckIntNumber(TextBox tb)
        {
            int tempvalue;

            return int.TryParse(tb.Text.Trim(),out tempvalue);

        }
     
        public static bool CheckFloatNumber(TextBox tb)
        {
            float tempvalue;
            return float.TryParse(tb.Text.Trim(), out tempvalue);
           
        }
            
    
    }


    public static class ServerInfo
    {
        private static string userId;
        private static string password;
        private static string strServer;
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static string desKey = "AmbleSYS";

        public static DatabaseInfo dbinfo;

        static ServerInfo()
        {
            try
            {
                strServer = OperatorFile.GetIniFileString("DataBase", "Server", "", Environment.CurrentDirectory + "\\AmbleAppServer.ini");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                MessageBox.Show("Please check the AmbleAppServer.ini file.");
            }
            try
            {
                ChannelServices.RegisterChannel(new TcpClientChannel(), false);
                dbinfo = (DatabaseInfo)Activator.GetObject(typeof(DatabaseInfo), "tcp://192.168.1.101:1111/DatabaseInfo");
                userId = DecryptDES(dbinfo.GetDbUser(), desKey);
                password=DecryptDES(dbinfo.GetDbPassword(),desKey);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            //    MessageBox.Show("Can not Connected to the Server---------, Please contact the Admin.");
            }
        
        }


        public static string GetUserId()
        {
            return userId;
        }

        public static string GetPassword()
         {
             return password;
        }
        public static string GetServerAddress()
        {
            return strServer;
        }


        private static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch(Exception ex)
            {
                return decryptString;
            }
        }


  
    
    }






}
