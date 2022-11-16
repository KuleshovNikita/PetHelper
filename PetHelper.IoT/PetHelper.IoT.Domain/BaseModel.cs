namespace PetHelper.IoT.Domain
{
    public abstract record BaseModel
    {
        public Guid Id { get; set; }
    }
}
