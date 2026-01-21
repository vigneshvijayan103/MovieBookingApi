🎬 Movie Ticket Booking API

An ASP.NET Core Web API that demonstrates **seat availability management and concurrency-safe booking** for a movie show.

This project focuses on **seat holding, booking, and preventing double booking**

---

 Features

- Create movie shows with auto-generated seats
- View seat availability
- Temporarily hold seats
- Confirm booking for held seats
- Auto-release expired seat holds using a background job
- Concurrency-safe booking logic

---

 Seat Lifecycle

- **Available**: Seat is free
- **Hold**: Seat is temporarily locked
- **Booked**: Seat is permanently booked
---

 Booking Flow

The system follows this flow:

1. **Create Show**
   - A show is created
   - Seats are auto-generated

2. **Get Seat Status**
   - Users can view current seat availability

3. **Hold Seats**
   - Selected seats are temporarily held
   - Only seats in `Available` state can be held
   - Hold time is recorded

4. **Confirm Booking**
   - Only seats in `Hold` state can be booked
   - Hold expiry is validated
   - Seats are marked as `Booked` atomically

5. **Auto-Release Expired Holds**
   - A background job releases seats whose hold duration has expired
  ---


## ⚙️ Setup & Configuration

### Prerequisites
- .NET SDK (6/7/8)
- SQL Server or LocalDB
- Visual Studio / VS Code

### Configuration
`appsettings.json` is **not committed** to the repository.

1. Copy the sample file:
2. Rename to:appsettings.json
3. Update the connection 

---

 Database Setup

Run Entity Framework migrations:

```bash
dotnet ef database update




