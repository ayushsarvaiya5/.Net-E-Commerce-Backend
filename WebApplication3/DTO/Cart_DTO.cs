namespace WebApplication3.DTO
{
    public class CartAddDTO
    {
        public int MobileId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MobileId { get; set; }
        public string MobileName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
