namespace SecretSanta.Domain
{
    public interface IRandom
    {
        int Next();
        int Next(int max);
    }
}