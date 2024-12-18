# Testowanie i Jakość Oprogramowania / Projekt
## Autor
Justyna Kurcz, 35213

## Scenariusze testowe dla testera manualnego

| Test Case ID                    | Tytuł | Warunki wstępne | Kroki testowe | Oczekiwany rezultat |
|--------------------------------|--------|-------------------|---------------|------------------|
| TC-001 | Rejestracja nowego użytkownika z poprawnymi danymi | Otwarta aplikacja | 1. Wciśnij przycisk 'Utwórz konto'<br>2. Wypełnij w formularzu wszystkie pola<br>3. Wciśnij przycisk "Załóż konto" | Nastąpi przekierowanie na stronę logowania |
| TC-002 | Rejestracja nowego użytkownika z użytym już mailem | Aplikacja otwarta | 1. Wciśnij przycisk 'Załóż konto'<br>2. Wypełnij w formularzu wszystkie pola<br>3. W polu email podaj wcześniej użyty email<br>4. Kliknij przycisk "Załóż konto" | Serwer zwróci błąd co uniemożliwi utworzenie konta |
| TC-003 | Rejestracja nowego użytkownika z niepoprawnym formatem email | Aplikacja otwarta | 1. Wciśnij przycisk 'Zarejestruj się'<br>2. Wypełnij formularz<br>3. W polu email wpisz adres bez znaku @ (np. "testmail.com")<br>4. Kliknij przycisk "Załóż konto" | Pod polem email wyświetla się informacja o niepoprawnym formacie adresu email |
| TC-004 | Rejestracja nowego użytkownika z błędnie wypełnionym polem hasło | Aplikacja otwarta | 1. Wciśnij przycisk 'Zarejestruj się'<br>2. Wypełnij w formularzu wszystkie pola<br>3. W polu hasło wpisz hasło krótsze niż 6 znaków | Pod polem wyświetla się informacja, że podane hasło nie spełnia wymagań systemu |
| TC-005 | Logowanie użytkownika z poprawnymi danymi | Aplikacja otwarta, użytkownik zarejestrowany | 1. Przejdź na stronę logowania<br>2. Wpisz email i hasło podane przy rejestracji<br>3. Kliknij przycisk "Zaloguj się" | Nastąpi przekierowanie na stronę ze zwierzakami |
| TC-006 | Wylogowanie z systemu | Użytkownik zalogowany w aplikacji | 1. Wciśnij przycisk "Wyloguj się" | Nastąpi przekierowanie na stronę główną |
| TC-007 | Próba dostępu do chronionych zasobów bez logowania | Użytkownik niezalogowany | 1. Wklej URL (/profile) do podstrony ze szczegółami konta w pasku adresu | Nastąpi przekierowanie na stronę główną aplikacji |
| TC-008 | Przeglądanie wcześniej dodanych zwierząt | Użytkownik posiada dodane zwierzęta | 1. Zaloguj się za pomocą email i hasła | Na stronie pokażą się wszystkie zwierzęta dodane przez użytkownika |
| TC-009 | Dodanie nowego zwierzaka | Użytkownik zalogowany | 1. Wciśnij przycisk "Dodaj zwierzę" (niebieski przycisk w prawym dolnym rogu)<br>2. Wypełnij wyświetlony formularz danymi<br>3. Naciśnij przycisk "Zapisz" | Na stronie głównej pojawia się nowo dodane zwierzę |
| TC-010 | Wyświetlenie szczegółów zwierzaka | Użytkownik zalogowany, posiada dodane zwierzęta | 1. Na stronie głównej znajdź kartę zwierzaka<br>2. Kliknij przycisk "Zobacz Więcej" na wybranej karcie | Następuje przekierowanie na stronę ze szczegółami wybranego zwierzaka |
  
