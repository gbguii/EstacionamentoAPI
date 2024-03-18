using System.Security.Cryptography;
using System.Text;

namespace EstacionamentoV2.Business;

public class CriptografiaSenhaBusiness
{
    private static HashAlgorithm _algoritmo = SHA1.Create();

    public static string CriptografarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
            return string.Empty;

        var encodedValue = Encoding.UTF8.GetBytes(senha);
        var encryptedPassword = _algoritmo.ComputeHash(encodedValue);

        var sb = new StringBuilder();
        foreach (var caracter in encryptedPassword)
        {
            sb.Append(caracter.ToString("X2"));
        }

        return sb.ToString();
    }

    public static bool VerificarSenha(string senhaDigitada, string senhaCadastrada)
    {
        var encryptedPassword = _algoritmo.ComputeHash(Encoding.UTF8.GetBytes(senhaDigitada));

        var sb = new StringBuilder();
        foreach (var caractere in encryptedPassword)
        {
            sb.Append(caractere.ToString("X2"));
        }

        return sb.ToString() == senhaCadastrada;
    }
}
