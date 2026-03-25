# UML Class Diagram

## User
- id: int
- username: string
- password: string
- role: string

+ Login()

---

## ParkingSpot
- id: int
- isOccupied: bool

+ Occupy()
+ Free()

---

## ParkingSession
- id: int
- plateNumber: string
- entryTime: DateTime
- exitTime: DateTime
- totalPrice: decimal

+ CalculatePrice()

---

## Relationships
- ParkingSession -> ParkingSpot (1:1)
- User -> System (Admin/Staff roles)
