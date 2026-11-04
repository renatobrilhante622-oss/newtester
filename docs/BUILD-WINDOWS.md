# Windows: compilar o que existe
Instale Git e .NET SDK 10. Abra PowerShell:

    git clone https://github.com/renatobrilhante622-oss/newtester.git
    cd newtester
    git switch feat/android-offline-foundation
    dotnet restore src/MuReborn.Core/MuReborn.Core.csproj
    dotnet build src/MuReborn.Core/MuReborn.Core.csproj -c Release

Isso compila apenas uma biblioteca C#, nao um APK.

# Avaliar cliente MonoGame original separadamente
Instale Android SDK e o workload .NET Android e configure o JDK. Depois:

    cd ..
    git clone https://github.com/xulek/muonline.git muonline-upstream
    cd muonline-upstream
    dotnet workload restore
    dotnet build MuAndroid/MuAndroid.csproj -c Debug

O cliente upstream requer dados graficos que nao estao inclusos e conectividade com servidor para o fluxo padrao. Ainda nao esta integrado ao newtester e nao ha APK offline jogavel. Nao publique assets proprietarios.
