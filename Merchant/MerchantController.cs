﻿using Lampac;
using Lampac.Engine;
using Shared.Model.Base;
using System;
using System.Linq;
using System.Web;

namespace Merchant
{
    public class MerchantController : BaseController
    {
        static DateTime LastWriteTimeUsers = default;

        static string _users = null;

        public static void PayConfirm(string email, string merch, string order, int days = 0)
        {
            var lastWriteTimeUsers = System.IO.File.GetLastWriteTime("merchant/users.txt");

            if (_users == null || LastWriteTimeUsers != lastWriteTimeUsers)
            {
                LastWriteTimeUsers = lastWriteTimeUsers;
                _users = System.IO.File.ReadAllText("merchant/users.txt");
            }

            string users = _users;

            if (!users.Contains($",{merch},{order}"))
            {
                DateTime ex = default;

                if (days > 0)
                {
                    if (AppInit.conf.accsdb.users.FirstOrDefault(i => i.id == email || i.id.Contains(email)) is AccsUser user)
                    {
                        ex = ex > DateTime.UtcNow ? ex.AddDays(days) : DateTime.UtcNow.AddDays(days);
                        user.expires = ex;
                    }
                    else
                    {
                        ex = DateTime.UtcNow.AddDays(days);
                        AppInit.conf.accsdb.users.Add(new AccsUser() 
                        {
                            id = email,
                            expires = ex
                        });
                    }
                }
                else
                {
                    if (AppInit.conf.accsdb.users.FirstOrDefault(i => i.id == email || i.id.Contains(email)) is AccsUser user)
                    {
                        ex = ex > DateTime.UtcNow ? ex.AddMonths(AppInit.conf.Merchant.accessForMonths) : DateTime.UtcNow.AddMonths(AppInit.conf.Merchant.accessForMonths);
                        user.expires = ex;
                    }
                    else
                    {
                        ex = DateTime.UtcNow.AddMonths(AppInit.conf.Merchant.accessForMonths);
                        AppInit.conf.accsdb.users.Add(new AccsUser()
                        {
                            id = email,
                            expires = ex
                        });
                    }
                }

                System.IO.File.AppendAllText("merchant/users.txt", $"{email.ToLower()},{ex.ToFileTimeUtc()},{merch},{order}\n");

                _users += $"{email.ToLower()},{ex.ToFileTimeUtc()},{merch},{order}\n";
                LastWriteTimeUsers = System.IO.File.GetLastWriteTime("merchant/users.txt");
            }
        }


        public static void WriteLog(string merch, string content)
        {
            try
            {
                System.IO.File.AppendAllText($"merchant/log/{merch}.txt", content + "\n\n\n");
            }
            catch { }
        }


        public static string decodeEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;    

            return HttpUtility.UrlDecode(email.ToLower().Trim());
        }
    }
}
