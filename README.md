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
- [ ] połączyć funkcjonalność zwracania książek z modelami Books i Users

## TODOs na zjazd 7
### Backlog
- przenieść logikę z code behind do view modeli (w userlist i booklist)
- dodać walidację i error handling gdzie potrzebne
- dodać bazę danych
- dodać funkcjonalność wypożyczania książek (po kliknięciu na usera okno do wpisywania ISBN)
- dodać możliwość edycji wielu książek i userów na raz (kilka edytowanych książek/userów powinno otwierać się jako zakładki w jednym oknie details)
