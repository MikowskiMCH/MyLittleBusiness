Powód podjęcia się stworzenia aplikacji "MyLittleBusiness":
-Aplikacja tworzona została w celu ułatwienia procesu fakturowania mojemu ojcu, który
prowadzi działalność przetwórni rybnej.

-Osoba prowadzącą działalność wypisywała faktury odręcznie(Wymagało to nabywania w sklepach
papierniczych gotowych szablonów dla faktur i odręcznego wypełniania ich).

-Celem aplikacji jest:
--Dodawanie, usuwanie i przechowywanie danych o sprzedawanych przez firmę towarów.
--Przechowywanie, dodawanie i usuwanie danych o klientach firmy.
--Tworzenie faktur, dodawanie i usuwanie zawartych w fakturze towarów.
--Eksportowanie faktury do pliku .pdf umożliwiającego eksport i wydruk faktury. 

Uwagi odnośnie projektu:
-Jest to mój pierwszy projektów aplikacji webowej ASP.NET (.NET 6).
-Większość planowanej funkcjonalności została zaimplementowana(W planach była jeszcze
przebudowa systemu uwierzytelniania, modyfikacja wyglądu aplikacji i poprawa wyświetlanych
komunikatów walidacyjnych).
-Aplikacja nie została dokończona (Docelowy konsument pod wpływem przedstawienia innego
gotowego rozwiązania problemu zdecydował się na jego wybór).

Opis słowny funkcjonalność aplikacji:

-System logowania/rejestracji:
Aplikacja wymaga zalogowania użytkownika / rejestracji użytkowania.
Ta część nie została do końca przemyślana. Konta mogą posiadać różne dane osobowe.
Mimo różnych firm(wprowadzonych danych firmy przy rejestracji), działając na 
tej samej bazie danych jesteśmy ograniczeni do działania na danych znajdujących się w niej.

-Po logowaniu aplikacja przenosi użytkownika do widoku głównego z wykresem przedstawiającym
całkowity utarg(brutto) uzyskany od klientów(można porównać klientów na podstawie utargów).

-Widok główny zawiera przyciski: "MyLittleBusiness" ,"Klienci", "Towary" po lewej stronie 
paska nawigacyjnego i opcje "Wyloguj" po prawej jego stronie. 

-Po kliknięciu "MyLittleBusiness" aplikacja przenosi użytkownika do widoku głównego
z wykresem.

-Po kliknięciu "Klienci" aplikacja przenosi użytkownika do widoku zarządzania klientami

-W widoku zarządzania klientami użytkownik ma następujące możliwości:
--Dodać / usunąć / edytować / wyświetlić szczegóły klienta
--Przejść do widoku zarządzania fakturami danego klienta poprzez wybranie przy nim opcji "Faktury"

-W widoku zarządzania fakturami klienta użytkownik ma możliwości:
--Przeglądać listę faktur użytkownika
--Dodać / Usunąć / Wyświetlić szczegóły faktury
--Wyświetlić fakturę w postaci pliku .pdf przeznaczonym do wydruku
--Przejście do widoku zarządzania zawartością faktury

-Widok zarządzania zawartością faktury zawiera następujące funkcjonalności:
--Dodaj towar do faktury (Musi być zawarty w tabeli towarów)
--Usuń towar z faktury
--Wyświetl szczegóły dodanej iteracji towaru

-Po kliknięciu przycisku "Towary" aplikacja przenosi użytkownika do widoku towarów
(Zarządzanie tabelą towary), gdzie można znaleźć następujące funkcjonalności:
--Dodaj nowy towar
--Usuń towar
--Szczegóły (Wyświetl szczegóły towaru)
--Edytuj towar

-Po kliknięciu przycisku "Wyloguj" następuje wylogowanie i przeniesienie użytkownika do widoku
logowania / rejestracji
