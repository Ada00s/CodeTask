using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WebService.Models
{
	public class Payload
	{
		[JsonProperty(Required = Required.Always)]
		public string Ts { get; set; }

		[JsonProperty(Required = Required.Always)]
		public string Sender { get; set; }

		[JsonProperty(Required = Required.Always)]
		public object Message { get; set; }

		[JsonProperty(Required = Required.DisallowNull)]
		public string SentFromIp { get; set; }

		[JsonProperty(Required = Required.DisallowNull)]
		public string Priority { get; set; }

		public bool IsValid(out List<ValidationError> validationErrors)
		{
			validationErrors = new List<ValidationError>();

			if (!ValidateTs(Ts))
			{
				validationErrors.Add(new ValidationError(ValidationErrorType.TimestampValidationError, $"Provided string ({Ts}) is not valid timestamp"));
			}

			if (string.IsNullOrWhiteSpace(Sender))
			{
				validationErrors.Add(new ValidationError(ValidationErrorType.InvalidSenderError, $"Provided sender ({Sender}) is invalid"));
			}

			if (!ValidateMessage(Message))
			{
				validationErrors.Add(new ValidationError(ValidationErrorType.InvalidMessageError, $"Provided message ({Message}) is invalid"));
			}

			if (!ValidateIP(SentFromIp)) 
			{
				validationErrors.Add(new ValidationError(ValidationErrorType.InvalidSenderIPError, $"Provided IP address ({SentFromIp}) is invalid"));
			}

			if (validationErrors.Count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		private bool ValidateTs(string ts)
		{
			if (Int32.TryParse(ts, out var ms)) //Make sure it's valid number
			{
				if (DateTimeOffset.Now.ToUnixTimeSeconds() - ms >= 0) //make sure it's not 'future' time
				{
					return true;
				}
			}
			return false;
		}

		private bool ValidateMessage(object messageObj)
		{
			try
			{
				var message = JsonConvert.SerializeObject(messageObj);
				if (message.Count(s=>s==':')>1)
				{
					return true;

				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool ValidateIP(string ip)
		{
			if(ip.Count(s => s == '.') == 3)
			{
				if(IPAddress.TryParse(ip, out var temp))
				{
					return true;
				}
			}
			return false;
		}
	}
}
