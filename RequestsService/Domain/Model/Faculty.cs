using RequestsService.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model
{
    /// <summary>
    /// Факультет
    /// </summary>
    public class Faculty: Entity
    {
        /// <summary>
        /// Название факультета
        /// </summary>
        public string Name { get; set; }
    }
}
