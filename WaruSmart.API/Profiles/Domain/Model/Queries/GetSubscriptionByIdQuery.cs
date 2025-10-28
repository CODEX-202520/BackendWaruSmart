namespace WaruSmart.API.Profiles.Domain.Model.Queries
{
    public class GetSubscriptionByIdQuery
    {
        public int Id { get; private set; }

        public GetSubscriptionByIdQuery(int id)
        {
            Id = id;
        }
    }
}