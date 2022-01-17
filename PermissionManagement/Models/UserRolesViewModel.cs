﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionManagement.Models
{
    public class UserRolesViewModel
    {
        public string RoleName { get; set; }
        public string Selected { get; set; }
    }
    public class ManageUserRolesViewModel
    {
        public string UserId{ get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
}