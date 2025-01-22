
# Praca inżynierska

## Autor

Justyna Kurcz

## Temat projektu

Projekt i implementacja aplikacji webowej wspomagającej opiekę nad zwierzętami domowymi

## Opis tematu (zakres pracy)

W ramach pracy inżynierskiej zaprojektowano i zaimplementowano aplikację webową, która umożliwia użytkownikom wspomaganie opieki nad swoimi zwierzętami domowymi. Aplikacja pozwala na dodawanie i edytowanie informacji o zwierzętach, takich jak dane osobowe, historia zdrowotna oraz kalendarze szczepień. Dodatkowo, użytkownicy otrzymują powiadomienia o nadchodzących terminach, takich jak wizyty kontrolne czy szczepienia. Projekt obejmował również implementację testów jednostkowych oraz integracyjnych, aby zapewnić wysoką jakość i niezawodność oprogramowania.

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

   Backend uruchomi się domyślnie pod adresem `http://localhost:5062`.

### Frontend

1. Przejdź do katalogu:

    ```bash
    cd PetManagerClient/src
    ```

2. Zainstaluj wymagane zależności:

    ```bash
    npm install
    ```

3. Następnie uruchom aplikację:

    ```bash
    ng serve
    ```

4. W przeglądarce wpisz adres:

    ```bash
    http://localhost:4200
    ```

   Frontend uruchomi się domyślnie pod tym adresem.

## Testy

### Uruchamianie testów jednostkowych

Testy jednostkowe sprawdzają poprawność poszczególnych jednostek kodu, takich jak metody czy klasy.

#### Krok 1: Wejdź do katalogu z testami jednostkowymi

W terminalu przejdź do katalogu, w którym znajdują się testy jednostkowe:

```bash
cd PetManager/tests/PetManager.Tests.Unit
```

#### Krok 2: Uruchom testy jednostkowe

Aby uruchomić testy jednostkowe, użyj polecenia:

```bash
dotnet test
```

To polecenie uruchomi testy, a w terminalu zobaczysz wyniki testów. Jeśli testy zakończą się sukcesem, będziesz miał informację o ich poprawnym przebiegu.

### Uruchamianie testów integracyjnych

Testy integracyjne sprawdzają, jak poszczególne części systemu współpracują ze sobą, np. komunikację między frontendem a backendem.

#### Krok 1: Wejdź do katalogu z testami integracyjnymi

W terminalu przejdź do katalogu, w którym znajdują się testy integracyjne:

```bash
cd PetManager/tests/PetManager.Tests.Integration
```

#### Krok 2: Uruchom testy integracyjne

Aby uruchomić testy integracyjne, użyj tego samego polecenia, co dla testów jednostkowych:

```bash
dotnet test
```

Podobnie jak w przypadku testów jednostkowych, wynik testów zostanie wyświetlony w terminalu.

## Dokumentacja API

Po uruchomieniu backendu dokumentacja API dostępna jest pod adresem:

- [http://localhost:5062/swagger/index.html](http://localhost:5062/swagger/index.html)

## Technologie użyte w projekcie

- **Backend**: C# .NET
- **Frontend**: Angular
- **Baza danych**: PostgreSQL
- **Przechowywanie zdjęć**: Azure Blob Storage
- **Dokumentacja API**: Swagger
