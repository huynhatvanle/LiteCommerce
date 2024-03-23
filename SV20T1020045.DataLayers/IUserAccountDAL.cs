﻿using SV20T1020045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020045.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
