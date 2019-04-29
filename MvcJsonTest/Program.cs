using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MvcJsonTest
{
    
    class Movies
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<MovieDetails> data { get; set; }
    }

    class MovieDetails
    {
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string imdbID { get; set; }
    }
       
    
    class Program
    {
        //   https://jsonmock.hackerrank.com/api/movies/search/?Title=spiderman&page=2

        static List<string> movieTitles = new List<string>();

        static List<string> getMovieTitles(string searchKey="")
        {
                int total_records = 0;
                int total_pages = 0;
                int current_page = 1;

                //perform the operation atleast once and then repeat untill criteria matches
                do
                {
                    String baseUrl = $"https://jsonmock.hackerrank.com/api/movies/search/?Title={searchKey}&page={current_page}";

                    var request = (HttpWebRequest)WebRequest.Create(baseUrl);
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var responseObject = JsonConvert.DeserializeObject<Movies>(responseString);
                    if (responseObject != null && responseObject.total_pages > 0 && responseObject.total > 0)
                    {
                        total_pages = responseObject.total_pages;
                        total_records = responseObject.total;

                        if (responseObject.data != null && responseObject.data.Count > 0)
                        {
                            //collect all titles in array and increment page no
                            foreach (var item in responseObject.data)
                            {
                                movieTitles.Add(item.Title);
                            }
                        }
                        current_page++;
                    }
                }
                while (current_page<=total_pages);           

           return  movieTitles;

        }
        
        static void Main(string[] args)
        {
            //get the input search string
            Console.WriteLine("Please enter a search string.");
            string searchString = Console.ReadLine();
            Console.WriteLine();

            if(!String.IsNullOrWhiteSpace(searchString))
            {
                List<string> result = getMovieTitles(searchString.Trim());
                if (result.Count > 0)
                {
                    Console.WriteLine($"The total number of records that match the given string is: {result.Count} ");
                    Console.WriteLine();
                    //iterate through the list and print title
                    for(int i=0;i<result.Count;i++)
                    {
                        Console.WriteLine(i+1+".) " + result[i]);
                    }
                       
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid search string.");
            }
           
            Console.ReadKey();
        }
    }
}
