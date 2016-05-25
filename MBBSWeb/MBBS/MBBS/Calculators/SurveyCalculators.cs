using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBBS.Database;

namespace MBBS.Calculators
{
    public class SurveyCalculators
    {
        public Dictionary<int, double> GetAverageRatingPerYear(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            Dictionary<int, double> results = new Dictionary<int, double>();
            Dictionary<int, List<int>> resultList = query.GetAllSurveyResults(moduleID);
            foreach(KeyValuePair<int, List<int>> year in resultList)
            {
                int yearTotal = 0;
                int yearNumber = 0;
                foreach(int rating in year.Value)
                {
                    if(rating != 0)
                    {
                        yearTotal += rating;
                        yearNumber++;
                    }
                }
                results.Add(year.Key, (double)yearTotal / (double)yearNumber);
            }
            return results;
        }

    }
}