# Guia de Apresentacao da Aplicacao

## Objetivo deste documento

Este documento serve como apoio para apresentar a aplicacao para outro desenvolvedor.
Ele complementa o arquivo `ARQUITETURA.md`, mas com uma abordagem mais pratica: o que a API faz, como o fluxo acontece e por que a estrutura foi organizada dessa forma.

## O que a aplicacao faz

Esta aplicacao expoe uma API REST para gerenciamento de produtos.

Na pratica, ela permite:

- cadastrar produtos
- consultar produtos por id
- listar produtos com paginacao e filtros
- atualizar produtos existentes
- excluir produtos de forma logica

O recurso principal do sistema e `Produto`, com os seguintes dados:

- `Nome`
- `Sku`
- `Descricao`
- `Preco`
- `QuantidadeEstoque`
- `Ativo`

Apesar de o dominio atual ser simples, a estrutura foi pensada para crescer sem misturar regra de negocio com detalhes de API ou banco de dados.

## Como eu apresentaria a aplicacao em poucos minutos

Uma forma objetiva de explicar a aplicacao e:

"A solucao implementa uma API de produtos usando separacao por camadas. A camada Api recebe a requisicao HTTP e devolve a resposta padronizada. A camada Application concentra os casos de uso, validacoes e contratos. A camada Domain representa o nucleo do negocio. A camada Infrastructure implementa persistencia, configuracao do Entity Framework, repositorios e inicializacao do banco."

Depois disso, vale complementar:

- a API ja sobe com Swagger
- o banco recebe migrations na inicializacao
- existe carga inicial de dados para facilitar teste e demonstracao
- a exclusao e logica, preservando historico
- a troca entre SQLite e PostgreSql fica isolada na infraestrutura

## Estrutura da solucao

### Api

Responsavel pela entrada HTTP da aplicacao.

Aqui ficam:

- controllers
- configuracao de servicos web
- middlewares
- contratos de resposta da API

O papel dessa camada e receber a chamada, delegar para a aplicacao e devolver a resposta certa. Ela nao deve conter regra de negocio.

### Application

Responsavel pelos casos de uso.

Aqui ficam:

- servicos de aplicacao
- DTOs de entrada e saida
- validadores
- mapeamentos
- interfaces de repositorios e servicos tecnicos

Essa camada orquestra o comportamento da aplicacao. E nela que a regra de uso aparece com mais clareza, por exemplo:

- normalizar campos antes de persistir
- impedir cadastro de SKU duplicado
- buscar um produto e lancar erro quando ele nao existe
- coordenar gravacao por meio da unidade de trabalho

### Domain

Responsavel pelo nucleo do negocio.

Hoje ele tem uma modelagem enxuta, mas importante:

- entidade `Produto`
- base auditavel com identificador, datas e dados de exclusao

O objetivo dessa camada e manter o modelo central desacoplado de framework, banco e transporte HTTP.

### Infrastructure

Responsavel pelos detalhes tecnicos.

Aqui ficam:

- `DbContext`
- configuracoes de entidade
- repositorios
- unidade de trabalho
- seed de dados
- servicos tecnicos
- configuracao de provider do banco

Essa camada conhece Entity Framework Core, migrations, SQLite e PostgreSql. As demais camadas nao precisam conhecer esses detalhes.

## Fluxo da requisicao

O fluxo principal da aplicacao pode ser explicado assim:

1. o cliente envia uma requisicao HTTP para a API
2. o controller recebe a chamada e delega para um servico da camada Application
3. a camada Application valida e executa o caso de uso
4. quando precisa de dados, ela usa interfaces de persistencia
5. a Infrastructure implementa essas interfaces com Entity Framework Core
6. o resultado volta para a Application, que monta o retorno
7. a Api devolve uma resposta padronizada para o cliente

Esse fluxo e importante para mostrar que cada camada tem um papel claro.

## Exemplo de fluxo: criacao de produto

Se eu precisasse demonstrar um fluxo de ponta a ponta, usaria o cadastro de produto:

1. o endpoint recebe um `POST /api/v1/produtos`
2. o model binding monta o DTO de entrada
3. a validacao garante que os campos obrigatorios e limites estejam corretos
4. o servico de aplicacao normaliza os dados, por exemplo o SKU em caixa alta
5. o servico consulta o repositorio para verificar duplicidade de SKU
6. se estiver tudo certo, o produto e criado e persistido
7. o contexto aplica auditoria automaticamente no `SaveChangesAsync`
8. a API retorna `201 Created` com o payload padronizado

Esse exemplo mostra bem a separacao de responsabilidades e o motivo da arquitetura.

## Decisoes de arquitetura e o motivo de cada uma

### 1. Separacao em camadas

Foi adotada para manter responsabilidade bem definida.

Motivos:

- facilita manutencao
- melhora legibilidade
- reduz acoplamento
- permite trocar detalhes tecnicos sem reescrever regra de negocio
- deixa a evolucao mais segura para novos recursos

Para uma prova, isso ajuda a demonstrar criterio de organizacao e nao apenas a entrega de endpoints funcionando.

### 2. Regra de negocio na Application

A regra de uso foi centralizada na camada Application em vez de ficar no controller.

Motivos:

- controller deve ser fino
- regras ficam reaproveitaveis
- testes ficam mais simples
- o fluxo de negocio fica explicito em um unico lugar

### 3. Domain enxuto

O dominio atual e pequeno, entao a modelagem foi mantida simples.

Motivos:

- evitar complexidade artificial
- representar apenas o que o problema realmente precisa
- deixar espaco para crescer quando o dominio ficar mais rico

Ou seja: a estrutura e preparada para evolucao, mas sem exagerar no desenho atual.

### 4. Infrastructure isolando persistencia

Toda a parte de banco e provider foi colocada na infraestrutura.

Motivos:

- proteger Application e Domain de detalhes de EF Core
- permitir troca de SQLite para PostgreSql por configuracao
- concentrar migrations, repositorios e configuracoes em um unico lugar tecnico

Essa escolha deixa claro que o banco e um detalhe de implementacao, nao o centro da aplicacao.

### 5. FluentValidation

Foi usado para validacao de entrada.

Motivos:

- separa validacao do controller
- deixa regras declarativas
- melhora leitura e manutencao
- padroniza respostas de erro

### 6. AutoMapper

Foi usado para mapear DTOs e entidades.

Motivos:

- evita codigo repetitivo de conversao
- deixa a camada Application mais focada na regra
- reduz ruido em fluxos de create, update e leitura

### 7. Resposta padronizada da API

As respostas foram encapsuladas em um contrato comum.

Motivos:

- consistencia para quem consome a API
- espaco para metadados de paginacao
- retorno de `traceId` para suporte e observabilidade
- tratamento uniforme de sucesso e erro

### 8. Middlewares para preocupacoes transversais

Middlewares foram usados para itens que nao pertencem ao caso de uso em si.

Exemplos:

- correlacao de requisicao
- logging
- cabecalhos de seguranca
- tratamento centralizado de excecoes

Motivo principal: evitar repeticao e manter controller e servico focados no que interessa.

### 9. Exclusao logica e auditoria

A exclusao de produto nao remove o registro fisicamente. Em vez disso, marca o item como excluido e registra data e usuario.

Motivos:

- preserva historico
- facilita rastreabilidade
- evita perda definitiva de dados por operacao de negocio

Isso e reforcado por filtro global no Entity Framework para nao retornar registros excluidos nas consultas normais.

### 10. Seed inicial de dados

Na subida da aplicacao, o banco aplica migrations e, se estiver vazio, recebe produtos iniciais.

Motivos:

- acelerar demonstracao
- facilitar testes locais
- permitir avaliar a API sem preparo manual previo

## Pontos que mostram maturidade tecnica

Se eu quisesse destacar escolhas de engenharia na apresentacao, eu enfatizaria estes pontos:

- o controller nao concentra regra de negocio
- a validacao nao esta espalhada pela API
- a troca de banco foi prevista desde o desenho da infraestrutura
- a resposta HTTP segue um formato consistente
- a aplicacao registra auditoria automaticamente
- a exclusao logica evita perda de historico
- o projeto ja sobe pronto para uso, com migration e seed

Esses pontos mostram preocupacao com manutencao, evolucao e operacao da aplicacao, nao apenas com o CRUD basico.

## O que eu diria sobre escalabilidade e evolucao

Mesmo sendo uma prova e tendo um escopo pequeno, a estrutura permite evoluir para cenarios maiores.

Exemplos de evolucao natural:

- criar novos modulos alem de produtos
- adicionar autenticacao e autorizacao
- incluir eventos de dominio ou integracao
- introduzir cache e observabilidade mais avancada
- trocar ou expandir o banco sem reescrever a camada de aplicacao

O ponto importante aqui e: a organizacao foi feita para que o crescimento nao force a mistura de responsabilidades.

## Trade-off assumido

Existe um custo em usar varias camadas em uma aplicacao pequena: mais projetos, mais interfaces e mais arquivos.

Mesmo assim, a escolha faz sentido nesta prova porque:

- demonstra criterio arquitetural
- evidencia preocupacao com boas praticas
- deixa claro onde cada decisao tecnica deve ficar
- prepara a base para crescimento real

Entao o desenho busca equilibrio: nao complica o dominio sem necessidade, mas organiza a solucao como uma aplicacao profissional.

## Roteiro rapido para apresentar

Se eu precisasse explicar de forma objetiva para outro desenvolvedor, seguiria esta ordem:

1. explicar que e uma API REST para gestao de produtos
2. mostrar as quatro camadas e o papel de cada uma
3. detalhar um fluxo simples, como criar ou listar produtos
4. destacar validacao, resposta padronizada, auditoria e exclusao logica
5. explicar por que o banco ficou isolado na infraestrutura
6. encerrar mostrando que a estrutura favorece manutencao e evolucao

## Fechamento

Em resumo, a aplicacao foi estruturada para separar bem entrada HTTP, casos de uso, modelo de dominio e detalhes de infraestrutura.

Ela resolve um problema simples, mas foi organizada de forma intencional para demonstrar clareza arquitetural, facilidade de manutencao e preparo para evolucao.