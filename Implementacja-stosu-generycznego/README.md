# Rozbudowa implementacji stosu generycznego

----------

* Krzysztof Molenda, 2018-07-30
* *Cel dydaktyczny*: analiza obcego kodu, tworzenie prostych testów jednostkowych, implementacja indeksera, implementacja interfejsu `IEnumerable`.

----------

W załączonym kodzie zrealizowano implementację struktury danych _stos_ w tablicy.

W folderze `src/` znajdują się pliki:

* `IStos.cs` - interfejs stosu
* `StosEmptyException.cs` - definiujący klasę wyjątku zgłaszanego, gdy stos jest pusty
* `StosWTablicy.cs` - generyczna implementacja stosu, wykorzystująca jako _nośnik danych_ tablicę

W folderze `unitTest/` znajduje się plik `UnitTest1.cs`, weryfikujący poprawność implementacji stosu - w oparciu o aksjomaty oraz definicje zawarte w `IStos<T>` - na przykładzie stosu znaków (`StosWTablicy<char>`).

Implementacja ta, aczkolwiek kompletna w sensie teoretycznym, pozbawiona jest funkcjonalności przeglądania zawartości stosu (nie działa pętla `foreach`, nie mamy bezpośredniego dostępu do tablicy przechowującej elementy stosu). Dostęp do jego elementów możliwy jest dopiero po jego wyeksportowaniu do tablicy `T[]`.

## Polecenia

1. Zapoznaj się z kodem, przetestuj, wykonaj refaktoryzację według własnych zasad. Musisz zrozumieć bieżącą implementację.

2. Bieżąca implementacja przewiduje automatyczne rozszerzanie się stosu (2-krotne powiększanie się tablicy) w sytuacji wyczerpania wolnego miejsca do składowania elementów (znajdź stosowny fragment kodu). Zaimplementuj metodę `void TrimExcess()` modyfikującą stos tak, aby zajętych było ok. 90% komórek (ok. 10% było wolnych - gotowych do przyjęcia nowych elementów). Spróbuj napisać testy jednostkowe weryfikujące poprawność zaimplementowanej metody.

3. Zaimplementuj indekser umożliwiający przeglądanie elementów stosu w sposób podobny do przeglądania tablic, z użyciem `[ ]`. Dodaj do interfejsu deklarację:

    ````csharp
    //indekser - przeciążenie operatora []
    T this[int index] { get; } //read-only
    ````

    Dodaj odpowiednie _unit testy_ weryfikujące poprawność implementacji indeksera.

4. Zaimplementuj w klasie `StosWTablicy<T>` mechanizm umożliwiający przeglądanie w trybie _read only_ elementów stosu za pomocą pętli `foreach`. Realizuje się to poprzez implementację interfejsu `IEnumerable<T>`. Przyjmij, że elementy będą przeglądane w kolejności od pierwszego wstawionego na stos do ostatniego:

    1. zrealizuj to, tworząc własny iterator implementujący `IEnumerator<T>` "od zera" - w formie prywatnej klasy wewnętrznej,

    2. zrealizuj to, wykorzystując indekser oraz słowo kluczowe `yield`.

5. Utwórz inny sposób przeglądania stosu - od ostatniego wstawionego do pierwszego, dostarczając stosowną metodę zwracającą `IEnumerable<T>` (w implementacji wykorzystasz `yield`).

6. Dopisz do implementacji stosu metodę:

    ```csharp
    public System.Collections.ObjectModel.ReadOnlyCollection<T> ToArrayReadOnly()
    {
        return Array.AsReadOnly(tab);
    }
    ```

    Porównaj działanie tej metody z metodą `ToArray()` zaimplementowaną wcześniej, dla `T` - typu wartościowego oraz referencyjnego (obiekty _mutables_).

7. Wykonaj implementację interfejsu `IStos<T>` w klasie `StosWLiscie<T>` - wykorzystując jako nośnik danych, zamiast tablicy, struktury wiązane (lista węzłów). Twoja implementacja powinna również przechodzić testy jednostkowe z `UnitTest1.cs`. Nie wykorzystuj gotowych kolekcji, np. `LinkedList` (Twoja implementacja powinna być tzw. _pierwszoklasowa_).
