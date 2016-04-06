using System;

namespace CarFuel.Services
{
    public class BusinessException : Exception
    {     
        public Guid? UserId { get; set; }

        public BusinessException() { }

        public BusinessException(string message) : base(message)
        {
            //
        }
    }
}