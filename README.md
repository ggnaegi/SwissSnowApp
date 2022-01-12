# SwissSnowApp

Retrieve snow conditions (depth) using swiss zip code as query parameter

```
"PostPlzEndpoint": "https://swisspost.opendatasoft.com/api/records/1.0/search/?dataset=plz_verzeichnis_v2&q=&facet=postleitzahl&refine.postleitzahl=",
"MeteoSwissEndpoint": "https://data.geo.admin.ch/ch.meteoschweiz.messwerte-gesamtschnee-1d/ch.meteoschweiz.messwerte-gesamtschnee-1d_de.json"
```

This solution includes several azure functions with triggers.
- Timer Triggered function, retrieving snow conditions from meteoswiss ```SnowStatisticsRetriever```, saving data to azure service bus queue
- Service Queue Input triggered function, parsing snow conditions and saving them to cosmos db instance ```SnowStatisticsParser```
- Http GET Triggered function ```PlzFunction``` retrieving cities referenced by zip code from swiss post, caching data in redis
- Http POST Triggered function ```SnowStatisticsFunction``` retrieving snow conditions by cities names from cosmos db
- Http GET Triggered durable function, starting background task for snow conditions referenced by zip code retrieval ```PlzAndSnowStatisticsOrchestration```

![image](https://user-images.githubusercontent.com/58469901/149235990-7d455c59-c0db-40a2-a84e-26a822cd5a30.png)


Example with zip code 7260 (Davos Dorf)
```json
{
  "name": "PlzAndSnowStatisticsOrchestration",
  "instanceId": "a5cea1efb26d4154b33cf8ff0f4880eb",
  "runtimeStatus": "Completed",
  "input": {
    "Plz": "7260"
  },
  "customStatus": null,
  "output": [
  {
    "StationId": "DAV",
    "StationName": "Davos",
    "AltitudeInM": 1560,
    "SnowInCm": 56,
    "SnowMeasureDate": "2022-01-12T07:00:00+01:00",
    "PosX": 2783880.86,
    "PosY": 1187439.9
  }
  ],
  "createdTime": "2022-01-12T22:06:59Z",
  "lastUpdatedTime": "2022-01-12T22:07:10Z"
}
```

or 6060, Sarnen
```json
{
  "name": "PlzAndSnowStatisticsOrchestration",
  "instanceId": "15a469da16d54967a27c8fd80ac446e6",
  "runtimeStatus": "Completed",
  "input": {
    "Plz": "6060"
  },
  "customStatus": null,
  "output": [
  {
    "StationId": "SRN",
    "StationName": "Sarnen",
    "AltitudeInM": 471,
    "SnowInCm": 0,
    "SnowMeasureDate": "2022-01-12T07:00:00+01:00",
    "PosX": 2662147.8,
    "PosY": 1194359.9
  }
  ],
  "createdTime": "2022-01-12T22:13:57Z",
  "lastUpdatedTime": "2022-01-12T22:14:00Z"
}
```
