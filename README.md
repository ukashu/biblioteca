## TODOs na zjazd 6
### Ola (branch zjazd6_ola)
- [x] zaimplementować zakładkę "Users" z listą użytkowników i z działającym w pamięci CRUDem
	- możesz wzorować się w pełni na implementacji istniejącej zakładki "Books"
	- CRUD działający tak samo jak w "Books" z podobnym UI
### Wiktoria (branch zjazd6_wiktoria)
- [x] zaimplementować zakładkę "Borrowing History" (BorrowingHistory.xaml)
	- dodać model Loan.cs i przykładową listę historii wypożyczeń w pamięci
	- zakładka ma wyświetlać listę wypożyczeń z pamięci
### Natalia (branch zjazd6_natalia)
- [x] zaimplementować zakładkę "Return Books" (ReturnBooks.xaml)
	- ma to być formularz z inputem tekstowym "ISBN" lub "Sygnatura" i przyciskiem "Dodaj", po kliknięciu przycisku "Dodaj" ISBN powinien pojawić się na liście w tym samym oknie
	- na dole okna przycisk "Zwróć", po kliknięciu ma wyświetlić MessageBox z tekstem "Zwrócono książki"
### Łukasz
- [x] dodać placeholdery do tasków
- [ ] połączyć funkcjonalność zwracania książek z modelami Books, Users i Loans

## TODOs na zjazd 8
### Backlog
- przenieść logikę z code behind do view modeli (w userlist i booklist)
- dodać walidację i error handling gdzie potrzebne
- dodać bazę danych
- dodać funkcjonalność wypożyczania książek (po kliknięciu na usera okno do wpisywania ISBN)
- dodać możliwość edycji wielu książek i userów na raz (kilka edytowanych książek/userów powinno otwierać się jako zakładki w jednym oknie details)

### Łukasz
- [x] dodać bazę danych SQLite
- [x] naprawianie commitów
### Ola
- [x] dodać funkcjonalność wypożyczania książek z wykorzystaniem bazy danych
	- dodać przycisk dla każdego Usera w zakładce Users, po naciśnięciu którego wyskoczy okno do wypożyczania książek przez tego użytkownika
	- okno wypożyczania zrobione podobnie jak zakładka Return Books, poprzez formularz dodajemy ISBN do listy i klikamy przycisk "Wypożycz" żeby wypożyczyć wiele książek na raz dla wybranego usera
	- po kliknięciu przycisku "Wypożycz" wyświetlić Dialog z komunikatem "Success" lub "Failure"
### Wiktoria
- [x] połączyć funkcjonalność zwracania książek z bazą danych
### Natalia
<s>- [ ] dodać w zakładce "Return Books" filtrowanie po nazwie wypożyczającego</s>

## TODOs na zjazd 9
### Backlog
- refaktoryzacja okien details/edit
- responsywność UI
- filtrowanie po wypożyczającym i ISBN w zakładce "Return Books"
- połączenie Scan Books z bazą danych
- sortowanie i filtrowanie w zakładce "Book List"
- sortowanie i filtrowanie w zakładce "User List"

## Łukasz
- [ ] refaktoryzacja okien details/edit
- [x] sortowanie i filtrowanie w zakładce "Book List"
- [x] sortowanie i filtrowanie w zakładce "User List"
### Ola
- [x] responsywność UI
	- zidentyfikować elementy, które nie skalują się dobrze i poprawić ich zachowanie przy zmianie rozmiaru okna
	- np. po rozwinięciu kolumny Details w User i Book List, trzeba ją ręcznie rozszerzyć, można zmienić tak, aby automatycznie się rozszerzała albo żeby zawsze wypełniała przestrzeń do prawej krawędzi okna; Można też zmienić layout tak, żeby szczegóły nie były jedną z kolumn a otwierały się zawsze po prawej stronie okna po jednokrotnym kliknięciu na książkę z listy
### Wiktoria
- [x] filtrowanie po wypożyczającym i ISBN w zakładce "Return Books"
	- dodać input tekstowy do wpisywania nazwy wypożyczającego i ISBN, po wpisaniu których będzie filtrować listę książek do zwrotu
### Natalia
- [ ] połączenie funkcjonalności Scan Books z bazą danych
	- zakładkaScan Books działa jak poprzednia implementacja Return Books
	- wpisanie ISBN powinno wyświetlić propozycje książek do zwrócenia, kliknięcie dodaj dodaje do listy do zwrócenia, a kliknięcie "Zwróć" zwraca książki z tej listy i wyświetla komunikat o sukcesie lub błędzie
	- kliknięcie Enter po wpisaniu ISBN powinno działać tak samo jak kliknięcie przycisku "Dodaj"

## TODOs na zjazd 10

### Łukasz
- [x] naprawić relację encji
- [x] wyświetlanie liczby wypożyczonych książek w informacjach o użytkowniku
- [x] wyświetlanie użytkownika który wypożyczył książkę w informacjach o książce
- [ ] podświetlać na czerwono użytkowników którzy mają książki wypożyczone >30 dni przed dzisiaj
- [x] refaktor okien details/edit
- [x] BUGFIX: podwójne kliknięcie działa gdziekolwiek w UserList i BookList
- [x] wyświetlanie okładek książek z URL
- [x] BUGFIX: ISBN powinno być unikatowe
- [x] User Card powinien być unikatowy
- [x] BUGFIX: Book Details visual bug
- [x] wyświetl okładkę w Book Details
- [ ] BUGFIX: Borrow Date nie wyświetla się
- [ ] Całą apkę przerobić na angielski/polski
- [ ] BUGFIX: ucina kolumny w BookList przy domyślnym rozmiarze okna i przy resizowaniu
### Ola
- [x] zidentyfikować miejsca gdzie potrzebna jest walidacja i ją dodać
	- np. wprowadzany przy tworzeniu książki ISBN powinien składać się tylko z określonych znaków
- [x] dodać wyświetlanie odpowiednich komunikatów jeżeli input nie spełnia walidacji
- [x] zidentyfikować miejsca gdzie jest potrzebny error handling i go dodać
- [x] zmienić wygląd UI na bardziej "customowy" poprzez dodanie stylizowania elementów itp.
- [x] dodać czcionki
### Wiktoria
- [x] dodać możliwość zwracania kilku książek na raz (poprzez zaznaczenie kilku)
	- wylistować zwrócone książki w Success Dialog
- [x] zmiany w BookList
	- [x] dodać kolumnę z datą wypożyczenia
	- [x] podświetlać na czerwono książki z datą wypożyczenia >30 dni przed dzisiaj
	- [x] dodać kontrolkę po kliknięciu której widać tylko książki z datą wypożyczenia >30 przed dzisiaj
### Natalia
- [ ] połączenie funkcjonalności Scan Books z bazą danych
	- zakładkaScan Books działa jak poprzednia implementacja Return Books
	- wpisanie ISBN powinno wyświetlić propozycje książek do zwrócenia, kliknięcie dodaj dodaje do listy do zwrócenia, a kliknięcie "Zwróć" zwraca książki z tej listy i wyświetla komunikat o sukcesie lub błędzie
	- kliknięcie Enter po wpisaniu ISBN powinno działać tak samo jak kliknięcie przycisku "Dodaj"
