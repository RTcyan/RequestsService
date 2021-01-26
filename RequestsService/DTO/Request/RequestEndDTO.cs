using System;
namespace RequestsService.DTO.Request
{
    public class RequestEndDTO
    {
        /// <summary>
        /// Результат по заявке
        /// </summary>
        public string ResultFileId { get; set; }

        /// <summary>
        /// Комментарий оператора
        /// </summary>
        public string OperatorComment { get; set; }
    }
}
