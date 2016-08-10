using System;

namespace Matrix
{
    [Serializable()]
    public class IncompabilityOfColumnsAndRows : Exception
    {
        public IncompabilityOfColumnsAndRows() : base() { }
        public IncompabilityOfColumnsAndRows(string message) : base(message) { }
        public IncompabilityOfColumnsAndRows(string message, System.Exception inner) : base(message, inner) { }

        protected IncompabilityOfColumnsAndRows(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable()]
    public class SumExceptiom : Exception
    {
        public SumExceptiom() : base() { }
        public SumExceptiom(string message) : base(message) { }
        public SumExceptiom(string message, System.Exception inner) : base(message, inner) { }

        protected SumExceptiom(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable()]
    public class MultException : Exception
    {
        public MultException() : base() { }
        public MultException(string message) : base(message) { }
        public MultException(string message, System.Exception inner) : base(message, inner) { }

        protected MultException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
