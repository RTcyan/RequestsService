using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Security
{
    /// <summary>
    /// Константы защиты
    /// </summary>
    public class SecurityConstants
    {
		/// <summary>
		/// Администратор
		/// </summary>
		public const string AdminRole = "ADMIN";

		/// <summary>
		/// Оператор
		/// </summary>
		public const string OperatorRole = "OPERATOR";

		/// <summary>
		/// Студент
		/// </summary>
		public const string StudentRole = "STUDENT";

		/// <summary>
		/// Логин администратора
		/// </summary>
		public const string AdminUserName = "ADMIN";

		/// <summary>
		/// Пароль администратора
		/// </summary>
		public const string AdminPassword = "123456";

		/// <summary>
		/// Email администратора
		/// </summary>
		public const string AdminEmail = "admin@test.ru";

		/// <summary>
		/// Имя администратора
		/// </summary>
		public const string AdminFirstName = "Администратор";

		/// <summary>
		/// Фамилия администратора
		/// </summary>
		public const string AdminSurName = "Системы";
	}
}
