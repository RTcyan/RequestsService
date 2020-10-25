using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Оператор (сотрудник)
    /// </summary>
    public class Operator: Employee
    {
        /// <summary>
        /// Отделения оператора
        /// </summary>
        public Department Department { get; set; }
    }
}
