using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherHandler : MonoBehaviour
{
    [SerializeField] private float latitude;
    [SerializeField] private float longitude;
    [SerializeField] private string url;
    [SerializeField] private string key;

    [SerializeField] private Light directionLight;
    [SerializeField] private Color color;

    [SerializeField] private WeatherData weatherData;

    private string jsonRaw;

    [SerializeField] private UnitType units;
    private void Awake()
    {
        url = $"https://api.openweathermap.org/data/4.0/onecall/current?lat={latitude}&lon={longitude}&appid={key}&units={units}";
        StartCoroutine(RetrieveWeather());
    }

    private IEnumerator RetrieveWeather()
    {
        UnityWebRequest request = new UnityWebRequest(url);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            StopCoroutine(RetrieveWeather());
            Debug.Log(request.error);
        }
        else
        {
            jsonRaw = request.downloadHandler.text;
            DecodeJSON();
        }
    }
    private void DecodeJSON()
    {
        JSONNode json = JSON.Parse(jsonRaw);
        string timezoneString = json["timezone"];
        weatherData.city = timezoneString.Split('/')[0];
        weatherData.continent = timezoneString.Split('/')[0];
        weatherData.actualTemp = json["data"][0]["temp"];
        weatherData.description = json["data"][0]["weather"][0]["description"];
        weatherData.windSpeed = json["data"][0]["wind_speed"];

        ChangeEnviroment();
    }
    public void ChangeEnviroment()
    {
        switch (weatherData.actualTemp)
        {
            case float temp when weatherData.actualTemp < 15:
                {
                    color = new Color(0.3361072f, 0.7830189f, 1f);
                    break;
                }
            case float temp when weatherData.actualTemp < 15 && weatherData.actualTemp < 30:
                {
                    color = new Color(0.8018868f, 0.589559f, 1f);
                    break;
                }
            case float temp when weatherData.actualTemp >= 30:
                {
                    color = new Color(0.8f, 0.3047248f, 1f);
                    break;
                }
        }
        directionLight.color = color;
    }

}


[Serializable]
public struct WeatherData
{
    public string continent;
    public string city;
    public float actualTemp;
    public string description;
    public string windSpeed;
}
public enum UnitType
{
    standard,

    metric,

    imperial,

}
