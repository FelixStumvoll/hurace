# Hurace

## Datenbank

Die Datenbank von Hurace besteht aus folgenden 18 Tabellen. In nachfolgendem Diagramm ist zu sehen, welche Spalten jeweils definiert sind und wie die Tabellen zusammenhängen.
![Diagramm](images/hurace.png)

### Tabellen

#### Country

Stellt ein Land dar.

#### Discipline

Stellt eine Disziplin dar.

#### EventType

Stellt den Typ eines möglichen Events dar, welches während eines Rennens passieren kann. Solche Events werden in RaceData gespeichert.

#### Gender

Stellt das Geschlecht dar, dieses wird um das Geschlecht eines Rennens und das eines Schirennläufers eindeutig zu definieren.

#### Location

Stellt einen Renn-Ort dar und besitzt zudem eine Referenz auf ein [Country](#country) in dem sich der Ort befindet.

#### PossibleDiscipline

Dies ist eine Assoziativtabelle zwischen [Discipline](#discipline) und [Location](#location). Hier wird dargestellt, welche Disziplinen an welchen Orten möglich sind.

#### Race

Stellt ein Rennen dar. Zusätzlich zu den informellen Spalten wird zudem eine Referenz auf ein [RaceState](#racestate) gespeichert, welcher den aktuellen Zustand des Rennens angibt.

#### RaceData

Diese Tabelle stellt ein Event-Log für ein Rennen dar. Hier werden alle Ereignisse eines Rennens gespeichert. Mittels eines [EventTypes](#eventtype) wird bestimmt um welches Event es sich handelt.

#### RaceEvent

Dies ist eine spezifizierte Ausführung von [RaceData](#racedata), welche ein Rennevent (z.B. Rennstart, Abbruch) darstellt.

#### RaceState

Stellt den Zustand eines Rennens dar. Dieser könnte z.B. Gestartet, Abgeschlossen oder Abgebrochen sein.

#### Season

Stellt eine Schisaison dar.

#### Sensor

Stellt einen Sensor dar, dieser hat eine Referenz auf ein [Race](#race). Dies folgt daraus, das sich die Anzahl der Sensoren von Rennen zu Rennen ändern können auch wenn diese am selben [Location](#location) stattfinden.

#### Skier

Stellt einen Schirennläufer dar.

#### SkierDiscipline

Stellt dar, welche Disziplinen ein Schirennläufer fährt.

#### SkierEvent

Dies ist eine spezifizierte Ausführung von RaceData, welche ein Event eines Skirennläufers darstellt. Dabei kann es sich z.B. um eine Disqualifikation oder um eine Zwischenzeit handeln.

#### StartList

Hier wird die Startreihenfolge eines Rennens dargestellt. Diese Tabelle referenziert einen Skirennläufer sowie ein Rennen.
Um doppelte Einträge zu vermeiden, kann ein Schirennläufer nur 1x für ein Rennen eingetragen werden.
Zudem wird der aktuelle Startzustand gespeichert, dieser kann z.B. Ausfall, Fertig oder im Starthaus wartend sein.

#### StartState

Stellt den Zustand eines StartListen-Eintrags dar. Dieser kann z.B. Fertig, Wartend oder Ausgefallen sein.

#### TimeData

Stellt eine Zwischenzeit dar. Es wird eine Referenz auf ein SkierEvent gespeichert um speichern zu können, wann eine Zwischenzeit passiert ist.
Zudem wird der Sensor referenziert, welcher die Zwischenzeit aufgenommen hat.
Dabei kann ein Skirennläufer nur eine Zwischenzeit pro Rennen für einen Sensor besitzen.
Weiters wird nicht ein Rennläufer direkt sondern eine StartList referenziert um sicherzugehen, dass der Schiläufer antritt bei dem jeweiligen Rennen.

## Database Access Layer

Der Database Access Layer (DAL) stellt eine Schnittstelle zur Verfügung, die Daten in der Datenbank zu manipulieren, ohne direkt Queries absetzen zu müssen.
Die DAL teilt sich dabei in zwei Bereiche auf:

- Interfaces, welche die Methoden definieren, mit welchen die Daten manipuliert werden können
- Database Access Objects, welche eine konkrete Implementierung der Interfaces für eine konkrete Datenbank zur Verfügung stellen.

Jedes DAO repräsentiert dabei eine Datenbanktabelle.

### Interfaces

Um verschiedene Datenbanksysteme realisieren zu können, werden die Methoden in Interfaces definiert, welche von den konkreten DAOs implementiert werden.
Da einige Methoden gleich aussehen und in allen DAOs verfügbar sein sollten, gibt es die vier Basis Interfaces von denen geerbt werden kann. Zudem gibt es einige Interface, welche zusätzliche Methoden definieren. Jene Interfaces, welche nur von den Basis Interfaces erben sind hier nicht näher beschrieben. Die gesamte Vererbungshierarchie sieht wie folgt aus:

![Interfaces](images/Dal.Interface.Diagram.png)

#### IReadonlyDao

Dieses Interface definiert eine _FindAllAsync_ Methode welche in allen DAOs verfügbar sein soll.

#### IDefaultReadonlyDao

Dieses Interface erbt von [IReadonlyDao](#ireadonlydao) und definiert eine _FindByIdAsync_ Methode mit welcher ein Eintrag anhand der Id retourniert.

#### ICrudDao

Dieses Interface erbt von [IReadonlyDao](#ireadonlydao) und definiert die Basis Crud Methoden:

- UpdateAsync
- InsertAsync
- DeleteAllAsync

Falls ein DAO eine Entität verwaltet die z.B. einen zusammengesetzten Primärschlüssel besitzt, kann von diesem Interface geerbt werden.

#### IDefaultCrudDao

Dieses Interface erbt sowohl von [IReadonlyDao](#ireadonlydao) als auch von [ICrudDao](#icruddao) und definiert CRUD Methoden für Entitäten mit einer einzelnen Id als Primärschlüssel:

- InsertGetIdAsync
- DeleteAsync

#### ITimeDataDao

Dieses Interface erbt von [ICrudDao](#icruddao). Dadurch das [TimeData](#timedata) keinen eindeutigen Primärschlüssel besitzt sondern sich dieser aus der Id des [Skiers](#skier) und der Id des [Races](#race) zusammensetzt müssen _DeleteAsync_ und _FindByIdAsync_ eigen definiert werden. Zudem definiert dieses Interface eine Methode _GetRankingForRace_ diese liefert den aktuellen Stand des Rennens.

#### IStartListDao

Dieses Interface erbt von [ICrudDao](#icruddao). Gleich wie bei [ITimeDataDao](#itimedatadao) müssen _DeleteAsync_ und _FindByIdAsync_ eigen definiert werden. Auch hier besteht der Primärschlüssel aus der Id des [Skiers](#skier) und der Id des [Races](#race). Dieses Interface definiert zudem eine Methode _GetCurrentSkierForRace_ zum Ermitteln des aktuellen Rennläufers, sowie eine Methode _GetNextSkierForRace_ zum Ermitteln des nächsten Rennläufers. Weiters gibt es eine Methode _GetStartListForRace_ welche die Startliste eines Rennens ermittelt.

#### ILocationDao

Dieses Interface erbt von [IDefaultCrudDao](#idefaultcruddao). In diesem Interface wird auch die Tabelle [PossibleDiscipline](#possiblediscipline) mit verwaltet. Deshalb sind diese Methoden definiert:

- DeletePossibleDisciplineForLocation
- GetPossibleDisciplineForLocation
- InsertPossibleDisciplineForLocation

#### ISkierDao

Dieses Interface erbt von [IDefaultCrudDao](#idefaultcruddao). In diesem Interface wird auch die Tabelle [SkierDiscipline](#skierdiscipline) mit verwaltet. Deshalb sind diese Methoden definiert:

- DeletePossibleDisciplineForSkier
- GetPossibleDisciplinesForSkier
- InsertPossibleDisciplineForSkier

### Domain Objects / Data Transfer Objects

Domain Objects bzw. Data Transfer Objects (DTOs) dienen dazu die Tabellen als Klassen abzubilden.
Bis auf die Tabellen [PossibleDiscipline](#possiblediscipline) und [SkierDiscipline](#skierdiscipline) gibt es für jede Tabelle ein eigenes DTO. Die Spaltennamen werden als Properties modelliert.
In den DTOs kommen zwei Attribute zum Einsatz um das Mappen zu erleichtern.
Für Primärschlüssel wird das _KeyAttribute_ von _System.ComponentModel.DataAnnotations_ verwendet.
Weiters sind in jedem DTO die referenzierten Tabellen als Properties verfügbar (z.B. [Country](#country) bei [Skier](#skier)). Diese werden mit dem _NavigationalAttribut_ gekennzeichnet.

### Database Access Objects

Die Database Access Objects (DAOs) sind eine konkrete Implementierung der [Interfaces](#interfaces).
Konkret stellen diese einen Zugriff auf eine MSSQL Datenbank zur Verfügung.
Gleich wie bei den [Interfaces](#interfaces) gibt es auch bei Basisklassen mit den gleichen Namen, die die jeweiligen Interfaces implementieren. Weiters gibt es eine _BaseDao_ Klasse welche über Methoden zum Ansprechen der Datenbank verfügt. Diese Methoden verwenden ADO.NET um Die Daten aus der Datenbank zu laden. Wie die Daten in Domänenklassen gemappt werden, wird in [Mapper](#mapper) beschrieben.
Im folgenden Diagram ist die Vererbungshierarchie der DAOs zu sehen.

![DAOs](images/Dal.Dao.Diagram.png)

Sämtliche Funktionalität ist dabei in _BaseDao_ untergebracht. Zudem besitzt diese Klasse eine _ConnectionFactory_ mit welcher eine Datenbankverbindung aufgebaut werden und anschließend SQL-Statements ausgeführt werden können.
Weiters steht eine [StatementFactory](#statementfactory) zur Verfügung, welche SQL Statements erzeugen kann.
Die wichtigsten Methoden sind im Anschluss beschrieben.

#### QueryAsync

Diese Methode führt eine Query aus und liefert eine List an generischen Ergebnissen.
Dabei wird ein Statement und optional Query Parameter übernommen. Zudem kann noch eine Konfiguration für den [Mapper](#mapper) übergeben werden. Zuerst wird mittels der _ConnectionFactory_ ein _DbCommand_ erzeugt. Anschließend wird das Statement ausgeführt und die Ergebnisse mittels des [Mappers](#mapper) gemappt und retourniert.

#### ExecuteAsync

Diese Methode führt ein Statement auf der Datenbank aus, dabei kann es sich z.B. um ein Update, Insert oder Delete Statement handeln. Diese Methode übernimmt ein Statement sowie die Query Parameter dafür. Anschließend wird wie bei [QueryAsync](#queryasync) ein _DbCommand_ erzeugt, mit welchem das Statement ausgeführt wird.

#### ExecuteGetIdAsync

Diese Methode funktioniert ähnlich wie [ExecuteAsync](#executeasync). Der Unterschied ist der, dass bei dieser Methode die letzte generierte Id returniert wird.

#### Mapper

Um nicht manuell einen Mapper für jedes DTO schreiben zu müssen, wird diese Funktionalität in einen Mapper ausgelagert. Dieser hat eine generische Methode _MapTo_. Der Mapper durchläuft alle Properties des angegebenen Typen und holt sich mittels des Property Names einen Wert aus dem ebenfalls übergebenen _IDataRecord_ welcher die Datenbankeinträge enthält. Ist ein Property mit dem _NavigationAttribute_ gekennzeichnet, so wird _MapTo_ rekursiv aufgerufen und die Properties dieses Typen gemappt.
Mittels einer _MapperConfig_ kann konfiguriert werden, dass z.B ein Property einen Wert erhält welcher unter einem anderen Namen aus der Datenbank geholt wird. Weiters wird mit der _MapperConfig_ angegeben, welche referenzierten Entitäten geladen werden sollen.

#### StatementFactory

Um simple Select Statements nicht jedes mal schreiben zu müssen, wird eine _StatementFactory_ zur Verfügung gestellt. Diese ermöglicht es Select, Insert und Update Queries anhand eines generischen Typen zu generieren. Einer der drei Builder kann mittels der jeweiligen Methoden erzeugt werden, der generische Typ gibt dabei an, auf welche Tabelle das Statement ausgeführt wird.
Die Vererbungshierarchie sieht wie folgt aus:

![StatementFactory](images/Common.Diagram.png)

Die drei StatementFactories bieten folgende Funktionen

##### SelectStatementBuilder

Der SelectQueryBuilder ermöglicht es ein Select Statement für eine Tabelle zu erzeugen. Zudem kann mittels _Join_ eine andere Tabelle gejoined werden. Dabei müssen zwei generische Typen angegeben werden, diese stellen die Tabellen dar zwischen welchen gejoined werden soll. Zudem können Join Conditions mitgegeben werden.
Mit der *Where* Methode können zusätzlich Where Conditions hinzugefügt werden.
Mittels der *Build* Methode kann ein Statement erzeugt werden.

##### UpdateQueryBuilder
Der UpdateQueryBuilder ermöglicht es Update Statements zu erzeugen.
Mittels *Where* kann das Update Statement eingeschränkt werden. Dies Funktioniert gleich wie bei dem [SelectStatementBuilder](#selectstatementbuilder).
Weiters gibt es eine Methode *WhereId* welche automatisch die Primärschlüssel in die Where Condition einfügt. Dies funktioniert mittels des *KeyAttributes*.

##### InsertQueryBuilder
