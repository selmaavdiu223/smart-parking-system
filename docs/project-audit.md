# Project Audit – Smart Parking System

## 1. Përshkrimi i shkurtër i projektit

Smart Parking System është një aplikacion që menaxhon vendet e parkimit duke lejuar krijimin, leximin, përditësimin dhe fshirjen e të dhënave për parkingjet.

Qëllimi i sistemit është të thjeshtojë menaxhimin e vendeve të parkimit dhe të mbajë një evidencë të organizuar të tyre.

Përdoruesit kryesorë janë:

* Administratorët e sistemit
* Persona që menaxhojnë parkingje

Funksionaliteti kryesor përfshin:

* Shtimin e parkingjeve të reja
* Shikimin e listës së parkingjeve
* Përditësimin e të dhënave ekzistuese
* Fshirjen e parkingjeve
* Ruajtjen e të dhënave në file (CSV)

---

## 2. Çka funksionon mirë?

* Projekti ka një strukturë të organizuar duke përdorur ndarjen në Models, Services dhe Repository
* CRUD operacionet janë implementuar dhe funksionojnë siç pritet
* Përdorimi i FileRepository për ruajtjen e të dhënave është i thjeshtë dhe funksional
* Logjika bazë e biznesit është e ndarë nga ruajtja e të dhënave

---

## 3. Dobësitë e projektit

* Mungon validimi i inputeve nga përdoruesi (p.sh. mund të jepen vlera negative ose fusha bosh)
* Error handling është minimal dhe nuk trajton raste si file që mungon ose gabime gjatë leximit/shkrimit
* Nuk ka testime automatike për të verifikuar funksionalitetin
* Nuk ka kontroll për rastet kur kërkohet një ID që nuk ekziston
* Ka mundësi për kod të duplikuar në disa pjesë të projektit
* Struktura mund të përmirësohet për ndarje më të qartë të përgjegjësive midis Service dhe Repository
* Dokumentimi është i kufizuar dhe nuk përfshin udhëzime të plota për përdorim
* Nuk ka trajtim të mirë të gabimeve nga përdoruesi në UI (console)

---

## 4. 3 përmirësime që do t’i implementoj

### Përmirësimi 1 — Validimi i inputeve

Problemi:
Sistemi aktual pranon inpute të pavlefshme si vlera negative, fusha bosh ose formate të gabuara.

Zgjidhja:
Do të implementoj validim në nivel Service për të kontrolluar të gjitha inputet para se të dërgohen në Repository.

Pse ka rëndësi:
Siguron integritetin e të dhënave dhe parandalon gabime gjatë ekzekutimit të aplikacionit.

---

### Përmirësimi 2 — Përmirësimi i Error Handling

Problemi:
Aplikacioni mund të ndalet (crash) nëse file mungon ose ndodh një gabim gjatë leximit/shkrimit.

Zgjidhja:
Do të përdor try-catch për operacionet me file dhe do të shtoj kontroll për ekzistencën e file-it.

Pse ka rëndësi:
E bën sistemin më stabil dhe më të besueshëm për përdorim në situata reale.

---

### Përmirësimi 3 — Përmirësimi i dokumentimit

Problemi:
Dokumentimi aktual është i kufizuar dhe nuk ndihmon përdoruesit e rinj të kuptojnë projektin.

Zgjidhja:
Do të përmirësoj README duke shtuar:

* Udhëzime për instalim
* Shembuj përdorimi
* Përshkrim të strukturës së projektit

Pse ka rëndësi:
E bën projektin më të kuptueshëm dhe më të lehtë për t’u përdorur dhe mirëmbajtur.

---

## 5. Pjesë që nuk e kuptoj plotësisht

Ende nuk e kuptoj plotësisht mënyrën optimale të ndarjes së përgjegjësive midis Service dhe Repository layer, veçanërisht në raste më komplekse.

Gjithashtu, dua të kuptoj më mirë si të strukturoj një arkitekturë më profesionale që mund të zgjerohet lehtë në të ardhmen.
