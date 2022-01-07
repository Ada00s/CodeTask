using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebService.Handlers;
using WebService.Models;

namespace WebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ValidationController : ControllerBase
	{
		private readonly DbHandler _dbHandler;
		private readonly SqsHandler _sqsHandler;
		private const string DbTable = "PayloadHistory";

		public ValidationController(DbHandler dbHandler, SqsHandler sqsHandler)
		{
			_dbHandler = dbHandler;
			_sqsHandler = sqsHandler;
		}

		[HttpGet("/info")]
		public string Info()
		{
			return $"Code assignment web service.\n{DateTime.Now}";
		}

		[HttpGet("/ping")]
		public bool Ping()
		{
			return true;
		}

		[HttpPost]
		public async Task<ActionResult<CustomResult>> ProcessPayload([FromBody] Payload payload)
		{
			var validationErrors = new List<ValidationError>();
			if(!payload.IsValid(out validationErrors))
			{
				return StatusCode((int)HttpStatusCode.NotAcceptable, validationErrors.CreateMessage());
			}
			try
			{
				await _sqsHandler.SendPayload(payload);
				_dbHandler.InsertNonQuery($"Insert into {DbTable} (timestamp, sender, message, sent_from_ip, priority) values (@timestamp, @sender, @message, @sent_from_ip, @priority)",
					new Dictionary<string, object>
					{
						["@timestamp"] = payload.Ts,
						["@sender"] = payload.Sender,
						["@message"] = payload.Message,
						["@sent_from_ip"] = payload.SentFromIp,
						["@priority"] = payload.Priority
					});
				return new CustomResult { Payload = payload, Processed = true, ProcessedTime = DateTime.Now };
			}
			catch(Exception e)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
			}

		}
	}
}
