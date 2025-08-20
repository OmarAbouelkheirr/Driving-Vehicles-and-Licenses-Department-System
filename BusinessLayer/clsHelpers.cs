using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BusinessLayer
{
    public class clsHelpers
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public static string GetUsernameFromRegistry(string KeyPath)
        {
            try
            {
                string value = Registry.GetValue(KeyPath, "Username", null) as string;

                if (value != null)
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetPasswordFromRegistry(string KeyPath)
        {
            try
            {
                string value = Registry.GetValue(KeyPath, "Password", null) as string;

                if (value != null)
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool StorePasswordRegistry(string keyPath, string password)
        {
            try
            {
                Registry.SetValue(keyPath, "Password", password, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool StoreUsernameRegistry(string keyPath, string username)
        {
            try
            {
                Registry.SetValue(keyPath, "Username", username, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IfUserStored(string keyPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                if (key == null)
                    return false;

                object username = key.GetValue("Username");
                object password = key.GetValue("Password");

                return username != null && password != null;
            }
        }

        public static bool ClearCurrentUserValues(string keyPath)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(keyPath, writable: true))
                {
                    if (key == null) return false;

                    key.DeleteValue("Username", false);
                    key.DeleteValue("Password", false);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
