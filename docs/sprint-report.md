# Sprint 2 Report — [Emri Yt]

## Çka Përfundova
- Implementova feature për kërkim të parkingjeve sipas emrit (Search by Name)
- Shtova validime për input (emër bosh, çmim invalid)
- Implementova error handling në Service dhe Repository (try-catch)
- Programi nuk crashon për inpute të gabuara
- Shtova unit teste për funksionalitetin kryesor (Search dhe Add)
- Projekti është i ndarë në shtresa: UI → Service → Repository

## Shembull Output
- Kërkim: "TestParking" → kthen rezultatet e sakta
- Kërkim: "XYZ123" → shfaq "Asnje parking nuk u gjet"

## Çka Mbeti
- Nuk ka mbetur asgjë për këtë sprint

## Çka Mësova
- Si të organizoj kodin në shtresa (architecture)
- Si të përdor try-catch për error handling
- Si të validroj input nga useri
- Si të krijoj dhe ekzekutoj unit teste me xUnit
- Si të përdor Git dhe GitHub për menaxhim projekti

## Vështirësitë
- Probleme me konfigurimin e test project (xUnit)
- Gabime me namespace dhe emra të projekteve
- Probleme me ekzekutimin e testeve në Git Bash

## Si i Zgjidha
- Duke përdorur komandat dotnet për instalim të paketave
- Duke korrigjuar emrat e projekteve dhe folderëve
- Duke testuar hap pas hapi deri sa funksionoi

## Përmirësime të Mundshme
- Shtimi i filtrimit sipas çmimit
- Shtimi i statistikave (mesatare, max, min)
- UI më i avancuar (menu më interaktive)