# Smart Parking System – Demo Plan

## 1. Titulli i projektit

Smart Parking System

---

## 2. Problemi që zgjidh

Ky projekt zgjidh problemin e menaxhimit të parkingjeve dhe vendeve të lira.

Në shumë parkingje informacioni ruhet manualisht ose nuk ka sistem të qartë për:
- shtimin e vendeve të parkingut
- kontrollimin e vendeve të lira
- rezervimet
- organizimin e të dhënave

Ky sistem e bën procesin më të shpejtë, më të saktë dhe më të lehtë për menaxhim.

---

## 3. Përdoruesit kryesorë

Sistemi përdoret nga:

- Administratorët e parkingut
- Punëtorët e parkingut
- Bizneset që menaxhojnë parkingje
- Në të ardhmen edhe klientët

---

## 4. Flow-i që do ta demonstroj live

Flow-i kryesor që do të demonstroj është:

Create Parking Spot → List Parking Spots → Update Spot → Delete Spot

Arsyeja pse e zgjodha këtë flow:

- tregon CRUD funksionalitetin e plotë
- tregon strukturën e sistemit
- tregon ruajtjen e të dhënave
- është pjesa më e rëndësishme e projektit aktual

---

## 5. Një problem real që e kam zgjidhur

Problemi:

Ruajtja e vendeve të parkingut nuk ishte e organizuar dhe të dhënat humbnin pas mbylljes së aplikacionit.

Ku ishte problemi:

Në versionin fillestar të projektit të dhënat mbaheshin vetëm në memory.

Si e zgjidha:

Implementova FileRepository që ruan të dhënat në file CSV.

Rezultati:

- të dhënat ruhen permanent
- sistemi është më realist
- CRUD funksionon edhe pas restartimit të aplikacionit

---

## 6. Çka mbetet ende e dobët

Pjesët që duhen përmirësuar:

- UI më moderne
- Login / Authentication
- Rezervime online nga klientët
- Database reale (SQL Server)
- Raporte statistikore

---

## 7. Struktura e prezantimit (5–7 min)

### Hyrja (1 min)

- Prezantoj projektin
- Shpjegoj problemin që zgjidh

### Demo Live (3 min)

- Hap projektin
- Shtoj parking spot të ri
- Shfaq listën
- Përditësoj një spot
- Fshij një spot

### Shpjegimi Teknik (1 min)

- Models
- Repository Pattern
- Services
- Controllers

### Problemi + Zgjidhja (1 min)

- Ruajtja e të dhënave me CSV
- Organizimi i kodit

### Mbyllja (30 sec)

- Çka mund të shtohet në të ardhmen
- Faleminderit

---

## 8. Plan B nëse demo nuk funksionon live

Nëse projekti nuk hapet live:

- Screenshot të aplikacionit
- Kodin në GitHub
- README
- Shpjegim i flow-it me file CSV
- Output nga terminali

---

## 9. Demo Readiness Checklist

- Repo në GitHub e përditësuar
- Projekti build pa errors
- README i qartë
- Flow kryesor funksional
- Screenshot gati
- Praktikuar prezantimi