# GUIA PARA O FUNCIONAMENTO DO SISTEMA

-Dentro da Pasta Scripts, deve executar o script "BdTCC_updt (ASP)" no mySql

-Após abrir o projeto no visual studio, vá em Web.config do próprio projeto, procure a <connectionString> com nome "conexaobd" insira no campo vazio o password do banco de dados

-Execute Primeiro a Index(View) da controller Login, vá para a aba consulta funcionários e faça um novo cadastro dele

-Após isso, use o link "Fazer Login" para fazer a autenticação

-Após autenticado, faça o cadastro de um novo cliente pela "Consultar Clientes" e utilize o MESMO CPF que o Funcionário cadastrado para realizar o resto das funções

-Depois de cadastrar o cliente, pode realizar o resto das funções do sistema  (AVISO: Sempre marque o código do item quando realizar um formulário, indo rapidamente em sua consulta pela aba superior, para evitar erro no sistema")

-Caso deseja fazer o teste com outro cliente, faça o logout pelo link "Sair" e repita o 2º e 3º processo listado
