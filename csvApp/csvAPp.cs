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
        catch (System.IO.IOException e)
            {
            
                Console.WriteLine("Error reading from {0}. Message = {1}", e.Message);
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



    static void timeDiff(List<int> tList)
    {
        int delta;

        List<int> maxTimeList = new List<int>();

        tList.Reverse();
        int num = 1;

        for (int i = 0; i < tList.Count; i++) // Loop through List with for
        {
            if (num >= tList.Count)
            {

                break;

            }

            else

             delta = (tList[i] - tList[num]);
             //Console.WriteLine (delta);
             num = num + 1;

             maxTimeList.Add(delta);

        }

        Console.WriteLine(maxTimeList.Max());
        Console.Read(); // Pause.
   
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

        //Console.WriteLine(symbol);

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


        try
        {
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

                    //Console.WriteLine(string.Format("{0}", time));

                    tList.Add(time);

                }
        }
        catch (System.IO.IOException e)
        {
            
            Console.WriteLine("Error reading from {0}. Message = {1}", e.Message);
        }

        
        //Calculate Total Volume
        int vtotal = vList.Sum();
        //Console.WriteLine("Total Volume:" + vtotal);

        //Calculate Max Price
        int mtotal = pList.Max();
        //Console.WriteLine("Max Price:" + mtotal);

        //Calculate the max time between trades (Standard Deviation)
        timeDiff(tList);
        

            
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
                //Console.WriteLine(string.Format("{0}", volume));
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


    ///////////Main Program calls the methods///////////

    static void Main()
    {

        //Method to get otuput of table data     
        getUnique();
        Console.Read(); // Pause.
    }
}
