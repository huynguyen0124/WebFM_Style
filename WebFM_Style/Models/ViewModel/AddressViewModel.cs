namespace WebFM_Style.Models.ViewModel
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? FormattedAddress { get { return $"{Street}, {Ward}, {District}, {City}"; } }
    }
}
