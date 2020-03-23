using System;
using System.Web;

namespace ASPNetframework2.Models
{
	public class LandInformation 
	{
		
		private String fertileLandOutputString;

		private String barrenLandInputString;
		public LandInformation()
		{
			barrenLandInputString = String.Empty;
			fertileLandOutputString = String.Empty;
		}

		public LandInformation(String barrenLand)
		{
			barrenLandInputString = barrenLand;
			fertileLandOutputString = String.Empty;
		}

		public LandInformation(String barrenLand, String fertileLand)
		{
			barrenLandInputString = barrenLand;
			fertileLandOutputString = fertileLand;
		}

		public String FertileLandOutputString
		{
			get { return fertileLandOutputString; }
			set { fertileLandOutputString = value; }
		}

		public String BarrenLandInputString
		{
			get { return barrenLandInputString; }
			set { barrenLandInputString = value; }
		}

	}

}
