using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Handlers
{
	public class SqsHandler
	{
		private readonly ServiceConfig _config;
		private IAmazonSQS SqsClient { get; }

		public SqsHandler(ServiceConfig config)
		{
			_config = config;
			var sqsConfig = new AmazonSQSConfig
			{
				ServiceURL = config.ServiceUrl,
				LogMetrics = true,
				LogResponse = true,
				DisableLogging = false
			};

			SqsClient = new AmazonSQSClient(config.AccessKey, config.SecretKey, sqsConfig);
		}

		public async Task SendPayload(Payload payload)
		{
			var message = new SendMessageRequest()
			{
				QueueUrl = _config.QueueUrl,
				MessageBody = JsonConvert.SerializeObject(payload)
			};

			await SqsClient.SendMessageAsync(message);
		}
	}
}
