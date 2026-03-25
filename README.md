# 🅿️ Smart Parking Management System

## 📌 Përshkrimi

Smart Parking Management System është një aplikacion full-stack për menaxhimin e një parkingu. Sistemi mundëson regjistrimin e hyrjes dhe daljes së veturave, llogaritjen automatike të pagesës dhe monitorimin në kohë reale të vendeve të parkingut.

Ky projekt po zhvillohet gjatë një semestri për të demonstruar përdorimin e arkitekturës së shtresuar dhe praktikave moderne të zhvillimit.

---

## 🎯 Funksionalitetet Kryesore

### 🔴 Must Have

* Login për Admin dhe Staff
* Regjistrimi i hyrjes së veturës (Check-in)
* Regjistrimi i daljes së veturës (Check-out)
* Llogaritja automatike e pagesës
* Shfaqja e vendeve të lira dhe të zëna

### 🟠 Should Have

* Raport ditor i të ardhurave

### 🟡 Could Have

* Grafik mujor i të ardhurave

---

## 👥 Rolet e Sistemit

* **Admin**

  * Monitoron sistemin
  * Shikon raporte dhe statistika

* **Staff**

  * Regjistron hyrje/dalje të veturave
  * Menaxhon pagesat

---

## ⚙️ Teknologjitë

### Backend

* ASP.NET Core Web API
* C#

### Frontend

* HTML
* CSS
* JavaScript

### Database

* PostgreSQL

### Arkitektura

* Layered Architecture (Models, Services, Data, UI)
* Repository Pattern

---

## 🏗️ Struktura e Projektit

```
/Models       -> Entitetet kryesore (User, ParkingSpot, ParkingSession)
/Services     -> Logjika e biznesit
/Data         -> Repository dhe menaxhimi i të dhënave
/UI           -> Frontend (HTML, CSS, JS)
/docs         -> Dokumentimi (UML, Architecture)
```

---

## 🧠 Arkitektura

Projekti përdor arkitekturë me shtresa për të ndarë përgjegjësitë:

* **Models** – përfaqësojnë të dhënat
* **Services** – përmbajnë logjikën e biznesit
* **Data** – menaxhon ruajtjen e të dhënave (Repository Pattern)
* **UI** – ndërfaqja me përdoruesin

---

## 📊 UML & Dokumentimi

Dokumentimi gjendet në folderin `/docs`:

* `class-diagram.md` – diagrami i klasave (UML)
* `architecture.md` – përshkrimi i arkitekturës

---

## 🔐 Parimet e Dizajnit

Projekti ndjek parimin:

* **Single Responsibility Principle (SOLID)**
  Çdo klasë ka një përgjegjësi të vetme, duke e bërë kodin më të mirëmbajtshëm dhe të zgjerueshëm.

---

## 🚀 Si të ekzekutohet projekti

1. Klono repository:

```
git clone https://github.com/username/smart-parking-system.git
```

2. Hape projektin në Visual Studio

3. Run aplikacionin:

```
dotnet run
```

4. Hape në browser:

```
https://localhost:xxxx/swagger
```

---

## 📌 Statusi i Projektit

🟡 Në zhvillim (Semester Project)

---

## 👨‍💻 Autori

* Selma Avdiu