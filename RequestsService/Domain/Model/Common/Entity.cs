using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestsService.Domain.Model.Common
{
    /// <summary>
    /// Интерфейс модели сущности предметной области
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Создание экземпляра модели сущности предметной области
        /// </summary>
        protected Entity()
        { }

        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public virtual long Id { get; set; }
    }
}
