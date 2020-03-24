using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using ASPNetframework2.Models;

namespace ASPNetframework2.DataControllers
{
	public class BarrenLandDataController 
	{
		//private static Int32 TILE_SIZE = 1;
		private static Int32 WIDTH = 400;
		private static Int32 HEIGHT = 600;

		//private Dictionary<int, List<GridUnit>> grid = new Dictionary<int, List<GridUnit>>();
		private static GridUnit[,] grid = new GridUnit[WIDTH,HEIGHT];
		//private static String[] STDIN = { "0 292 399 307" };
		//private static String[] STDIN = {"48 192 351 207", "48 392 351 407", "120 52 135 547", "260 52 275 547"};

		/// <summary>
		/// Find total fertile land in grid based on String array of rectangle endpoints
		/// </summary>
		/// <returns>The fertile land areas' square unit count array</returns>
		public static LandInformation GetAllFertileLand(LandInformation landInfo)
		{
			try
			{
				LandInformation updatedLandInfo = new LandInformation(landInfo.BarrenLandInputString);
				List<Int32> fertileLand = new List<Int32>();
				String[] barrenLandStrings = CreateBarrenLandStrings(updatedLandInfo.BarrenLandInputString);

				List<Int32[]> barrenLandEndPoints = ConvertBarrenLandCoordinates(barrenLandStrings);

				List<GridUnit> allBarrenLand = new List<GridUnit>();
				//Fill/create all barrenLand individual grid units 
				foreach (Int32[] rectangle in barrenLandEndPoints)
				{
					allBarrenLand.AddRange(FillIndividualBarrenLandGridUnits(rectangle));
				}

				//Loop through bounds of the entire 400 x 600 grid
				//create grid units for the entire 400 x 600 grid
				//filling a multidimentional array with the grid units
				for (int y = 0; y < HEIGHT; y++)
				{
					for (int x = 0; x < WIDTH; x++)
					{
						GridUnit unit = new GridUnit(x, y);
						//check if grid unit is present in the BarrenLand list
						if (allBarrenLand.Exists(xy=> xy.X == x && xy.Y == y))
						{
							//mark that grid unit as barren and accounted for
							unit.IsBarren = true;
							unit.AccountedFor = true;
							break;
						}
						grid[x,y] = unit;
					}
				}

				fertileLand = CheckForUnaccountedAreasAndCountFertileLand(fertileLand, 0, 0);

				fertileLand.Sort();

				updatedLandInfo.FertileLandOutputString = "";

				if (fertileLand.Count > 0)
				{
					foreach (Int32 land in fertileLand)
					{
						updatedLandInfo.FertileLandOutputString += land.ToString() + " ";
					}
				}
				else
				{
					updatedLandInfo.FertileLandOutputString = "No fertile land available.";
				}

				 
				return updatedLandInfo;

			}
			catch (Exception e)
			{
				//Log.Error(e, "GetAllFertileLand()");
				return null;
			}
		}
		/// <summary>
		/// Converts the user entered string containing all barrn land coordinates into string array of barren land plots
		/// </summary>
		/// <param name="barrenLandInputString">single user entered string containing all barren land coordinates</param>
		/// <returns>string array of each barren land plot if any specified by user</returns>
		private static String[] CreateBarrenLandStrings(String barrenLandInputString)
		{
			String [] barrenLands = new String[0];
			try
			{
				barrenLands = barrenLandInputString.Split(',');
			}
			catch (Exception e)
			{
				//Log.Error(e, "CreateBarrenLandStrings()");
				
			}
			return barrenLands;
		}

		/// <summary>
		/// Convert each string of rectangle endpoints into a list of Int32 
		/// </summary>
		/// <param name="barrenCorners"></param>
		/// <returns>List of Int32 array of rectangle endpoint</returns>
		private static List<Int32[]> ConvertBarrenLandCoordinates(String[] barrenCorners)
		{
			try
			{
				//List of arrays of rectangle points 
				List<Int32[]> rectanglePoints = new List<Int32[]>();
				//        For each rectangle coordinates, split into array of strings, convert to array of ints, add array to list of arrays of rectangles 
				for (int h = 0; h < barrenCorners.Length; h++)
				{
					String[] strRectangleCorner = barrenCorners[h].Trim().Split(' ');
					Int32[] intRectangleCorner = new Int32[strRectangleCorner.Length];
					for (int i = 0; i < strRectangleCorner.Length; i++)
					{
						int iNum = -1;
						if (int.TryParse(strRectangleCorner[i], out iNum))
							intRectangleCorner[i] = iNum;
						else
							continue;
					}
					rectanglePoints.Add(intRectangleCorner);
				}

				return rectanglePoints;
			}
			catch (Exception e)
			{
				//Log.Error(e, "ConvertBarrenLandCoordinates()");
				return new List<Int32[]>(); 
			}
		}

		/// <summary>
		/// Create grid units for a barren land rectangle 
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns>List of grid units in barren land rectangle</returns>
		private static List<GridUnit> FillIndividualBarrenLandGridUnits(Int32[] bounds)
		{
			try
			{ 
				List<GridUnit> barrenLandGridUnits = new List<GridUnit>();
				if (bounds.Length < 4)
					return barrenLandGridUnits;
				//Loop through endpoints
				for (int i = bounds[0]; i <= bounds[2]; i++)
				{
					for (int j = bounds[1]; j <= bounds[3]; j++)
					{
						// create new grid unit for each coordinate within rectangle endpoints
						GridUnit barrenLandGridUnit = new GridUnit(i, j);
						//barrenLandGridUnit.setIsBarren(true);
						//then add to barrenLandGridUnits list
						barrenLandGridUnits.Add(barrenLandGridUnit);
					}
				}
				//return all grid units for input coordinates
				return barrenLandGridUnits;
			}
			catch (Exception e)
			{
				//Log.Error(e, "FillIndividualBarrenLandGridUnits()");
				return new List<GridUnit>();
			}
		}

		/// <summary>
		/// Check through grid, find first unaccounted for point, 
		/// fill the fertile area directly connected to that point, and return the total area
		/// </summary>
		/// <param name="land"></param>
		/// <param name="xValue"></param>
		/// <param name="yValue"></param>
		/// <returns>List of area of each fertile land plot</returns>
		private static List<Int32> CheckForUnaccountedAreasAndCountFertileLand(List<Int32> land, int xValue, int yValue)
		{
			try
			{
				for (int y = yValue; y < HEIGHT; y++)
				{
					for (int x = xValue; x < WIDTH; x++)
					{
						GridUnit tile = grid[x,y];
						if (!tile.AccountedFor)
						{
							int totalFertileArea = FillFertileLandAreas(x, y);
							land.Add(totalFertileArea);
							CheckForUnaccountedAreasAndCountFertileLand(land, x, y);
						}
					}
				}
				return land;
			}
			catch (Exception e)
			{
				//Log.Error(e, "CheckForUnaccountedAreasAndCountFertileLand()");
				return new List<Int32>();
			}

		}

		/// <summary>
		/// Find the area of all grid units in a fertile land space 
		/// </summary>
		/// <param name="x"> width coordinate</param>
		/// <param name="y">height coordinate</param>
		/// <returns>Area (int) of the current fertile land space</returns>
		private static int FillFertileLandAreas(int x, int y)
		{
			int count = 0; // Count of grid squares being accounted for

			Stack<GridUnit> stack = new Stack<GridUnit>();

			try {

				stack.Push(new GridUnit(x, y));

				while (stack.Count > 0)
				{
					GridUnit gUnit = stack.Pop();

					//If GriUnit gUnit is not yet accounted for, then account for it, 
					//increase count by 1, and add neighbors to the stack;
					if (!IsGridUnitAccountedFor(gUnit))
					{
						count += 1;
						//if the neighbor unit below is not out of bounds and is not yet accounted for then process it
						if (gUnit.Y - 1 >= 0 && !grid[gUnit.X,gUnit.Y - 1].AccountedFor)
						{
							stack.Push(new GridUnit(gUnit.X, gUnit.Y - 1));
						}
						//if the neighbor unit above is not out of bounds and is not yet accounted for then process it
						if (gUnit.Y + 1 < HEIGHT && !grid[gUnit.X,gUnit.Y + 1].AccountedFor)
						{
							stack.Push(new GridUnit(gUnit.X, gUnit.Y + 1));
						}
						//if the neighbor unit to the left is not out of bounds and is not yet accounted for then process it
						if (gUnit.X - 1 >= 0 && !grid[gUnit.X - 1,gUnit.Y].AccountedFor)
						{
							stack.Push(new GridUnit(gUnit.X - 1, gUnit.Y));
						}
						//if the neighbor unit to the rgiht is not out of bounds and is not yet accounted for then process it
						if (gUnit.X + 1 < WIDTH && !grid[gUnit.X + 1,gUnit.Y].AccountedFor)
						{
							stack.Push(new GridUnit(gUnit.X + 1, gUnit.Y));
						}
					}

				}
				return count;
			}
			catch (Exception e)
			{
				//Log.Error(e, "FillFertileLandAreas()");
				return -1;
			}
			
		}

		/// <summary>
		/// Check if gridUnit has been accounted for already - if not, switch accounted for to true
		/// </summary>
		/// <param name="gUnit"></param>
		/// <returns>bool representing whether grid unit has been accounted for previously or not</returns>
		private static Boolean IsGridUnitAccountedFor(GridUnit gUnit)
		{
			try
			{
				//        Check that grid unit is not outside bounds of the grid
				if (gUnit.X < 0 || gUnit.Y < 0 || gUnit.X >= WIDTH || gUnit.Y >= HEIGHT)
				{
					//outside of bounds
					return true;
				}

				GridUnit coordinateToCheck = grid[gUnit.X, gUnit.Y];

				if (coordinateToCheck.AccountedFor)
				{
					//already account for
					return true;
				}

				coordinateToCheck.AccountedFor = true;
				//had not yet been accounted for
				return false;
			}
			catch (Exception e)
			{
				//Log.Error(e, "IsGridUnitAccountedFor()");
				return true;
			}
		}
	}
}
