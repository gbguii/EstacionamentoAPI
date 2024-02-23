using EstacionamentoV2.Context;
using EstacionamentoV2.Model;
using EstacionamentoV2.Repository;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoTest.Repository;

public class UsuarioRepositoryTest
{
    private readonly EstacionamentoContext _context;
    private readonly UsuarioRepository _repository;

    public UsuarioRepositoryTest()
    {
        _context = GeradorContextoEmMemoria.GerarContexto();
        _repository = new UsuarioRepository(_context);
    }

    private async Task AdicionaUsuario()
    {
        UsuarioModel usuario = new()
        {
            Id = 1,
            Login = "ltpt",
            Senha = "ltpt",
            Acesso = "admin"
        };
        List<UsuarioModel> usuarios = await _context.Usuario.ToListAsync();
        foreach (var u in usuarios)
        {
            _context.Usuario.Remove(u);
        }
        await _context.SaveChangesAsync();
        await _repository.CriaUsuario(usuario);
    }

    [Fact]
    public async Task CriaUsuario_ComSucesso()
    {
        // Arrange 
        UsuarioModel usuario = new()
        {
            Login = "ltpt",
            Senha = "ltpt",
            Acesso = "admin"
        };

        // Act
        await _repository.CriaUsuario(usuario);
        UsuarioModel usuarioRetornado = await _repository.RetornaUsuario("ltpt", "ltpt");

        // Assert
        Assert.NotNull(usuarioRetornado);

    }

    [Fact]
    public async Task RetornaUsuario_ComSucesso()
    {
        // Arrange
        await AdicionaUsuario();

        // Act
        UsuarioModel usuarioRetornado = await _repository.RetornaUsuario("ltpt", "ltpt");

        // Assert
        Assert.NotNull(usuarioRetornado);
    }

    [Fact]
    public async Task RetornaUsuarioPorLogin_ComSucesso()
    {
        // Arrange
        await AdicionaUsuario();

        // Act
        UsuarioModel usuarioRetornado = await _repository.RetornaUsuarioPorLogin("ltpt");

        // Assert
        Assert.NotNull(usuarioRetornado);
    }
    
}
