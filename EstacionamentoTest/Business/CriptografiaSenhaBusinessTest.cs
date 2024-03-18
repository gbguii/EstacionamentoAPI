using System.Security.Cryptography;
using EstacionamentoV2.Business;

namespace EstacionamentoTest.Business;

public class CriptografiaSenhaBusinessTest
{

    [Fact]
    public void CriptografarSenha_ComSucesso()
    {
        // Arrange
        string senha = "123456";

        // Act
        String senhaCriptografada = CriptografiaSenhaBusiness.CriptografarSenha(senha);

        // Assert
        Assert.NotEmpty(senhaCriptografada);
    }

    [Fact]
    public void CriptografarSenha_SemSucesso_SenhaVazia()
    {
        // Arrange
        string senha = "";

        // Act
        String senhaCriptografada = CriptografiaSenhaBusiness.CriptografarSenha(senha);

        // Assert
        Assert.Empty(senhaCriptografada);
    }

    [Fact]
    public void DescriptografarSenha_ComSucesso()
    {
        // Arrange
        string senha = "123456";
        string senhaCriptografada = CriptografiaSenhaBusiness.CriptografarSenha(senha);

        // Act
        bool senhaValida = CriptografiaSenhaBusiness.VerificarSenha(senha, senhaCriptografada);

        // Assert
        Assert.True(senhaValida);
    }

    [Fact]
    public void DescriptografarSenha_SemSucesso_SenhaInformada_Incorreta()
    {
        // Arrange
        string senha = "123456";
        string senhaCriptografada = CriptografiaSenhaBusiness.CriptografarSenha(senha);

        // Act
        bool senhaValida = CriptografiaSenhaBusiness.VerificarSenha("1234567", senhaCriptografada);

        // Assert
        Assert.False(senhaValida);
    }

}
