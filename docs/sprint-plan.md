# Sprint 2 Plan — Avdiu
Data: 1 Prill 2026

## Gjendja Aktuale

- Çka funksionon tani?
  - Shtimi i parkingjeve (Create)
  - Shfaqja e listës së parkingjeve (Read)
  - Ruajtja dhe leximi nga file (parkings.csv)
  - Struktura e projektit me Models, Repository dhe Service

- Çka nuk funksionon?
  - Update/Edit nuk është i implementuar
  - Delete nuk është i implementuar
  - Nuk ka error handling të mirë (programi mund të crashojë)
  - Ka disa warnings në kod

- A kompajlohet dhe ekzekutohet programi?
  - Po, kompajlohet dhe ekzekutohet

## Plani i Sprintit

### Feature e Re
- Do të implementoj funksionin **Search (Kërkim i parkingjeve sipas emrit)**
- Useri do të shkruajë një emër dhe programi do të shfaqë të gjitha parkingjet që përmbajnë atë emër
- Kjo do të implementohet përmes:
  - UI → merr input nga useri
  - Service → bën filtrimin/logjikën
  - Repository → lexon të dhënat nga file

### Error Handling
Do të shtoj trajtim të gabimeve në këto raste:

1. File mungon
   - Nëse parkings.csv nuk ekziston → programi krijon file të ri automatikisht

2. Input i gabuar
   - Nëse useri shkruan tekst në vend të numrit → shfaq mesazh: "Ju lutem shkruani numër valid"

3. ID nuk ekziston
   - Nëse useri kërkon parking që nuk ekziston → shfaq mesazh: "Parking nuk u gjet"

### Teste
Do të krijoj unit tests për:

- Shtim të parkingut valid → duhet të kthejë sukses
- Shtim me emër bosh → duhet të kthejë error/false
- Search:
  - Kur parking ekziston → duhet të gjendet
  - Kur parking nuk ekziston → duhet të mos gjendet

## Afati
- Deadline: Martë, 8 Prill 2026, ora 08:30
