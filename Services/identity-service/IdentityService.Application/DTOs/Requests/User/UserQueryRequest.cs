namespace IdentityService.Application.DTOs.Requests.User
{
    public class UserQueryRequest
    {
        public string? SearchTerm { get; set; } // Tìm kiếm theo tên, email, hoặc username
        public int PageNumber { get; set; } = 1; // Trang hiện tại
        public int PageSize { get; set; } = 10; // Số lượng bản ghi trên mỗi trang
    }
}
