# Utils - C# Klassenbibliothek
Willkommen im Utils-Repository! Dieses Projekt ist eine Klassenbibliothek, die in C# geschrieben wurde und verschiedene eigens erstellte Libraries und Methoden-Erweiterungen enthält. Das Hauptziel dieser Bibliothek ist es, nützliche Funktionen und Hilfsmittel bereitzustellen, um die Entwicklung von C#-Anwendungen zu erleichtern.

## Features
* **CSV:** Eine eigens erstellte Library zum vereinfachten Lesen von CSV-Dateien.
* **ExtendedStopwatch:** Eine Erweiterung der Stopwatch-Klasse, die zusätzliche Funktionalitäten bietet.
* **GlobalHotkey:** Eine Library, die die Registrierung und Behandlung globaler Tastenkombinationen ermöglicht.
* **Json:** Eine Library zur Arbeit mit JSON-Daten unter Verwendung der Newtonsoft.Json-Bibliothek.
* **Twitch**: Eine Library zur Interaktion mit der Twitch-Plattform unter Verwendung der Twitch.Lib-Bibliothek.
* **Youtube**: Eine Library zur Interaktion mit der YouTube v3 API zur Verwaltung von YouTube-Inhalten.


## Verwendung

Um die Utils-Klassenbibliothek in Ihrem Visual Studio-Projekt zu verwenden, können Sie die folgenden Schritte befolgen:

1. **Klonen des Repositorys:** Klonen Sie dieses Repository auf Ihren lokalen Arbeitsplatz.

```
git clone https://github.com/WhiteWolfysGame/utils.git
```

2. **Integration in Visual Studio:** Öffnen Sie Ihr Visual Studio-Projekt und fügen Sie die Utils-Bibliothek zu Ihrem Projekt hinzu. Gehen Sie dazu wie folgt vor:
- Klicken Sie mit der rechten Maustaste auf Ihr Projekt im Projektmappen-Explorer und wählen Sie "Verweis hinzufügen" aus.
- Suchen Sie nach den gewünschten Libraries (CSV, ExtendedStopwatch, GlobalHotkey, Json, Twitch, Youtube) und fügen Sie sie Ihrem Projekt hinzu.
- Stellen Sie sicher, dass die erforderlichen Abhängigkeiten (wie Newtonsoft.Json und Twitch.Lib) ebenfalls hinzugefügt werden.

3. **Verwenden der Libraries:** Importieren Sie die entsprechenden Namespaces in Ihren Quellcode und nutzen Sie die Funktionen und Erweiterungen, die in den einzelnen Libraries bereitgestellt werden.

**Beispiele:**
1. **Verwendung der Library Csv**
``` csharp
using Utils.Lib.Csv;

namespace YourNamespace
{
    class YourClass
    {
        // some code...
        
        private void TestCsv()
        {
            Csv csv = new Csv("YourCsvFile.txt", ';');  // Initialisiert CSV und lädt alle Daten aus der Datei. CSV-Datei muss einen Header haben!
            
            int numCols = csv.NumColumns; // Anzahl der Spalten dieser Datei
            int numRows = csv.NumRows; // Anzahl der Zeilen
            
            CsvColumn cols = csv.Columns; // Gibt ein Array mit CsvColumns zurück
            CsvColumn colvaluesById = csv[2]; // Gibt Werte der Spalte 3 zurück
            CsvColumn colvaluesByKeyword = csv["lastname"]; // Gibt Werte einer bestimmten Spalte (Header) zurück
            
            CsvCell myRowById = csv[1].Rows[4]; // Liefert den Wert in der Spalte 3 und Zeile 4 zurück
            CsvCell myRowByKeyword = csv["lastname"].Rows[4]; // Liefert den Wert in Spalte "lastname" und Zeile 4 zurück
        }
        // some code...
    }
}
```



## Abschluss
Ich freue mich über Beiträge zur Utils-Klassenbibliothek. Wenn Sie einen Fehler gefunden haben oder eine Verbesserung vorschlagen möchten, können Sie gerne einen Pull Request erstellen oder ein Issue auf GitHub öffnen.

Vielen Dank für Ihr Interesse an Utils!
