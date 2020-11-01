﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public UserRole UserRole { get; set; }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public Employee Employee { get; set; }
    }
}
