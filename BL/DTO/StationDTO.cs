namespace BL.DTO
{
    public class StationDTO
    {
        public int? Id { get; set; }
        public int? Number { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public List<string>? CarNames { get; set; } = new List<string>();

        public StationDTO() { }

        public StationDTO(int? id,int number, string street, string city, string neighborhood, double x, double y)
        {
            Id = id;
            this.Number = number;
            this.Street = street;
            this.City = city;
            this.Neighborhood = neighborhood;
            this.X = x;
            this.Y = y;
        }
        public StationDTO(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
