namespace PetHelper.Domain
{
    public abstract record BaseModel
    {
        public Guid Id { get; set; }
    }
}
