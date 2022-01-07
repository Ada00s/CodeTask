using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
	public class CustomResult
	{
		public Payload Payload { get; set; }
		public bool Processed { get; set; }
		public DateTime ProcessedTime { get; set; }
	}
}
