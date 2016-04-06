using System;

namespace CarFuel.Services
{
    public class OverQuotaException : BusinessException
    {
        public OverQuotaException(string message) : base(message)
        {
        }
    }
}