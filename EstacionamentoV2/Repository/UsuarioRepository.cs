using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoV2.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly EstacionamentoContext _context;
    public UsuarioRepository(EstacionamentoContext context)
    {
        _context = context;
    }

    public async Task CriaUsuario(UsuarioModel usuario)
    {
        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<UsuarioModel> RetornaUsuario(string login, string senha)
    {
        return await _context.Usuario.FirstOrDefaultAsync(u => u.Login == login && u.Senha == senha);
    }

    public async Task<UsuarioModel> RetornaUsuarioPorLogin(string login)
    {
        return await _context.Usuario.FirstOrDefaultAsync(u => u.Login == login);
    }
}
