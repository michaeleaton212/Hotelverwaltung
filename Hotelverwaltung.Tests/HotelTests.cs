using System;
using System.Collections.Generic;
using Xunit;

namespace Hotelverwaltung.Tests
{
    public class HotelTests
    {
        [Fact]
        public void TestGetAvailableRooms()
        {
            // Arrange, prepare test data
            // create two rooms
            var room1 = new Room(1, "Room 101");
            var room2 = new Room(2, "Room 102");

            //
            // create visitor 
            var visitor = new Visitor("Michael");

            // add booking to room2 -> this room will NOT be available
            var booking = new Booking(visitor, room2, DateTime.Now, DateTime.Now.AddDays(1));
            room2.AddBooking(booking);

            // create hotel with both rooms
            var hotel = new Hotel("Hotel Adula", new List<Room> { room1, room2 });

            // time range to check availability
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddHours(2);

            // Act/cal emtod to test
            // call method under test(test which room is free)
            List<Room> available = hotel.GetAvailableRooms(from, to);

            // Assert(/check if result is correct)
            // check that exactly 1 room is available
            Assert.Single(available);              // only one room should be free
            Assert.Equal(1, available[0].RoomID);  // must be room1 (id = 1) start at 0
        }

        [Fact]
        public void TestGetAvailableRooms_NoRoomsAvailable()
        {
            // Arrange: create two rooms (prepare testdata)
            var room1 = new Room(1, "Room 101");
            var room2 = new Room(2, "Room 102");

            // create visitor 
            var visitor = new Visitor("Michael");

            // add booking to both rooms -> none will be available
            var booking1 = new Booking(visitor, room1, DateTime.Now, DateTime.Now.AddDays(1));//these two intentionally overlap
            var booking2 = new Booking(visitor, room2, DateTime.Now, DateTime.Now.AddDays(1));
            room1.AddBooking(booking1);
            room2.AddBooking(booking2);
            // time range to check availability
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddHours(2);
            // create hotel with both rooms, new hotel obcect gets createt
            var hotel = new Hotel("Hotel Adula", new List<Room> { room1, room2 });



            // Act: call method under test (should return no rooms)
            List<Room> available = hotel.GetAvailableRooms(from, to);

            // Assert: check that the list is empty
            Assert.Empty(available);
        }
    }
}
