using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebService.Models
{
	/// <summary>
	/// Model class for throwing validation errors.
	/// </summary>
	public class ValidationError
	{
		public ValidationErrorType ErrorType { get; set; }
		public string Message { get; set; }

		/// <summary>
		/// Constructor used for throwing validation errors.
		/// </summary>
		/// <param name="errorType">Type of error that occured</param>
		/// <param name="message">More detailed information about error that occured</param>
		public ValidationError(ValidationErrorType errorType, string message)
		{
			ErrorType = errorType;
			Message = message;
		}
	}

	/// <summary>
	/// Predefined error type.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ValidationErrorType
	{
		TimestampValidationError,
		InvalidSenderError,
		InvalidMessageError,
		InvalidSenderIPError
	}

	public static class ValidationErrorExtensions
	{
		public static string CreateMessage(this List<ValidationError> list)
		{
			var message = "Error(s) occured while validating the payload.\n";
			foreach (ValidationError e in list)
			{
				message += e.Message + "\n";
			}
			return message;
		}
	}
}
