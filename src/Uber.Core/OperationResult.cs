using System.Collections.Generic;
using System.Linq;

namespace Uber.Core
{
    public class OperationResult
    {
        public bool Succeeded => !Errors.Any();
        public IEnumerable<string> Errors { get; protected set; }

        private protected OperationResult() { }

        private static OperationResult successResult = new OperationResult { Errors = new string[0] };

        public static OperationResult Success => successResult;

        public static OperationResult Failed(string error) =>
            new OperationResult { Errors = new[] { error } };

        public static OperationResult Failed(IEnumerable<string> errors) =>
            new OperationResult { Errors = errors };
    }

    public class OperationResult<TResult> : OperationResult
    {
        public readonly TResult Value;

        private OperationResult() { }

        public OperationResult(TResult value)
        {
            Value = value;
            Errors = new string[0];
        }

        public new static OperationResult<TResult> Success(TResult result) => new OperationResult<TResult>(result);

        public new static OperationResult<TResult> Failed(string error) =>
            new OperationResult<TResult> { Errors = new[] { error } };

        public new static OperationResult<TResult> Failed(IEnumerable<string> errors) =>
            new OperationResult<TResult> { Errors = errors };
    }
}
