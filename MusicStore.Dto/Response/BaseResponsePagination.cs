namespace MusicStore.Dto.Response;

public class BaseResponsePagination<T> : BaseResponse
{
    public ICollection<T>? Collection { get; set; }

    public int TotalPages { get; set; }
}