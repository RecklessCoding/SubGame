using System.IO;
using System.Text;
using UnityEngine;

public class LogfileWriter
{

    private string filePath;
    private string delimiter = ",";

    private static LogfileWriter instance;

    public static LogfileWriter GetInstance()
    {

        if (instance == null)
        {
            instance = new LogfileWriter();
        }

        return instance;
    }


    private LogfileWriter()
    {

    }

    internal void Createfile()
    {

        bool exists = System.IO.Directory.Exists(getPath());

        if(!exists)
        {
            System.IO.Directory.CreateDirectory(getPath());
        }

        filePath = getPath() +
            PlayerPrefs.GetString("DisplayName") + "_" + System.DateTime.Now.ToString("MM_dd_HH_mm") + ".csv";

        string header = "DaysPassed" + "," + "TotalPopulation" + "," + "TotalBabies" + "," + "DeathsAge" + "," + "DeathsPredator" + "," + "DeathsStarved" + ","
            + "TotalDeaths" + "," + "FoodTime" + "," + "BridgesTime" + "," + "HousesTime" + "," + "ProcreationTime" + "," + "AverageFoodTime" + ","
            + "AverageBridgesTime" + "," + "AverageHouseTime" + "," + "AverageProcreationTime";

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(header);
        outStream.Close();
    }

    internal void WriteLogline(int daysPassed, int totalPopulation, int totalBabies, int[] deaths, float[] times, float[] averages)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(daysPassed.ToString());
        sb.Append(",");
        sb.Append(totalPopulation.ToString());
        sb.Append(",");
        sb.Append(totalBabies.ToString());
        sb.Append(",");
        sb.Append(deaths[0].ToString());
        sb.Append(",");
        sb.Append(deaths[1].ToString());
        sb.Append(",");
        sb.Append(deaths[2].ToString());
        sb.Append(",");
        sb.Append(deaths[3].ToString());
        sb.Append(",");
        sb.Append(times[0].ToString());
        sb.Append(",");
        sb.Append(times[1].ToString());
        sb.Append(",");
        sb.Append(times[2].ToString());
        sb.Append(",");
        sb.Append(times[3].ToString());
        sb.Append(",");
        sb.Append(averages[0].ToString());
        sb.Append(",");
        sb.Append(averages[1].ToString());
        sb.Append(",");
        sb.Append(averages[2].ToString());
        sb.Append(",");
        sb.Append(averages[3].ToString());

        using (StreamWriter w = File.AppendText(filePath))
        {
            w.WriteLine(sb.ToString());
            w.Close();

        }
    }

    internal void WriteFloodLine(int daysPassed, string floodTime)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(daysPassed.ToString());
        sb.Append(",");
        sb.Append(floodTime);
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");
        sb.Append(",");
        sb.Append("-");

        using (StreamWriter w = File.AppendText(filePath))
        {
            w.WriteLine(sb.ToString());
            w.Close();
        }
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
        return Application.dataPath + "/CSV/";
    }
}
