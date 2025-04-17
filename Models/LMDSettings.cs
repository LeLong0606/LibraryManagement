namespace LibraryManagement.Models
{
    public class LMDSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public Collections Collections { get; set; } = null!;
    }

    public class Collections
    {
        public string Books { get; set; } = null!;
        public string Admins { get; set; } = null!;
        public string Categories { get; set; } = null!;
        public string Quantities { get; set; } = null!;
    }
}