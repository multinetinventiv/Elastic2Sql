using System;

namespace Inventiv.Tools.Elastic2Sql.Models
{
	public class TransportHistory
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CreateDate { get; set; }
		public Result? Result { get; set; }
		public string Description { get; set; }
	}

	public enum Result
	{
		Fail = 0,
		Success = 1
	}
}
