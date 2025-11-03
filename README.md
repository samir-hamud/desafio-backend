## Desafio 

### Backend utilizando DDD, clean archicteture, SOLID e boas práticas.
#### Asp.Net com EntityFramework, Postgresql e Rabbitmq
#### Necessário ter Docker instalado e executar o docker-compose antes de iniciar o projeto.

#### Os testes integrados também precisam do Docker executando, porém é criado container temporário em execução (utilizando o pacote Testcontainers).
  - #### Os testes integrados testarão todos os endpoints, verificando se o resultado no banco de dados é o esperado.
#### Os testes unitários utilizam do sqlite em memória, pra cada teste é recriada a base de dados.

#### O serviço para consumir as mensagens da mensageria é instanciado em um background service.

#### A modelagem do EntityFramework é feita pela FluentAPI nos arquivos de configurações
#### É utilizado também FluentValidations em alguns pontos dos services

