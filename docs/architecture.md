# Architecture Documentation

## Layers

### 1. Models
Përmban klasat kryesore të sistemit si User, ParkingSpot dhe ParkingSession.

### 2. Services
Përmban logjikën e biznesit si:
- Regjistrimi i hyrjes/daljes
- Llogaritja e pagesës

### 3. Data
Përgjegjëse për ruajtjen dhe leximin e të dhënave.
Përdoret Repository Pattern për të ndarë logjikën nga storage.

### 4. UI
Përmban pjesën vizuale të aplikacionit (HTML, CSS, JS).

---

## Design Decisions

- U përdor Repository Pattern për të ndarë logjikën nga data storage.
- Struktura me shtresa rrit mirëmbajtjen dhe testimin.
- Projekti është i ndërtuar në mënyrë modulare për zgjerim në të ardhmen.

---

## SOLID Principle (BONUS)

Single Responsibility Principle:
Çdo klasë ka një përgjegjësi të vetme.
P.sh:
- ParkingService merret vetëm me logjikën e parkingut
- Repository merret vetëm me data