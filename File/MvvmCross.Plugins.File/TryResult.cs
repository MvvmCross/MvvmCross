namespace MvvmCross.Plugins.File
{
    public static class TryResult
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Cirrious.MvvmCross.Plugins.File.TryResult`1"/> class.
        /// </summary>
        /// <param name="operationSucceeded">If set to <c>true</c> operation succeeded.</param>
        /// <param name="result">The result of the operation.</param>
        public static TryResult<TResult> Create<TResult>(bool operationSucceeded, TResult result)
        {
            return new TryResult<TResult>(operationSucceeded, result);
        }
    }

    /// <summary>
    /// Result with a boolean indicating if the operation succeeded.
    /// </summary>
    public class TryResult<TResult>
    {
        /// <summary>
        /// Gets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value><c>true</c> if operation succeeded; otherwise, <c>false</c>.</value>
        public bool OperationSucceeded { get; private set; }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        public TResult Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cirrious.MvvmCross.Plugins.File.TryResult`1"/> class.
        /// </summary>
        /// <param name="operationSucceeded">If set to <c>true</c> operation succeeded.</param>
        /// <param name="result">The result of the operation.</param>
        public TryResult(bool operationSucceeded, TResult result = default(TResult))
        {
            OperationSucceeded = operationSucceeded;
            Result = result;
        }
    }
}