
DELETE FROM "Przypomnienia";
DELETE FROM "Zabiegi";
DELETE FROM "Rosliny";
DELETE FROM "Gatunki";

INSERT INTO "Gatunki" ("GatunekId", "NazwaGatunku", "WymagaNawadnianiaCoIleDni", "WymagaNawozeniaCoIleDni", "Swiatlo", "TemperaturaMin", "TemperaturaMax")
VALUES
    (1, 'Fikus', 7, 60, 'Jasne rozproszone', 15, 30),
    (2, 'Sukulenty', 14, 90, 'Pełne słońce', 10, 35),
    (3, 'Paproć', 3, 45, 'Cieniste', 12, 28),
    (4, 'Dracena', 10, 60, 'Półcień', 15, 28),
    (5, 'Aloes', 12, 120, 'Słoneczne', 10, 35),
    (6, 'Monstera', 7, 50, 'Rozproszone światło', 16, 30),
    (7, 'Zamiokulkas', 14, 60, 'Półcień', 15, 30),
    (8, 'Sansewieria', 20, 180, 'Półcień', 12, 35),
    (9, 'Storczyk', 5, 30, 'Jasne rozproszone', 18, 28),
    (10, 'Bluszcz', 3, 40, 'Cieniste', 10, 25);


INSERT INTO "Rosliny" ("RoslinaId", "Nazwa", "DataZakupu", "Miejsce", "GatunekId", "ZdjeciePath")
VALUES
    (1, 'Benek', '2023-05-01', 'Salon', 1, NULL),
    (2, 'Kaktusik', '2024-01-15', 'Parapet', 2, NULL),
    (3, 'Zielonka', '2022-11-20', 'Łazienka', 3, NULL),
    (4, 'Draka', '2023-03-12', 'Kuchnia', 4, NULL),
    (5, 'Alek', '2024-02-10', 'Biuro', 5, NULL),
    (6, 'Moni', '2023-08-05', 'Sypialnia', 6, NULL),
    (7, 'Zamcio', '2023-09-25', 'Korytarz', 7, NULL),
    (8, 'Wąż', '2022-12-10', 'Garaż', 8, NULL),
    (9, 'Storcio', '2023-06-17', 'Salon', 9, NULL),
    (10, 'Iwia', '2023-07-22', 'Łazienka', 10, NULL);


INSERT INTO "Zabiegi" ("ZabiegId", "RoslinaId", "TypZabiegu", "DataWykonania", "Opis")
VALUES
    (1, 1, 'Podlanie', '2024-05-10', 'Podlano umiarkowanie'),
    (2, 2, 'Nawożenie', '2024-04-01', 'Użyto nawozu do kaktusów'),
    (3, 3, 'Przesadzanie', '2024-03-15', 'Nowa doniczka i ziemia'),
    (4, 4, 'Podlanie', '2024-05-08', 'Podlano lekko'),
    (5, 5, 'Nawożenie', '2024-05-01', 'Specjalny nawóz'),
    (6, 6, 'Podlanie', '2024-05-12', 'Ziemia sucha'),
    (7, 7, 'Mycie liści', '2024-04-25', 'Liście wilgotną ściereczką'),
    (8, 8, 'Podlanie', '2024-05-15', 'Minimalne podlewanie'),
    (9, 9, 'Nawożenie', '2024-05-02', 'Nawóz do storczyków'),
    (10, 10, 'Podlanie', '2024-05-14', 'Dokładnie podlano');

INSERT INTO "Przypomnienia" ("PrzypomnienieId", "RoslinaId", "TypZabiegu", "DataPlanowana", "CzyWykonane")
VALUES
    (1, 1, 'Podlanie', '2024-05-20', FALSE),
    (2, 1, 'Nawożenie', '2024-06-01', FALSE),
    (3, 2, 'Podlanie', '2024-05-22', FALSE),
    (4, 3, 'Przesadzanie', '2024-06-10', FALSE),
    (5, 4, 'Podlanie', '2024-05-21', FALSE),
    (6, 5, 'Nawożenie', '2024-06-15', FALSE),
    (7, 6, 'Podlanie', '2024-05-23', FALSE),
    (8, 7, 'Mycie liści', '2024-06-05', FALSE),
    (9, 8, 'Podlanie', '2024-05-25', FALSE),
    (10, 9, 'Nawożenie', '2024-06-01', FALSE);
