using System.Text.RegularExpressions;

namespace EstacionamentoV2.Business;

public class Validacoes
{
    public static bool ValidaPlacaVeiculo(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa)) { return false; }

        if (placa.Length > 7) { return false; }

        placa = placa.Replace("-", "").Trim();

        /*
         *  Verifica se o caractere da posição 4 é uma letra, se sim, aplica a validação para o formato de placa do Mercosul,
         *  senão, aplica a validação do formato de placa padrão.
         */
        if (char.IsLetter(placa, 4))
        {
            /*
             *  Verifica se a placa está no formato: três letras, um número, uma letra e dois números.
             */
            var padraoMercosul = new Regex("[a-zA-Z]{3}[0-9]{1}[a-zA-Z]{1}[0-9]{2}");
            return padraoMercosul.IsMatch(placa);
        }
        else
        {
            // Verifica se os 3 primeiros caracteres são letras e se os 4 últimos são números.
            var padraoNormal = new Regex("[a-zA-Z]{3}[0-9]{4}");
            return padraoNormal.IsMatch(placa);
        }
    }
}
