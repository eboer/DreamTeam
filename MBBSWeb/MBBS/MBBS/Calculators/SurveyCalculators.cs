//===========================================================================================
//Project: MBBS
//Description:
//   Returns the average rating per year, per subsection per year, or per subsection. Ignores
//  where rating is 0 (which stands for 'not applicable').
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using System;
using System.Collections.Generic;
using MBBS.Database;

namespace MBBS.Calculators
{
    public class SurveyCalculators
    {
        /// <summary>
        /// Calculates the average of a list of ratings given.
        /// </summary>
        /// <param name="resultList"></param>
        /// <returns></returns>
        private Dictionary<object, double> CalculateRating(Dictionary <object, List<int>> resultList)
        {
            
            Dictionary<object, double> results = new Dictionary<object, double>();
            foreach(KeyValuePair<object, List<int>> year in resultList)
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
                double ratingResult = (double)yearTotal / (double)yearNumber;
                if(yearTotal != 0)
                {
                    results.Add(year.Key, Math.Round(ratingResult, 1));
                }
                
            }
            return results;
        }

        public Dictionary<object, double> GetAverageRatingPerYear(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetAllSurveyResults(moduleID));
        }

        public Dictionary<object, double> GetAverageRatingPerYearPerSubsection(string moduleID, int subsectionID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetSubsectionSurveyResults(moduleID, subsectionID));
        }

        public Dictionary<object, double> GetAverageRatingPerSubsection(string moduleID)
        {
            SurveyQueries query = new SurveyQueries();
            return CalculateRating(query.GetCurrentSubsectionSurveyResults(moduleID));
        }

    }
}