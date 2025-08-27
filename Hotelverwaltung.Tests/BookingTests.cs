using System;
using System.Collections.Generic;
using Xunit;

namespace Hotelverwaltung.Tests
{
    public class HotelBookRoomTests
    {
        [Fact]
        public void BookRoom_Erfolgreich_SpeichertInRoomUndVisitor()// Testing if booking is created and saved in room and visitor
        {
            // arrange (prepare test data)
            var room1 = new Room(1, "Alpenblick");       // test room 1
            var room2 = new Room(2, "Brainstorm");       // test room 2
            var hotel = new Hotel("Hotel Adula", new List<Room> { room1, room2 }); // hotel with rooms
            var visitor = new Visitor("Michael Eaton");   // test visitor
            DateTime from = new DateTime(2025, 12, 01);  // booking start
            DateTime to = new DateTime(2025, 12, 05);  // booking end

            // act (call method under test)
            var booking = hotel.BookRoom(visitor, room1.RoomID, from, to); // create booking

            // assert (check results)
            Assert.NotNull(booking);                    // check if booking was created
            Assert.Same(room1, booking.GetRoom());      // check if both variable show to same room/objekt
            Assert.Equal(from, booking.StartTime);      // check if both dates are the same
            Assert.Equal(to, booking.EndTime);          // check if both dates are the same

            Assert.Single(room1.Bookings);              // check if room/array has exactly 1 booking
            Assert.Same(booking, room1.Bookings[0]);    // check if booking stored in room

            Assert.Single(visitor.GetBookings());       // check if visitor has exactly 1 booking
            Assert.Same(booking, visitor.GetBookings()[0]); // check if booking stored in visitor
        }


        [Fact] // Testing if booking with invalid room id returns null and saves nothing


        public void BookRoom_UngueltigeZimmerId_GibtNullZurueck()
        {
            // arrange (prepare test data)
            var room1 = new Room(1, "Alpenblick");       // room 1
            var room2 = new Room(2, "Brainstorm");       // room 2
            var hotel = new Hotel("Hotel Adula", new List<Room> { room1, room2 }); // hotel with rooms
            var visitor = new Visitor("Michael Eaton");   // visitor
            DateTime from = new DateTime(2025, 12, 01);  // booking start
            DateTime to = new DateTime(2025, 12, 02);  // booking end

            // act (call method under test)
            var booking = hotel.BookRoom(visitor, roomID: 999, startTime: from, endTime: to); // try invalid room id

            // assert (check results)
            Assert.Null(booking);                       // check if no booking created
            Assert.Empty(visitor.GetBookings());        // check if visitor has no bookings
            Assert.Empty(room1.Bookings);               // check if room1 has no bookings
            Assert.Empty(room2.Bookings);               // check if room2 has no bookings
        }


        [Fact] // Testing if returns null if a booking overlaps an existing booking

        public void BookRoom_UeberlappendeBuchung_GibtNullZurueck()
        {
            // 1). sets up the initial situation
            // ARRANGE
            var room1 = new Room(1, "Alpenblick");
            var room2 = new Room(2, "Brainstorm");
            var hotel = new Hotel("Hotel Adula", new List<Room> { room1, room2 });
            var visitorA = new Visitor("Michael Eaton");
            DateTime firstFrom = new DateTime(2025, 12, 01);
            DateTime firstTo = new DateTime(2025, 12, 03);

            // ACT
            var firstBooking = hotel.BookRoom(visitorA, room1.RoomID, firstFrom, firstTo);

            // ASSERT
            Assert.NotNull(firstBooking);//check if booking even got created
            Assert.Single(room1.Bookings);//check if in room alpenblick a booking got created
            Assert.Single(visitorA.GetBookings());//check  if 

            // 2). (testing overlapping booking)
            // ASSERT
            Assert.Same(firstBooking, room1.Bookings[0]);

            // ARRANGE
            var visitorB = new Visitor("Bob");
            DateTime overlapFrom = new DateTime(2025, 12, 02);
            DateTime overlapTo = new DateTime(2025, 12, 04);

            // ACT
            var overlapping = hotel.BookRoom(visitorB, room1.RoomID, overlapFrom, overlapTo);

            // ASSERT
            Assert.Null(overlapping);
            Assert.Single(room1.Bookings);
            Assert.Empty(visitorB.GetBookings());
        }
    }
}
