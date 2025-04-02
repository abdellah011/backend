namespace Admin.Models
{
    public class UserStatsDto
    {
        public DateTime Date { get; set; }
        public int TotalUsers { get; set; }
        public int ConnectedUsers { get; set; }
        public int DisconnectedUsers => TotalUsers - ConnectedUsers;
    }
}
