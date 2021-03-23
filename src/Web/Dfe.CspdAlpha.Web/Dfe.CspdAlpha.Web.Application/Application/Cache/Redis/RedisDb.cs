namespace Dfe.Rscd.Web.Application.Application.Cache.Redis
{
    public static class RedisDb
    {
        /// <summary>
        /// For general data (in reality points to General)
        /// </summary>
        public const int Default = -1;

        /// <summary>
        /// For general data + legacy components
        /// </summary>
        public const int General = 0;
    }
}
