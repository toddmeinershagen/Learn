using System;
using System.Linq.Expressions;

namespace Learn.FromRuby.Try
{
    public static class TryExtensions
    {
        /// <summary>
        /// Allows for a graceful try of a function call in case the object you are calling on is null.
        /// </summary>
        /// <remarks>
        /// This was found from Ruby and their try() method.  I read about it in an article found at http://6ftdan.com/allyourdev/2014/12/27/assume-everything-will-break-development/#HJIX
        /// Wonder if this is just a band-aid and avoids finding problems till later in the code.  Might be better to do a check for null and log that something is wrong.
        /// </remarks>
        /// <typeparam name="TInput">The type of the input value</typeparam>
        /// <typeparam name="TOutput">The type of the result</typeparam>
        /// <param name="value">The object that Try is being called on</param>
        /// <param name="function">The lambda function to call on the object that is being extended with Try</param>
        /// <returns></returns>
        public static TOutput Try<TInput, TOutput>(this TInput value, Func<TInput, TOutput> function)
            where TInput : class
        {
            try
            {
                if (value == null)
                    return default(TOutput);

                return function(value);

            }
            catch (NullReferenceException)
            {

                return default(TOutput);
            }

        }

        /// <summary>
        /// This is equivalent to the numerous Try methods within the .NET framework that will swallow exceptions and return a true or false for success.  The result parameter will hold the successful value.
        /// </summary>
        /// <remarks>
        /// After looking at the Try() from Ruby, I thought this extension would get rid of the need to implement all of the one off method implementations
        /// of TryMethodName within the .NET framework.
        /// </remarks>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="value"></param>
        /// <param name="function"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Try<TInput, TOutput>(this TInput value, Func<TInput, TOutput> function, out TOutput result)
            where TInput : class
        {
            return Handle.Try(() => value.Try(function), out result);
        }
    }

    public static class Handle
    {
        /// <summary>
        /// Tries the specified function and returns a true if successful, a false if not.
        /// </summary>
        /// <remarks>
        /// This allows for calling static functions in a graceful way.  If you have an object, you can just call Try() instead.  
        /// 
        /// For example, instead of
        /// <code>if (int.TryParse("0", out value)) { doSomething(value); }</code>
        /// you can just call
        /// <code>if (Handle.Try(() => int.Parse("0"), out value)) { doSomething(value); }</code>
        /// </remarks>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool Try<TOutput>(Func<TOutput> function, out TOutput result)
        {
            try
            {
                result = function();
                return true;
            }
            catch (Exception)
            {
                result = default(TOutput);
                return false;
            }
        }
    }
}