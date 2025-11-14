namespace ApiEnCouches.Controllers;

public static class Routes
{
    public const string Auth = "Auth";
    public const string Users = "Users";
    public const string MeetingRooms = "MeetingRooms";
    public const string Reservations = "Reservations";

    public static class AuthRoutes
    {
        public const string Register = "Register";
        public const string Login = "Login";
        public const string Refresh = "Refresh";
        public const string Logout = "Logout";
    }

    public static class UserRoutes
    {
        public const string GetAll = "";
        public const string GetById = "{userId}";
        public const string Create = "";
        public const string Update = "{userId}";
        public const string Delete = "{userId}";
    }

    public static class MeetingRoomRoutes
    {
        public const string GetAll = "";
        public const string GetById = "{roomId}";
        public const string Create = "";
        public const string Delete = "{roomId}";
        public const string Availability = "Disponibility/{roomId}";
    }

    public static class ReservationRoutes
    {
        public const string GetAll = "";
        public const string GetById = "{reservationId}";
        public const string Create = "";
        public const string Update = "{reservationId}";
        public const string Delete = "{reservationId}";
    }
}
