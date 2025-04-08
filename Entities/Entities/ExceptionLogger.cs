using Core.Entities.EntityFramwoek;

namespace Entities.Entities
{
    public class ExceptionLogger : BaseEntity
    {
        public string ExceptionMessage { get; set; }
        public string ControllerName { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
