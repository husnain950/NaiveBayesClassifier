using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Reviews
    {
        public static string GetReviews(string label)
        {

            string review = "";
            var path = Directory.GetCurrentDirectory() + "\\movie_review1.csv"; 
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.ReadLine();
                if (label.Equals("pos", StringComparison.OrdinalIgnoreCase))
                {
                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        label = fields[0];
                        if (label.Equals("Pos", StringComparison.OrdinalIgnoreCase))
                            review += fields[1];
                        else
                            continue;
                    }
                }
                else
                {
                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        label = fields[0];
                        if (label.Equals("Neg", StringComparison.OrdinalIgnoreCase))
                            review += fields[1];
                        else
                            continue;
                    }
                }
                return review;
            }
        }
    }
}
