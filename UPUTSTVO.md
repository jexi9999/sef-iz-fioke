# Šef iz fioke – WPF Projekat: Uputstvo za Setup

## Struktura projekta

```
SefIzFioke/
├── App.xaml + App.xaml.cs
├── MainWindow.xaml + MainWindow.xaml.cs
├── Models/
│   ├── Korisnik.cs
│   ├── Recept.cs
│   └── Komentar.cs
├── Data/
│   └── AppDbContext.cs
├── Helpers/
│   ├── SessionManager.cs
│   ├── NavigationHelper.cs
│   └── PasswordHelper.cs
├── Views/
│   ├── GlavniEkranPage.xaml + .xaml.cs
│   ├── PretragaPage.xaml + .xaml.cs
│   ├── RezultatiPage.xaml + .xaml.cs
│   ├── DetaljiReceptaPage.xaml + .xaml.cs
│   └── UnosReceptaPage.xaml + .xaml.cs
└── Dialogs/
    ├── LoginWindow.xaml + .xaml.cs
    └── RegisterWindow.xaml + .xaml.cs
```

---

## KORAK 1 – Kreiranje projekta u Visual Studio

1. Otvori **Visual Studio 2022**
2. Klikni **Create a new project**
3. Izaberi **WPF Application** (C#, .NET)
4. Klikni **Next**
5. Upiši naziv projekta: `SefIzFioke`
6. **Framework:** .NET 8.0
7. Klikni **Create**

---

## KORAK 2 – Instalacija NuGet paketa

U Visual Studio:  
**Tools → NuGet Package Manager → Package Manager Console**

Upiši ove komande jednu po jednu:

```
Install-Package Microsoft.EntityFrameworkCore.Sqlite
Install-Package Microsoft.EntityFrameworkCore.Tools
```

---

## KORAK 3 – Dodavanje fajlova u projekat

1. U **Solution Explorer** desni klik na projekat
2. **Add → New Folder** – kreiraj foldere: `Models`, `Data`, `Helpers`, `Views`, `Dialogs`
3. Za svaki `.cs` fajl: desni klik na folder → **Add → Existing Item** → izaberi fajl
4. Za svaki `.xaml` fajl: desni klik na folder → **Add → Existing Item** → izaberi XAML fajl

**NAPOMENA:** Kada dodaješ `.xaml` fajl, VS automatski prepoznaje i prateći `.xaml.cs` fajl.

---

## KORAK 4 – Zamena App.xaml i MainWindow.xaml

- Obriši postojeći sadržaj `App.xaml` i zameni ga sa kodom iz ovog projekta
- Isto uradi za `MainWindow.xaml` i `MainWindow.xaml.cs`

---

## KORAK 5 – Popravka namespace-a

U svakom `.cs` fajlu provjeri da namespace odgovara:
```csharp
namespace SefIzFioke.Models     // za Models/
namespace SefIzFioke.Data       // za Data/
namespace SefIzFioke.Helpers    // za Helpers/
namespace SefIzFioke.Views      // za Views/
namespace SefIzFioke.Dialogs    // za Dialogs/
```

---

## KORAK 6 – Kreiranje baze podataka

Baza se **automatski kreira** pri prvom pokretanju aplikacije (u `App.xaml.cs`).

Lokacija baze: `C:\Users\[TvojeIme]\AppData\Local\SefIzFioke\baza.db`

Ako želiš da koristiš EF migracije (opcionalno):

```
Add-Migration InitialCreate
Update-Database
```

---

## KORAK 7 – Pokretanje

Pritisni **F5** ili dugme **▶ Start**. 

Aplikacija ce:
1. Kreirati bazu podataka
2. Otvoriti glavni ekran sa karticom recepta
3. Strelicama < > mozete listati recepte

---

## Ekrani i navigacija

| Ekran | Klasa | Opis |
|-------|-------|------|
| Glavni ekran | `GlavniEkranPage` | Karusel recepata, navigacija |
| Pretraga | `PretragaPage` | Filteri po namirnicama i restrikcijama |
| Rezultati | `RezultatiPage` | Lista pronađenih recepata |
| Detalji | `DetaljiReceptaPage` | Pun prikaz recepta + komentari |
| Unos | `UnosReceptaPage` | Forma za dodavanje recepta |
| Login | `LoginWindow` | Modal dijaloška prijava |
| Registracija | `RegisterWindow` | Modal dijaloška registracija |

---

## Tok korišćenja

```
Glavni ekran
    ├── Klik 🔍  → Pretraga → Rezultati → Detalji recepta
    ├── Klik 📋  → (ako je prijavljen) Unos recepta
    │             (ako nije) → Login dialog
    └── Klik 👤  → Login dialog → (ili) prikaz profila
                    └── Link "Registrujte se" → Register dialog
```

---

## Česte greške

**Greška:** `Could not load file or assembly 'Microsoft.EntityFrameworkCore.Sqlite'`  
**Rešenje:** Ponovo pokrenite NuGet instalaciju: `Install-Package Microsoft.EntityFrameworkCore.Sqlite`

**Greška:** `The type or namespace 'SefIzFioke.Views' could not be found`  
**Rešenje:** Proverite namespace u svakom fajlu i da li su fajlovi dodati u projekat (trebaju se videti u Solution Explorer-u)

**Greška:** Prazan ekran / bela strana  
**Rešenje:** Proverite da li je u `MainWindow.xaml.cs` linija `MainFrame.Navigate(new GlavniEkranPage());` prisutna

---

## Boja teme (za referencu)

```
Pozadina:  #5B8FA8
Header:    #4A7A8F
Dugme:     #2E6B84
Tekst:     White (#FFFFFF)
Kartica:   White (#FFFFFF)
```
