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
- [ ] naprawianie commitów
### Ola
- [ ] dodać funkcjonalność wypożyczania książek z wykorzystaniem bazy danych
	- dodać przycisk dla każdego Usera w zakładce Users, po naciśnięciu którego wyskoczy okno do wypożyczania książek przez tego użytkownika
	- okno wypożyczania zrobione podobnie jak zakładka Return Books, poprzez formularz dodajemy ISBN do listy i klikamy przycisk "Wypożycz" żeby wypożyczyć wiele książek na raz dla wybranego usera
	- po kliknięciu przycisku "Wypożycz" wyświetlić Dialog z komunikatem "Success" lub "Failure"
### Wiktoria
- [x] połączyć funkcjonalność zwracania książek z bazą danych
### Natalia
- [ ] dodać w zakładce "Return Books" filtrowanie po nazwie wypożyczającego
