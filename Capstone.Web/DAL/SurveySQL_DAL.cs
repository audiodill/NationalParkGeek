using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Capstone.Web.DAL
{
    public class SurveySQL_DAL : ISurvey
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["NationalParkDB"].ConnectionString;
        private const string SQL_SubmitSurvey = "INSERT survey_result VALUES (@parkcode, @email, @state, @activityLevel);";
        private const string SQL_FavoriteParks = "select survey_result.parkCode,Count(survey_result.parkCode) as 'surveyCount', park.parkName from survey_result join park on park.parkCode = survey_result.parkCode group by survey_result.parkCode, park.parkName having count('surveyCount') >1 order by surveyCount desc, park.parkName;";
        public List<Survey> GetFavoriteParks()
        {
            List<Survey> output = new List<Survey>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_FavoriteParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Survey s = new Survey();
                        s.ParkCode = Convert.ToString(reader["parkCode"]);
                        s.ParkName = Convert.ToString(reader["parkName"]);
                        s.SurveyCount = Convert.ToInt32(reader["surveyCount"]);

                        output.Add(s);                               
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }

        public bool SubmitSurvey(Survey input)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SubmitSurvey, conn);
                    cmd.Parameters.AddWithValue("@parkcode", input.ParkCode);
                    cmd.Parameters.AddWithValue("@email", input.Email);
                    cmd.Parameters.AddWithValue("@state", input.State);
                    cmd.Parameters.AddWithValue("@activityLevel", input.ActivityLevel);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
    }
}