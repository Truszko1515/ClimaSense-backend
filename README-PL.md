# Spyrosoft Weather Forecast API

## Opis projektu
Ten projekt to aplikacja ASP.NET Core API służąca do generowania tygodniowej prognozy pogody wraz z szacowaniem energii generowanej przez instalacje fotowoltaiczne

---

## Funkcjonalności
- **Prognoza pogody** na 7 dni, w tym:
  - Minimalne i maksymalne temperatury
  - Szacowany czas nasłonecznienia
  - Szacowana ilość energii generowanej w kWh
- **Podsumowanie nadchodzącego tygodnia**:
  -  Minimalna i maksymalna temperatura w nadchodzącym tygodniu
  -  Średnie ciśnienie na poziomie morza oraz na powierzchni danej lokalizacji
  -  Średni czas nasłonecznienia
---

## Technologie
- **.NET 8**
- **ASP.NET Core** (Web API)
- **Docker i Docker Compose**
- **Scrutor** - do implementacji wzorca projektowego **Decorator**
- **IMemoryCache** - mechanizm cachowania w pamięci
---

## Wzorce projektowe i dobre praktyki
- **Wzorzec Decorator**: Zaimplementowany przy użyciu biblioteki Scrutor, aby rozszerzyć funkcjonalność serwisu pogodowego o mechanizm cachowania
- **Separacja warstw**: Aplikacja podzielona na warstwy:
  - **API**: Obsługa żądań HTTP.
  - **Core**: Serwisy i interfejsy, logika aplikacji
  - **Common Infrastructure**: Narzędzia i konfiguracje wspólne dla całego projektu
---

## Instalacja i uruchamianie

### Wymagania
- **Docker** i **Docker Compose**
- .NET SDK 8.0 (do lokalnego uruchamiania aplikacji)

### Uruchamianie aplikacji w Dockerze
1. Zbuduj dockerowy obraz a następnie uruchom kontener na jego podstawie:
   ```bash
   docker-compose up --build
