using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Xml;


class Program

{
	
	public static DataTable Data(string filePath)

	{

	
		//Create datatable

		DataTable dataTable = new DataTable();

		dataTable.Columns.Add("time", typeof(int));
		dataTable.Columns.Add("symb", typeof(string));
		dataTable.Columns.Add("quant", typeof(int));
		dataTable.Columns.Add("price", typeof(int));



		//Loop through each line of the CSV file

		//initialising a StreamReader type variable and will pass the file location

		IEnumerable<String> lines = File.ReadAllLines(filePath);

		foreach (var line in lines)

		{

			var fields = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			DataRow newRow = dataTable.Rows.Add();
			newRow.ItemArray = fields;

		}

		return dataTable;

	}



	public static void printTable()

	{

		// Get instance of the DataTable.
	
		DataTable table = Data("tmp:/input.csv");


		foreach (DataRow row in table.Rows) { // Loop over the rows.
			Console.WriteLine ("--- Row ---"); // Print separator.
				foreach (var item in row.ItemArray) { // Loop over the items.
					Console.WriteLine (item); // Invokes ToString abstract method.

			}
		}
	}


	
	public static void WeightedPrice()

	{





	}

	
	public static void timeDiff()

	{





	}



	public static void queryData()

	{

		//Lists to hosd data

		List<int> vList = new List<int>();
		List<int> pList = new List<int>();
		List<int> tList = new List<int>();



		// Get instance of the DataTable.

		DataTable table = Data("/tmp/input.csv");


		///SQL SELECT Statement using LINQ

		var volumeList = (from sq in table.AsEnumerable()

		                  where sq.Field<string>("symb") == "aab"
		                  select              
		                  sq.Field<int>("quant")

		                  ).ToList();



		var priceList = (from sq in table.AsEnumerable()

		                 where sq.Field<string>("symb") == "aab"
		                 select
		                 sq.Field<int>("price")

		                 ).ToList();



		var timeList = (from sq in table.AsEnumerable()

		                where sq.Field<string>("symb") == "aab"
		                select
		               sq.Field<int>("time")

		                ).ToList();



		//Loop through the data in the list

		foreach (var volume in volumeList)
		{

			//Console.WriteLine(string.Format("{0}", volume));

			vList.Add(volume);


		}


		foreach (var price in priceList)
		{

			//Console.WriteLine(string.Format("{0}", price));

			pList.Add(price);

		}

		foreach (var time in timeList)
		{

			Console.WriteLine(string.Format("{0}", time));

			tList.Add(time);

		}


	
		//Calculate Total Volume

		int vtotal = vList.Sum();
		Console.WriteLine("Total Volume:" + vtotal);

		//Calculate Max Price

		int mtotal = pList.Max();
		Console.WriteLine("Max Price:" + mtotal);

		//Calculate the max time between trades

		//int ttoal = tList.Sort();

	}


	//Main Program calls the methods

	static void Main()
	{
		
		//Method to get otuput of table data     

		//printTable();      
		//Query Table

		queryData ();
		Console.Read (); // Pause.

	}
}
