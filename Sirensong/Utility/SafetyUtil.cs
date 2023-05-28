using System;

namespace Sirensong.Utility
{
    public static class SafetyUtil
    {
        /// <summary>
        ///     Wraps a value or an exception to safely handle exceptions.
        /// </summary>
        /// <typeparam name="TValue">The value type that has been wrapped.</typeparam>
        /// <typeparam name="TException">The exception from the value failing to be retrieved.</typeparam>
        public readonly struct OptionWrap<TValue, TException> where TException : Exception
        {
            /// <summary>
            ///     The value if it exists.
            /// </summary>
            private TValue? Value { get; }

            /// <summary>
            ///     The exception if it exists.
            /// </summary>
            private TException? Exception { get; }

            /// <summary>
            ///     The value.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="exception"></param>
            /// <param name="hasValue"></param>
            internal OptionWrap(TValue? value, TException? exception)
            {
                this.Value = value;
                this.Exception = exception;
            }

            /// <summary>
            ///     Whether or not the value exists.
            /// </summary>
            public bool HasValue => this.Value != null;

            /// <summary>
            ///     Returns the value if it exists, otherwise returns the alternative.
            /// </summary>
            /// <param name="alternative">The alternative value.</param>
            /// <returns>The value or the alternative.</returns>
            public TValue ValueOr(TValue alternative) => this.Value ?? alternative;
            /// <summary>
            ///     Returns the value if it exists, otherwise returns the alternative.
            /// </summary>
            /// <param name="alternative">The alternative value factory.</param>
            /// <returns>The value or the alternative.</returns>
            public TValue ValueOr(Func<TValue> alternative) => this.Value ?? alternative();

            /// <summary>
            ///     Returns the exception if it exists, otherwise returns the alternative.
            /// </summary>
            /// <param name="alternative">The alternative exception.</param>
            /// <returns>The exception or the alternative.</returns>
            public TException ExceptionOr(TException alternative) => this.Exception ?? alternative;

            /// <summary>
            ///     Returns the exception if it exists, otherwise returns the alternative.
            /// </summary>
            /// <param name="alternative">The alternative exception factory.</param>
            /// <returns>The exception or the alternative.</returns>
            public TException ExceptionOr(Func<TException> alternative) => this.Exception ?? alternative();

            /// <summary>
            ///     Unwraps the option, returning the value if it exists, otherwise throwing the exception.
            /// </summary>
            /// <returns>The value.</returns>
            public TValue? Unwrap() => this.Value ?? throw this.Exception!;
        }

        /// <summary>
        ///     Runs a function safely, returning either the result or the exception.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="func">The function to run.</param>
        /// <returns>The result of the function or the exception.</returns>
        public static OptionWrap<TValue, TException> RunSafely<TValue, TException>(Func<TValue> func) where TException : Exception
        {
            try
            {
                return new OptionWrap<TValue, TException>(func(), default);
            }
            catch (Exception e)
            {
                SirenLog.Warning($"An exception was thrown while running {func.Method.Name} safely: {e.Message}");
                return new OptionWrap<TValue, TException>(default, (TException)e);
            }
        }

        /// <summary>
        ///     Runs the given action safely, logging any exceptions that are thrown but not rethrowing them.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public static void RunSafely(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                SirenLog.Warning($"An exception was thrown while running {action.Method.Name} safely: {e.Message}");
                // ignored
            }
        }
    }
}
