using RestApiNet5.Data.Models;

namespace RestApiNet5.Policies
{
    public static class UserPolicy
    {
        public static bool ReadAll(User auth)
        {
            return true;
        }
    }
}