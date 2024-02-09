using EstacionamentoV2.Context;
using Microsoft.EntityFrameworkCore;

namespace EstacionamentoTest;

public class GeradorContextoEmMemoria
{
    public static EstacionamentoContext GerarContexto()
    {
        var options = new DbContextOptionsBuilder<EstacionamentoContext>()
            .UseInMemoryDatabase(databaseName: "Estacionamento")
            .Options;
        EstacionamentoContext context = new(options);
        return context;
    }
}
