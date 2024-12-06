using System;

public class Event
{
	public int Costs [][];
	public DateTime startDate { get; set; }
	public DateTime endDate { get; set; }

	public Event(startDate, endDate)
	{
		Costs = new int[4][6];
		this.startDate = startDate;
		this.endDate = endDate;
	}
}
