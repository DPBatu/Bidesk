namespace Bidesk.Domain.CustomAttributes.Log
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class LoggableAttribute : Attribute
    {
        public bool IsLoggable { get; }

        public LoggableAttribute(bool isLoggable = false)
        {
            IsLoggable = isLoggable;
        }
    }

}
