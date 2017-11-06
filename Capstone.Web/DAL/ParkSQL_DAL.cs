using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Capstone.Web.DAL
{
    public class ParkSQL_DAL : IParkDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["NationalParkDB"].ConnectionString;
        private const string SQL_GetAllParks = "SELECT * FROM park;";
        private const string SQL_GetPark = "SELECT * FROM park WHERE parkCode = @parkcode";
        private const string SQL_GetWeather = "SELECT * FROM weather WHERE parkCode = @parkcode;";

        public List<Weather> GetFiveDayForecast(string id, string degree)
        {
            decimal equation = 5 / 9m;
            List<Weather> output = new List<Weather>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetWeather, conn);
                    cmd.Parameters.AddWithValue("@parkcode", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Weather w = new Weather();
                        w.ParkCode = Convert.ToString(reader["parkCode"]);
                        w.FiveDateForecastValue = Convert.ToInt32(reader["fiveDayForecastValue"]);
                        w.Low = Convert.ToInt32(reader["low"]);
                        w.High = Convert.ToInt32(reader["high"]);
                        w.Forecast = Convert.ToString(reader["forecast"]).Replace(" ", string.Empty);
                        w.Advisory = WeatherAdvisory(w.High, w.Low, w.Forecast);

                        if (degree == "Celsius")
                        {
                            decimal low = Convert.ToDecimal(w.Low);
                            decimal high = Convert.ToDecimal(w.High);
                            low = (low - 32) * equation;
                            high = (high - 32) * equation;
                            w.Low = Convert.ToInt32(low);
                            w.High = Convert.ToInt32(high);
                            w.DegreeType = "C";
                        }
                        else
                        {
                            w.DegreeType = "F";
                        }
                        output.Add(w);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }

        public List<Park> GetAllParks()
        {
            List<Park> output = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllParks, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkCode = Convert.ToString(reader["parkCode"]);
                        p.ParkName = Convert.ToString(reader["parkName"]);
                        p.State = Convert.ToString(reader["state"]);
                        p.Acreage = Convert.ToInt32(reader["acreage"]);
                        p.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
                        p.MilesOfTrail = Convert.ToDecimal(reader["milesOfTrail"]);
                        p.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
                        p.Climate = Convert.ToString(reader["climate"]);
                        p.YearFounded = Convert.ToInt32(reader["yearFounded"]);
                        p.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
                        p.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
                        p.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                        p.ParkDescription = Convert.ToString(reader["parkDescription"]);
                        p.EntryFee = Convert.ToInt32(reader["entryFee"]);
                        p.NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);

                        output.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }

        public Park GetPark(string id)
        {
            Park p = new Park();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetPark, conn);
                    cmd.Parameters.AddWithValue("@parkcode", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        p.ParkCode = Convert.ToString(reader["parkCode"]);
                        p.ParkName = Convert.ToString(reader["parkName"]);
                        p.State = Convert.ToString(reader["state"]);
                        p.Acreage = Convert.ToInt32(reader["acreage"]);
                        p.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
                        p.MilesOfTrail = Convert.ToDecimal(reader["milesOfTrail"]);
                        p.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
                        p.Climate = Convert.ToString(reader["climate"]);
                        p.YearFounded = Convert.ToInt32(reader["yearFounded"]);
                        p.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
                        p.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
                        p.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                        p.ParkDescription = Convert.ToString(reader["parkDescription"]);
                        p.EntryFee = Convert.ToInt32(reader["entryFee"]);
                        p.NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return p;
        }

        public List<string> WeatherAdvisory(int high, int low, string forecast)
        {
            List<string> output = new List<string>();

            if(forecast == "snow")
            {
                output.Add("Make sure you pack your snow shoes!");
            }
            if(forecast == "rain")
            {
                output.Add("Make sure you pack your rain gear and wear waterproof shoes!");
            }
            if(forecast == "thunderstorms")
            {
                output.Add("We advise you to seek shelter and avoid hiking on exposed ridges!");
            }
            if(forecast == "sunny")
            {
                output.Add("Make sure you pack sunblock!");
            }
            if(high > 75)
            {
                output.Add("Make sure you bring an extra gallon of water!");
            }
            if((high - low) > 20)
            {
                output.Add("Make sure you wear breathable layers!");
            }
            if(low < 20)
            {
                output.Add("Be aware of possible exposure to frigid temperatures!");
            }
            return output;
        }

    }
}