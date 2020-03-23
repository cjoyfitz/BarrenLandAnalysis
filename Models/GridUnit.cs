using System;
using System.Web;

namespace ASPNetframework2.Models
{
	public class GridUnit 
	{
		
		private Int32 x;
		private Int32 y;
		private Boolean isBarren = false;
		private Boolean accountedFor = false;
		public GridUnit()
		{

		}

		public GridUnit(Int32 xVal, Int32 yVal)
		{
			x = xVal;
			y = yVal;
			isBarren = false;
			accountedFor = false;
		}

		public GridUnit(Int32 xVal, Int32 yVal, Boolean isBarrenVal, Boolean isAcctFor)
		{
			x = xVal;
			y = yVal;
			isBarren = isBarrenVal;
			accountedFor = isAcctFor;
		}

		public Int32 X
		{
			get { return x; }
			set { x = value; }
		}
		public Int32 Y
		{
			get{ return y; }
			set { y = value; }
		}
		public Boolean IsBarren
		{
			get { return isBarren; }
			set { isBarren = value; }
		}

		

		public Boolean AccountedFor
		{
			get { return accountedFor; }
			set { accountedFor = value; }
		}
	}

}
