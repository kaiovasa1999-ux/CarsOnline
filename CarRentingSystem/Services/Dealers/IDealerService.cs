namespace CarRentingSystem.Services.Dealer
{
    public interface IDealerService
    {
        public bool IsDealer(string userId);
        public int GetIdByUser(string userId);
        public string BecomeDealer(string dealerName,string phoneNumber);
    }
}
