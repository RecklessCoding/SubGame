using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class ABOD3_Bridge
{
    private static ABOD3_Bridge instance;

    public static ABOD3_Bridge GetInstance()
    {

        if (instance == null)
        {
            instance = new ABOD3_Bridge();
        }

        return instance;
    }

    private StreamWriter streamWriter;

    private int currentBot = 0;

    private ABOD3_Bridge()
    {
        AttemptToConnect();
    }

    internal void AttemptToConnect()
    {
        try
        {
            TcpClient client = new TcpClient("localhost", 3000);

            Stream stream = client.GetStream();
            streamWriter = new StreamWriter(stream);
            streamWriter.AutoFlush = true;
        }
        catch
        {
        }
    }

    internal void ChangeSelectedBot(int newCurrentBot)
    {
        currentBot = newCurrentBot;
    }

    internal void AlertForCondition(string conditionName, int botNumber)
    {
        if (streamWriter != null && botNumber == currentBot)
        {
            streamWriter.WriteLine(conditionName);
        }
    }

    internal void AlertForGoal(string goalName, int botNumber)
    {
        if (streamWriter != null && botNumber == currentBot)
        {
            streamWriter.WriteLine(goalName);
        }
    }


    internal void AletForElement(int botNumber, string elementName, string elementType)
    {
        if (streamWriter != null && botNumber == currentBot)
        {
            streamWriter.WriteLine(botNumber.ToString() + "," + elementName + "," + elementType);
        }
    }
}