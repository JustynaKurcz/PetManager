# Testowanie i Jakość Oprogramowania

## Autor

Justyna Kurcz

## Temat projektu

Testowanie aplikacji wspomagającej opiekę nad zwierzętami domowymi

## Opis projektu

Implementacja aplikacji webowej, która pozwala użytkownikom na dodawanie i zarządzanie informacjami o swoich zwierzakach.

Aplikacja umożliwia:

- Dodawanie zwierząt oraz edytowanie informacji o nich,
- Śledzenie historii zdrowotnej zwierzęcia,
- Generowanie powiadomień dotyczących ważnych terminów, takich jak wizyty czy szczepienia.

Projekt obejmuje również implementację testów jednostkowych i integracyjnych.

## Uruchomienie projektu

### Backend

1. Przejdź do katalogu:

    ```bash
    cd PetManager/src/PetManager.api
    ```

2. Wykonaj polecenie:

    ```bash
    dotnet run
    ```

### Frontend

1. Przejdź do katalogu:

    ```bash
    cd PetManagerClient/src
    ```

2. Wykonaj polecenie:

    ```bash
    ng serve
    ```

3. W przeglądarce wpisz:

    ```bash
    localhost:4200
    ```

Frontend uruchomi się domyślnie pod tym adresem.

## Testy

### Testy jednostkowe

Testy jednostkowe znajdują się w lokalizacji:

- **PetManager/tests/PetManager.Tests.Unit**

Podzielone na foldery w zależności od kategorii:

- **Users**
- **Pets**
- **HealthRecords**

### Testy integracyjne

Testy integracyjne znajdują się w lokalizacji:

- **PetManager/tests/PetManager.Tests.Integration**

Podzielone na foldery w zależności od kategorii:

- **Users**
- **Pets**
- **HealthRecords**

## Dokumentacja API

Po uruchomieniu projektu (backendu), dokumentacja API dostępna jest pod adresem:

- [http://localhost:5062/swagger/index.html](http://localhost:5062/swagger/index.html)

## Scenariusze testowe dla testera manualnego

| Test Case ID | Tytuł                                | Warunki wstępne                              | Kroki testowe                                                                                   | Oczekiwany rezultat                                          |
|--------------|--------------------------------------|----------------------------------------------|-------------------------------------------------------------------------------------------------|--------------------------------------------------------------|
| TC-001       | Rejestracja nowego użytkownika       | Otwarta aplikacja                            | 1. Wciśnij przycisk 'Zarejestruj się'<br>2. Wypełnij wszystkie pola<br>3. Wciśnij przycisk "Załóż konto" | Nastąpi przekierowanie na stronę logowania                  |
| TC-002       | Rejestracja z użytym już mailem      | Aplikacja otwarta                            | 1. Wciśnij przycisk 'Zarejestruj się'<br>2. Wprowadź wcześniej użyty email<br>3. Kliknij "Załóż konto" | Serwer zwróci błąd i uniemożliwi utworzenie konta            |
| TC-003       |Resestowanie hasła            | Stroan resetowania hasła otwarta                           | 1. Wprowadź adres e-mail. 2. Kliknij "Wyślij link".          | Link do resetowania hasła zostaje wysłany na e-mail  |
| TC-004       | Hasło niespełniające wymagań        | Aplikacja otwarta                            | 1. Wciśnij 'Zarejestruj się'<br>2. Podaj hasło krótsze niż 6 znaków<br>3. Kliknij "Załóż konto" | Wyświetla się informacja o niespełnieniu wymagań hasła       |
| TC-005       | Uzupełnianie danych opcjonalnych               | Użytkownik jest zalogowany, otwarta strona moje konto              | 1. Kliknij "Edytuj". 2. Dodaj brakujące dane opcjonalne. 3. Zapisz zmiany.                                          |Dane zostaną uzupełnione             |
| TC-006       | Wylogowanie z systemu               | Użytkownik zalogowany                        | 1. Wciśnij przycisk "Wyloguj się"                                                               | Nastąpi przekierowanie na stronę główną                     |
| TC-007       | Dostęp do chronionych zasobów       | Użytkownik niezalogowany                     | 1. Wklej URL do chronionych zasobów w pasku adresu                                              | Nastąpi przekierowanie na stronę główną aplikacji           |
| TC-008       | Przeglądanie dodanych zwierząt      | Użytkownik posiada dodane zwierzęta          | 1. Zaloguj się                                                                                  | Wyświetla się lista zwierząt dodanych przez użytkownika      |
| TC-009       | Dodanie nowego zwierzaka            | Użytkownik zalogowany                        | 1. Kliknij "Dodaj zwierzę"<br>2. Wypełnij formularz<br>3. Kliknij "Zapisz"                      | Na liście pojawia się nowy zwierzak                         |
| TC-010       | Wyświetlenie szczegółów zwierzaka   | Użytkownik posiada dodane zwierzęta          | 1. Kliknij "Zobacz Więcej" na karcie zwierzaka                                                 | Wyświetlane są szczegóły wybranego zwierzaka                |

## Technologie użyte w projekcie

- **Backend**: C# .NET
- **Frontend**: Angular
- **Baza danych**: PostgreSQL
- **Przechowywanie zdjęć**: Azure Blob Storage
- **Dokumentacja API**: Swagger
