

using System;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;


namespace Scrummer.Common.Helpers
{
    /// <summary>
    ///  Helper class for throwing exceptions from controller.
    /// </summary>
    public class ExceptionHelpers
    {
        private string _errorMessage = string.Empty;
        private HttpResponseMessage _responseMessage = null;

        /// <summary>
        /// Handles HttpResponseMessage wrapping for an exception
        /// </summary>
        /// <param name="ex">exception parameter</param>
        /// <param name="errorMessage">error message to throw</param>
        public HttpResponseMessage ThrowExceptionMessage(Exception ex, string errorMessage)
        {
            string innerException = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
            errorMessage = string.Format("{0} Message: {1}", errorMessage, innerException);

            Trace.WriteLine(errorMessage, ExceptionHelpersConstants.TraceError);
            this._errorMessage = errorMessage;
            this._responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpError = new HttpError(this._errorMessage);
            this._responseMessage.Content = new ObjectContent<HttpError>(httpError, new JsonMediaTypeFormatter(), ExceptionHelpersConstants.ApplicationJson);
            return this._responseMessage;
        }

        /// <summary>
        /// Logs the ErrorMessage/s in Trace Log file
        /// </summary>
        /// <param name="validation">string</param>
        /// <param name="errrorDescription">string</param>
        /// <param name="modelStates">ModelStateDictionary</param>
        public void LogValidationErrorMessage(string validation, string errrorDescription, ModelStateDictionary modelStates)
        {
            string errorMessage = string.Empty;

            foreach (ModelState model in modelStates.Values)
            {
                if (model.Errors.Any())
                {
                    errorMessage = string.Format("{0} , {1}", errorMessage, model.Errors.FirstOrDefault().ErrorMessage);
                }
            }

            Trace.WriteLine(string.Format("{0} Details: {1}{2}", validation, errrorDescription, errorMessage), ExceptionHelpersConstants.TraceError);
        }

        #region Constants

        /// <summary>
        /// Constants class
        /// </summary>
        private class ExceptionHelpersConstants
        {
            internal const string ApplicationJson = "application/json";
            internal const string TraceError = "Error";
        }

        #endregion
    }
}
