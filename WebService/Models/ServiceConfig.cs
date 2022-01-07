using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
	public class ServiceConfig
	{
		public string ServiceUrl { get; set; }
		public string QueueUrl { get; set; }
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
		public string DbConnection { get; set; }
	}
}
