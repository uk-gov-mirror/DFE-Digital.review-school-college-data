namespace Dfe.Rscd.Api.Models
{
    public class GetResponse<T>
    {
        public T Result { get; set; }

        public Error Error { get; set; }
    }
}