﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAppV2
{
    public partial class CaptureView : UserControl
    {
        static string request;
        public CaptureView()
        {
            InitializeComponent();
            List<ForecastModel> forecasts = WeatherDbDataAccess.getAllForecasts();
            mapForecastsToGrid(forecasts);
            setFieldsEditable(false);
        }
        public void resetFields()
        {
            cmbCitySelector.Text = "Select a City..";
            datepicker.ResetText();
            cmbConditions.Text = "Select Conditions..";
            maxTempSelector.Value = 0;
            minTempSelector.Value = 0;
            numWindSpeed.Value = 0;
            numHumidity.Value = 0;
            numPrecipitation.Value = 0;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Declare variables
            string cityName = "";
            DateTime selectedDate = DateTime.Today;
            string condition = "";
            int maxTemp = 0;
            int minTemp = 0;
            int windspeed = 0;
            int humidity = 0;
            int precipitation = 0;
            //Get values from interface
            try
            {

                ForecastModel weather = new ForecastModel
                {
                    city = cmbCitySelector.SelectedItem.ToString(),
                    date = datepicker.Value.Date,
                    condition = cmbConditions.SelectedItem.ToString(),
                    precipitation = (int)numPrecipitation.Value,
                    maxTemp = (int)maxTempSelector.Value,
                    minTemp = (int)minTempSelector.Value,
                    windspeed = (int)numWindSpeed.Value,
                    humidity = (int)numHumidity.Value
                };

                WeatherDbDataAccess.CreateForecast(weather);

                //Display success to the user
                MessageBox.Show("Successfully Captured \n" +
                    "-------------------------\n" +
                      "City: " + cityName+ "\n" +
                      "Date: " + selectedDate + "\n" +
                      "Conditions: " + condition + " \n" +
                      "Precipitation Chance: " + precipitation + "% \n" +
                      "Max Temp: " + maxTemp + "°C \n" +
                      "Min Temp: " + minTemp + "°C \n" +
                      "Windpeed: " + windspeed + "Knot \n" +
                      "Humidity: " + humidity + "% \n" +
                      "-------------------------");

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("An error occured in your submission, Please ensure all fields were filled out");
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Maximum number of a Forecasts reached");
            }
            resetFields();

            List<ForecastModel>forecasts = WeatherDbDataAccess.getAllForecasts();
            mapForecastsToGrid(forecasts);
        }

        public void mapForecastsToGrid(List<ForecastModel> forecasts)
        {
            forecastModelBindingSource.Clear();
            foreach (var forecast in forecasts)
            {
                Console.WriteLine("Max temp: " +forecast.maxTemp);
                forecastModelBindingSource.Add(forecast);
            }
        }

        private void setFieldsEditable(Boolean active)
        {
            datepicker.Enabled = active;
            cmbCitySelector.Enabled = active;
            cmbConditions.Enabled = active;
            numPrecipitation.Enabled = active;
            maxTempSelector.Enabled = active;
            minTempSelector.Enabled = active;
            numWindSpeed.Enabled = active;
            numHumidity.Enabled = active;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            request = "POST";
            setFieldsEditable(true);
            forecastModelBindingSource.AddNew();
            forecastModelBindingSource.MoveLast();
            cmbCitySelector.Focus();
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            request = "PATCH";
            setFieldsEditable(true);
            cmbCitySelector.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to delete?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ForecastModel report = new ForecastModel
                {
                    city = cmbCitySelector.SelectedItem.ToString(),
                    date = datepicker.Value.Date,
                    condition = cmbConditions.SelectedItem.ToString(),
                    precipitation = (int)numPrecipitation.Value,
                    maxTemp = (int)maxTempSelector.Value,
                    minTemp = (int)minTempSelector.Value,
                    windspeed = (int)numWindSpeed.Value,
                    humidity = (int)numHumidity.Value,
                };
               
                WeatherDbDataAccess.deleteRecord(report);
                List<ForecastModel> forecasts = WeatherDbDataAccess.getAllForecasts();
                mapForecastsToGrid(forecasts);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get values from interface
            try
            {

                ForecastModel weather = new ForecastModel
                {
                    city = cmbCitySelector.SelectedItem.ToString(),
                    date = datepicker.Value.Date,
                    condition = cmbConditions.SelectedItem.ToString(),
                    precipitation = (int)numPrecipitation.Value,
                    maxTemp = (int)maxTempSelector.Value,
                    minTemp = (int)minTempSelector.Value,
                    windspeed = (int)numWindSpeed.Value,
                    humidity = (int)numHumidity.Value
                };
                if(request == "PATCH")
                {
                    WeatherDbDataAccess.editRecord(weather);
                }
                else
                {
                    WeatherDbDataAccess.CreateForecast(weather);
                }

                //Display success to the user
                MessageBox.Show("Successfully Captured \n" +
                    "-------------------------\n" +
                      "City: " + weather.city + "\n" +
                      "Date: " + weather.date + "\n" +
                      "Conditions: " + weather.condition + " \n" +
                      "Precipitation Chance: " + weather.precipitation + "% \n" +
                      "Max Temp: " + weather.maxTemp + "°C \n" +
                      "Min Temp: " + weather.minTemp + "°C \n" +
                      "Windpeed: " + weather.windspeed + "Knot \n" +
                      "Humidity: " + weather.humidity + "% \n" +
                      "-------------------------");

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("An error occured in your submission, Please ensure all fields were filled out");
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Maximum number of a Forecasts reached");
            }

            List<ForecastModel> forecasts = WeatherDbDataAccess.getAllForecasts();
            mapForecastsToGrid(forecasts);
            setFieldsEditable(false);

        }
    }
    
}