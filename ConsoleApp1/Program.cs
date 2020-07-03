using EnglishStemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using PorterStemmer;
namespace ConsoleApp1
{
    public class Program
    {
        public static string PositiveReview;
        public static string NegativeReview;
        public static string[] Positvecorpa, Negativecorpa;
        public static object[][] WordMatrix = new object[3][];
        static int sum = 0;
        static void Main(string[] args)
        {
            PositiveReview = Reviews.GetReviews("pos");
            NegativeReview = Reviews.GetReviews("neg");

            Positvecorpa = PreprocessingAndTokenizing(PositiveReview);
            Negativecorpa = PreprocessingAndTokenizing(NegativeReview);
            var corpus = Positvecorpa.Concat(Negativecorpa).ToArray();
            Console.WriteLine("Loading Data... Please wait for 5 minutes");
            TextToFeature(corpus);
            while (true)
            {
                Console.WriteLine("Enter a review to test if it is Positive or Negative ?");
                string test = Console.ReadLine();
                var t = PreprocessingAndTokenizing(test);
                if (NaiveBayesPrediction(t) == false)
                    Console.WriteLine("-ve");
                else
                    Console.WriteLine("+ve");

            }
        }

        public static string[] PreprocessingAndTokenizing(string raw)
        {
            var str = raw.Split(" .!,".ToCharArray());
            var tokens = str.Except(StopWords.stopWordsList).ToArray();
            var st = new PorterStemmer.Stemmers.EnglishStemmer();
            return tokens.ToArray();
        }
        public static void TextToFeature(string[] data)
        {
            var DistinctWords = data.Distinct().ToArray();
            WordMatrix[0] = new string[DistinctWords.Count()];
            for (int i = 1; i < WordMatrix.Length; i++)
            {
                WordMatrix[i] = new object[DistinctWords.Count()];
            }
            int k = 0;
            foreach (var item in DistinctWords)
            {
                WordMatrix[0][k++] = item;
            }

            var posraw = PositiveReview.Split(" .!,".ToCharArray());
            var negraw = NegativeReview.Split(" .!,".ToCharArray());
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < WordMatrix[i].Length; j++)
                {
                    WordMatrix[i + 1][j] = (from x1 in posraw where string.Equals(x1.ToString(), (string)WordMatrix[0][j], StringComparison.OrdinalIgnoreCase) select x1).Count().ToString();
                }
            }

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < WordMatrix[i].Length; j++)
                {
                    WordMatrix[i + 2][j] = (from x1 in negraw where string.Equals(x1.ToString(), (string)WordMatrix[0][j], StringComparison.OrdinalIgnoreCase) select x1).Count().ToString();
                }
            }

            for (int i = 1; i < WordMatrix.Length; i++)
            {
                for (int j = 0; j < WordMatrix.Length; j++)
                {
                    sum += Convert.ToInt32(WordMatrix[i][j]);
                }
            }
        }
        public static bool NaiveBayesPrediction(string[] input)
        {
            double PositivePrediction, NegativePrediction;
            PositivePrediction = NegativePrediction = Convert.ToDouble(.5);
            for (int i = 0; i < input.Length; i++)
            {
                PositivePrediction *= CalculateProbability(input[i], 1);
                NegativePrediction *= CalculateProbability(input[i], 0);
            }
            Console.WriteLine("Positive Prediction" + Convert.ToDecimal(PositivePrediction));
            Console.WriteLine("Negative Prediction" + Convert.ToDecimal(NegativePrediction));
            if (PositivePrediction > NegativePrediction)
                return true;
            else
                return false;
        }
        public static double CalculateProbability(string data, int flag)
        {
            int i = 0;
            int val = 0;
            double result;
            for (; i < WordMatrix[0].Length; i++)
            {
                if (string.Equals((string)WordMatrix[0][i], data, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
            if (i == WordMatrix[0].Length)
            {
                result = (1.0) / Convert.ToDouble(sum);
                return result;
            }
            if (flag == 1)
            {
                val = Convert.ToInt32(WordMatrix[1][i]);
            }
            else if (flag == 0)
            {
                val = Convert.ToInt32(WordMatrix[2][i]);
            }
            result = (Convert.ToDouble(val) + 1.0) / Convert.ToDouble(sum);
            return result;
        }
    }
}
