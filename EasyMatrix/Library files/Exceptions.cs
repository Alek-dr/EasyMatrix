using System;

namespace MatrixOperations
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
    public class WrongNumberOfRows : Exception
    {
        public WrongNumberOfRows() : base() { }
        public WrongNumberOfRows(string message) : base(message) { }
        public WrongNumberOfRows(string message, System.Exception inner) : base(message, inner) { }

        protected WrongNumberOfRows(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable()]
    public class WrongNumberOfColumns : Exception
    {
        public WrongNumberOfColumns() : base() { }
        public WrongNumberOfColumns(string message) : base(message) { }
        public WrongNumberOfColumns(string message, System.Exception inner) : base(message, inner) { }

        protected WrongNumberOfColumns(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
