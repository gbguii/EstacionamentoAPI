@echo off
chcp 65001 > nul

dotnet tool install -g dotnet-reportgenerator-globaltool

echo Limpamdo arquivos temporários...
for /d /r %%i in (*) do (
    if exist "%%i\TestResults" (
        rmdir /s /q "%%i\TestResults"
        echo Excluída a pasta TestResults em: %%i
    )
)
dotnet clean
dotnet build
dotnet test --collect:"XPlat Code Coverage"

reportgenerator -reports:./**/coverage.cobertura.xml -targetdir:coverage_report -filefilters:-**Moq** -assemblyFilters:-*.Tests

echo Limpamdo arquivos temporários...
for /d /r %%i in (*) do (
    if exist "%%i\TestResults" (
        rmdir /s /q "%%i\TestResults"
        echo Excluída a pasta TestResults em: %%i
    )
)

cls

cd coverage_report

set isEmpty=true
for /f %%A in ('dir /b /a-d ^| find /c /v ""') do (
    if %%A gtr 0 set isEmpty=false
)

if %isEmpty%==true (
    echo Não foram encontrados projetos de testes ou estão com erro.
) else (
    start index.html
    echo O relatório de cobertura foi aberto no navegador.
)
echo Finalizado. 