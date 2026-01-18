namespace UserManagement.Application
{
    public class PageResult<T>
    {
        public int TotalRecord { get; set; }

        public List<T> Result { get; set; }
    }
}
