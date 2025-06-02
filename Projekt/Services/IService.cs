namespace Projekt.Services
{
    public interface IService
    {
        public static IService? Instance { get; }
        public static bool IsValid { get; }
    }
}
