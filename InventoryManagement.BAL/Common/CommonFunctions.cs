using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace InventoryManagement.BAL.Common
{
    public static class CommonFunctions
    {
        public static string ConnectionString = @"Data Source=CREDENCYS-PC\SQLEXPRESS;initial catalog=InventoryManagement;persist security info=True;user id=sa;password=sa123;";

        #region XML Related

        public static XElement ToXML<T>(this IList<T> lstToConvert, string rootName, string subLevelName)
        {
            Func<T, bool> filter = null;
            var lstConvert = (filter == null) ? lstToConvert : lstToConvert.Where(filter);
            return new XElement(rootName,
               (from node in lstConvert
                select new XElement(subLevelName,
                   from subnode in node.GetType().GetProperties()
                   select new XElement(subnode.Name, subnode.GetValue(node, null)))));
        }

        //public static XElement ToXML(this DataSet lstToConvert, string rootName, string subLevelName)
        //{
        //    DataTable _dt = lstToConvert.Tables[0];
        //    var lstConvert = _dt.AsEnumerable();
        //    return new XElement(rootName,
        //       (from node in lstConvert
        //        select new XElement(subLevelName,
        //           from subnode in node.GetType().GetProperties()
        //           select new XElement(subnode.Name, subnode.GetValue(node, null)))));
        //}

        //DataTable Convert To List Method
        public static IList<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (value.GetType() == typeof(System.DBNull))
                        {
                            value = null;
                        }
                        pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        #endregion

        #region Enum Related Methods

        public static List<EnumListItem> GetEnumValues(Type type, bool isSelectRequired)
        {
            List<EnumListItem> el = new List<EnumListItem>();
            EnumListItem ei;
            foreach (int item in Enum.GetValues(type))
            {
                ei = GetEnumItem(type, item);
                el.Add(ei);
            }
            return el;
        }

        private static EnumListItem GetEnumItem(Type type, int item)
        {
            string name = Enum.GetName(type, item);
            string displayName = string.Empty;
            object[] displayAttributes = type.GetField(Enum.GetName(type, item)).GetCustomAttributes(typeof(DisplayValueAttribute), false);
            if (displayAttributes.Length > 0)
                displayName = ((DisplayValueAttribute)displayAttributes[0]).Value;
            else
                displayName = name;
            return new EnumListItem(item, name, displayName);
        }

        public static List<EnumListItem> GetEnumValues(Type type, bool isSelectRequired, IList objList, string itemProperty)
        {
            List<EnumListItem> el = new List<EnumListItem>();
            EnumListItem ei;
            foreach (object obj in objList)
            {
                int item = (int)obj.GetType().GetProperty(itemProperty).GetValue(obj, null);
                ei = GetEnumItem(type, item);
                el.Add(ei);
            }
            return el;
        }

        #endregion

        #region CurrentUserInfo

        //public static CurrentUserInfo CurrentUserInformation
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["CurrentUserInformation"] == null)
        //        {
        //            HttpContext.Current.Session["CurrentUserInformation"] = new CurrentUserInfo();

        //        }
        //        return (CurrentUserInfo)HttpContext.Current.Session["CurrentUserInformation"];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["CurrentUserInformation"] = value;
        //    }
        //}

        #endregion

        #region common Properties


        //public static string AdminEmail
        //{
        //    get
        //    {
        //        Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
        //        MailSettingsSectionGroup mailSettings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
        //        return mailSettings.Smtp.Network.UserName.ToString();
        //    }
        //}

        #endregion

        #region File & Folder Related Properties & Methods

        /// <summary>
        /// To Get File Bytes on base of passed file path
        /// </summary>
        /// <param name="fileNamewithPath"></param>
        /// <returns></returns>
        public static byte[] GetFileBytes(string fileNamewithPath)
        {
            byte[] fileByte = null;
            if (File.Exists(HttpContext.Current.Server.MapPath(fileNamewithPath)))
            {
                FileStream fstrm = new FileStream(HttpContext.Current.Server.MapPath(fileNamewithPath), System.IO.FileMode.Open, System.IO.FileAccess.Read);

                fileByte = new byte[fstrm.Length];
                fstrm.Read(fileByte, 0, fileByte.Length);
                fstrm.Close();
            }
            return fileByte;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        public static bool CreateDirectory(string Path)
        {
            if (!Directory.Exists(Path))
            {
                try
                {
                    Directory.CreateDirectory(Path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //Save the file with File byte array
        public static bool SaveFile(string path, string fileName, byte[] fileByte)
        {
            if (CreateDirectory(path))
            {
                if (fileByte != null)
                {
                    FileStream fs = null;
                    try
                    {
                        string fileActualName = fileName;
                        fs = new FileStream(path + fileActualName, FileMode.Create, FileAccess.Write);
                        fs.Write(fileByte, 0, fileByte.Length);
                        fs.Flush();
                        fs.Dispose();
                        fs.Close();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //Save the file normally
        public static bool SaveFile(string path, string fileName)
        {
            if (CreateDirectory(path))
            {
                FileStream fs = null;
                try
                {
                    string fileActualName = fileName;
                    fs = new FileStream(path + fileActualName, FileMode.Create, FileAccess.Write);
                    fs.Flush();
                    fs.Dispose();
                    fs.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filenameWithPath)
        {
            try
            {
                if (File.Exists(filenameWithPath))
                {
                    File.Delete(filenameWithPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetFileExtension(string fileName)
        {
            if (fileName != null)
                return fileName.Substring(fileName.LastIndexOf('.') + 1, fileName.Length - (fileName.LastIndexOf('.') + 1));
            else
                return null;
        }

        /* Zip Download */

        //public static void CreateZipFile(string strZipPath, string OrigFilePath, string FolderName)
        //{
        //    try
        //    {
        //        using (ZipOutputStream s = new ZipOutputStream(File.Create(strZipPath)))
        //        {
        //            s.UseZip64 = UseZip64.Dynamic;
        //            s.SetLevel(9); // 0 - store only to 9 - means best compression
        //            byte[] buffer = new byte[4096];
        //            //for file zipping
        //            ZipEntry entry = new ZipEntry(Path.GetFileName(OrigFilePath));
        //            entry.DateTime = DateTime.Now;
        //            s.PutNextEntry(entry);
        //            using (FileStream fs = File.OpenRead(OrigFilePath))
        //            {
        //                int sourceBytes;
        //                do
        //                {
        //                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //                    s.Write(buffer, 0, sourceBytes);
        //                } while (sourceBytes > 0);
        //            }


        //            if (FolderName != "")
        //            {
        //                ////for folder zipping
        //                ArrayList ar = GenerateFileList(FolderName); // generate file list
        //                int TrimLength = (Directory.GetParent(FolderName)).ToString().Length;
        //                //// find number of chars to remove     // from orginal file path
        //                TrimLength += 1; //remove '\'
        //                FileStream ostream;
        //                byte[] obuffer;
        //                ////string outPath = FolderName + @"\" + outputPathAndFile;
        //                string outPath = strZipPath;

        //                s.SetLevel(9); // maximum compression
        //                ZipEntry oZipEntry;
        //                foreach (string Fil in ar) // for each file, generate a zipentry
        //                {
        //                    oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
        //                    s.PutNextEntry(oZipEntry);

        //                    if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
        //                    {
        //                        ostream = File.OpenRead(Fil);
        //                        obuffer = new byte[ostream.Length];
        //                        ostream.Read(obuffer, 0, obuffer.Length);
        //                        s.Write(obuffer, 0, obuffer.Length);
        //                    }
        //                }
        //                s.Finish();
        //                s.Close();
        //            }
        //        }


        //    }
        //    catch
        //    {
        //    }
        //}

        public static ArrayList GenerateFileList(string Dir)
        {
            ArrayList fils = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                fils.Add(file);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                // if directory is completely empty, add it
                {
                    fils.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    fils.Add(obj);
                }
            }
            return fils; // return file list
        }

        public static void download_Zip(FileInfo file)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + file.Name + "");
                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/zip";
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.WriteFile(file.FullName);
                HttpContext.Current.Response.End();
            }
            catch
            {
                //HttpContext.Current.Response.Write(ex.Message.ToString());
            }
        }

        #endregion

        #region Common

        public static bool ValidateDataset(DataSet ds)
        {
            bool isValid = false;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool ValidateDataTable(DataTable dt)
        {
            bool isValid = false;

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = true;
            }

            return isValid;

        }

        public static string GetCommanSeparatedValues(List<object> lstIn)
        {
            string strCSVIds = string.Empty;

            for (int i = 0; i < lstIn.Count; i++)
            {
                if (!String.IsNullOrEmpty(lstIn[i].ToString()))
                    strCSVIds += lstIn[i].ToString() + ",";
            }

            strCSVIds = strCSVIds.Substring(0, strCSVIds.LastIndexOf(","));

            return strCSVIds;
        }


        public static String CheckNull(this string value)
        {
            return value == null ? "" : value;
        }

        public static Int32? CheckNull(this Int32? value)
        {
            return value == null ? 0 : value;
        }
        public static Int64? CheckNull(this Int64? value)
        {
            return value == null ? 0 : value;
        }

        public static Int64 CheckNull(this Int64 value)
        {
            return value == null ? 0 : value;
        }

        public static Int32 CheckNull(this Int32 value)
        {
            return value == null ? 0 : value;
        }

        public static bool CheckNull(this bool value)
        {
            return value == null ? false : value;
        }


        public static decimal CheckNull(this decimal value)
        {
            return value == null ? 0 : value;
        }



        #endregion

        #region NPOI Color

        //public static HSSFColor setColorNPOI(HSSFWorkbook workbook, short index, byte r, byte g, byte b)
        //{
        //    HSSFPalette palette = workbook.GetCustomPalette();
        //    HSSFColor hssfColor = null;
        //    hssfColor = palette.FindColor(r, g, b);
        //    if (hssfColor == null)
        //    {
        //        palette.SetColorAtIndex(index, r, g, b);
        //        hssfColor = palette.GetColor(index);
        //    }
        //    return hssfColor;
        //}

        #endregion

        #region Password

        /// <summary>
        /// get encrypted password
        /// </summary>
        /// <param name="cleanString"></param>
        /// <returns></returns>
        public static String getEncrypt(String cleanString)  //getting encrypted string
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }

        //For SALT + SHA1
        private const int PBKDF2IterCount = 1000; // default for Rfc2898DeriveBytes
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        /// <summary>
        /// To get salt value of passwword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            System.Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            System.Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }


        /// <summary>
        /// Verifies the hashed password.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Wrong length or version header.
            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
                return false;

            byte[] salt = new byte[SaltSize];
            System.Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            byte[] storedSubkey = new byte[PBKDF2SubkeyLength];
            System.Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return storedSubkey.SequenceEqual(generatedSubkey);
        }

        #endregion

        #region Month

        public static string GetMonth(string strMonth)
        {
            switch (strMonth)
            {
                case "Jan":
                    return "1";
                case "Feb":
                    return "2";
                case "Mar":
                    return "3";
                case "Apr":
                    return "4";
                case "May":
                    return "5";
                case "Jun":
                    return "6";
                case "July":
                    return "7";
                case "Aug":
                    return "8";
                case "Sep":
                    return "9";
                case "Oct":
                    return "10";
                case "Nov":
                    return "11";
                case "Dec":
                    return "12";
                default:
                    return "0";
            }

        }

        #endregion


        #region Common Encrypt Decrypt Methods
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
        // -- Code ends --

        #region Convart Json To Datatable
        public static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }
        #endregion

        public static String ChangeDateTimeFormat(String InputString)
        {
            if (InputString != null)
            {
                List<String> li = InputString.Split('/').ToList();
                return li[1] + "/" + li[0] + "/" + li[2];

            }
            return "";
        }

        public static string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}
