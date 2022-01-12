# SwissSnowApp

Retrieve snow conditions (depth) using swiss zip code as query parameter

```
"PostPlzEndpoint": "https://swisspost.opendatasoft.com/api/records/1.0/search/?dataset=plz_verzeichnis_v2&q=&facet=postleitzahl&refine.postleitzahl=",
"MeteoSwissEndpoint": "https://data.geo.admin.ch/ch.meteoschweiz.messwerte-gesamtschnee-1d/ch.meteoschweiz.messwerte-gesamtschnee-1d_de.json"
```

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
