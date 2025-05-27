namespace WebServicoAPI.JWT
{
    public interface IJWToken
    {
        public string GenerateToken(Guid id, string email);
    }
}
