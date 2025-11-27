Este é um projeto ASP.NET Core que usa a arquitetura de microserviços. Ele é uma adaptação do tutorial abaixo, mas com diversas modificações para utilizar versões mais recentes das tecnologias.

[![Video Title](https://img.youtube.com/vi/DgVjEo3OGBI/0.jpg)](https://www.youtube.com/watch?v=DgVjEo3OGBI)

As tecnologias utilizadas são:
<ul>
  <li> ASP.NET Core </li>
  <li> Banco de dados SQLServer </li>
  <li> Docker </li>
  <li> Kubernetes </li>
  <li> RabbitMQ </li>
  <li> gRPC </li>
</ul>

# Estrutura do Projeto

O projeto consiste em 2 serviços principais, que são aplicações ASP.NET Core. Uma é o Platforms service e a outra é o Commands service. Essas duas aplicações implementam APIs REST. Ambas aplicações executam como containers em Kubernetes. Elas comunicam entre si e com um usuário externo através de serviços das Kubernetes, como ClusterIP e LoadBalancer.
