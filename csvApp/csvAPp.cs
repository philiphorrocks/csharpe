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

        try
        {


            IEnumerable<String> lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {

                var fields = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                DataRow newRow = dataTable.Rows.Add();
                newRow.ItemArray = fields;

            }
        }
        catch
        {

            Console.WriteLine("Error reading from file");
        }

            return dataTable;
    }
    

    public static void printTable()
    {

        // Get instance of the DataTable.

        DataTable table = Data("tmp:/input.csv");


        foreach (DataRow row in table.Rows)
        { 
            //Console.WriteLine ("--- Row ---"); // Print separator.
            
            foreach (var item in row.ItemArray)
            { 
                //Console.WriteLine (item); // Invokes ToString abstract method.

            }
        }
    }


    static int timeDiff (List<int> tList)
    {
        
        int max = Int32.MinValue;
        int delta = 0;

        for (int i = 1; i < tList.Count; i++) 
        {
             delta = tList[i] - tList[i - 1];
             //Console.WriteLine (delta);

             if (delta > max) max = delta; 
                      
        }
            Console.WriteLine("Max Time: " + delta);
        
        return delta;

        //Console.Read(); // Pause.
 
   
    }
    

    //Process the trade data
    static void queryData(string symbol)
    {

        //Lists to hosd data

        List<int> vList = new List<int>();
        List<int> pList = new List<int>();
        List<int> tList = new List<int>();



        // Get instance of the DataTable.

        DataTable table = Data("D:/input.csv");

        Console.WriteLine("Symbol: " + symbol);

        ///SQL SELECT Statement using LINQ

            var volumeList = (from sq in table.AsEnumerable()

                              where sq.Field<string>("symb") == symbol
                              select
                              sq.Field<int>("quant")

                              ).ToList();


            var priceList = (from sq in table.AsEnumerable()

                             where sq.Field<string>("symb") == symbol
                             select
                             sq.Field<int>("price")

                             ).ToList();


            var timeList = (from sq in table.AsEnumerable()

                            where sq.Field<string>("symb") == symbol
                            select
                           sq.Field<int>("time")

                            ).ToList();



        //Loop through the data in the list

        foreach (var volume in volumeList)
        {
            //Console.WriteLine(volume);

            vList.Add(volume);
        }

        foreach (var price in priceList)
        {
            //Console.WriteLine(string.Format("{0}", price));

            pList.Add(price);

        }

        foreach (var time in timeList)
        {

            //Console.WriteLine(string.Format("{0}", time));

            tList.Add(time);

         }


        //Calculate Total Volume
        int vtotal = vList.Sum();
        Console.WriteLine("Total Volume:" + vtotal);

        ////Calculate Max Price
        int mtotal = pList.Max();
        Console.WriteLine("Max Price:" + mtotal);

        ////Calculate the max time between trades (Standard Deviation)
        var dTime = timeDiff(tList);
        Console.WriteLine("Max time " + dTime);

        //Calculate Weighted Average Price
        var wightAv = weightedAverage(pList, vList);
        Console.WriteLine("Weighted Average " + wightAv);

        Console.WriteLine("\n");

        //Add Values to List and update it

    }



    //Loop thorough each unique trade
    public static List<string> getUnique()
    {

        DataTable table = Data("D:/input.csv");

        //Create list to hold unique values

        List<string> uList = new List<string>();

        //Linq query

        var symList = (from sq in table.AsEnumerable()

                       select sq.Field<string>("symb")

                       ).ToList().Distinct();

        try
        {

            //get unique symbols
            foreach (var uniq in symList)
            {
                uList.Add(uniq);
                queryData(uniq);
                
            }

        }

        catch (System.IO.IOException e)
        {

            Console.WriteLine("Error reading from {0}. Message = {1}", e.Message);
        }

        //Console.WriteLine("Unique symbols: " + uList.Count());
        return uList;
    }



    static int weightedAverage(List<int> pList, List<int> vList)
    {
        List<int> weightMultiplyList = new List<int>();

        foreach (var pair in pList.Zip(vList, (a, b) => new { A = a, B = b }))
        {
            //Calculate Average Weighting
            int multVal = pair.A  * pair.B;
            weightMultiplyList.Add(multVal);
            
        }

        weightMultiplyList.Sum();
        vList.Sum();

        return weightMultiplyList.Sum() / vList.Sum();
    }
   
	        



    ///////////Main Program calls the methods///////////
    /// <Process Trade Application>
    /// ///////////////////////////////////////////////
   
    static void Main()
    {

        //Method to get otuput of table data     
        getUnique();
        Console.Read(); // Pause.
    }
}
