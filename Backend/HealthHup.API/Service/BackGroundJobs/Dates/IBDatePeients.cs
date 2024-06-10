namespace HealthHup.API.Service.BackGroundJobs.Dates
{
    public interface IBDataPeients
    {
        //Day
        public Task DeleteOldDates();
        Task AlertDate();
        //Mount
        public Task DeleteOldDisease();
        
    }
}
