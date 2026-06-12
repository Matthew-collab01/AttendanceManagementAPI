namespace AttendanceManagementAPI.Models
{
    public class AttendanceRequest
    {

            public string name { get; set; } = string.Empty;
            public int present { get; set; }

            public int absent { get; set; }
        
    }
}

