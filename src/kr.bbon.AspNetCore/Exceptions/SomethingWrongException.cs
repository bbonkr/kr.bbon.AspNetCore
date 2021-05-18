using System;

namespace kr.bbon.AspNetCore
{
    public abstract class SomethingWrongException : Exception
    {
        public SomethingWrongException(string message) : base(message) { }

        public abstract object GetDetails();

        public abstract T GetDetails<T>() where T : class;
    }

    public class SomethingWrongException<TDetails> : SomethingWrongException where TDetails : class
    {
        public SomethingWrongException(string message, TDetails details) : base(message)
        {
            this.Details = details;
        }

        public TDetails Details { get; init; }

        public override object GetDetails()
        {
            return Details;
        }
        public override T GetDetails<T>() where T:class
        {
            return (T)GetDetails();
        }
    }
}
