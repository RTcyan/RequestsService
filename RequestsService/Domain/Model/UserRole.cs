using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Новая
        /// </summary>
        Admin = 0,
        /// <summary>
        ///  В обработке
        /// </summary>
        Student = 1,
        /// <summary>
        /// Обработано
        /// </summary>
        Operator = 2,
    }
}
