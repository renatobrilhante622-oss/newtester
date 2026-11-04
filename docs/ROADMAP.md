# Plano de execução — MU Reborn

## Portão 0: auditoria obrigatória
- Confirmar licença do repositório xulek/muonline e de cada dependência.
- Identificar dados proprietários de MU Online: não copiar ou redistribuir sem autorização.
- Validar versão do .NET, MonoGame, Android SDK e pipeline de build.
- Verificar se o cliente depende de servidor para entrar no mundo.

## PR 1: fundação (atual)
- Criar contratos independentes de motor gráfico para simulação, comandos e saves.
- Documentar critérios de aceite para Android offline.
- Não afirmar APK pronto antes de compilação e teste em dispositivo.

## PR 2: cliente Android
- Integrar somente código/recursos cuja licença permita.
- Abrir cena 3D e renderizar personagem/terreno com assets próprios ou licenciados.
- Joystick touchscreen, câmera, opções de qualidade.

## PR 3: loop offline
- IA de monstros, combate determinístico, drops e inventário.
- Save local SQLite com versão e migrações.
- Farm automático com raio máximo, poções e botão parar.

## PR 4: multiplayer
- Servidor autoritativo, autenticação e sincronização.
- Saves offline não se tornam itens online confiáveis automaticamente.

## Critérios mínimos do primeiro APK
- Instalação limpa no Android, inicialização sem internet.
- 30 minutos de gameplay sem crash, controle por toque.
- Monstro com IA e combate, progresso persistente após reinício.
- Medição de FPS, memória e tempo de carregamento em aparelho real.
